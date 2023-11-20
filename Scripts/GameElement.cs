using Godot;
using System;

public partial class GameElement : Node2D
{
	public enum GameElementType
	{
		Green, Blue, Red
	}
	
	[Export] private int[] _winLevelPoints = {0,1,3,9,27};
	[Export] private Node2D _effect;
	[Export] private DraggableComponent _draggableComponent;
	private Sprite2D _image;
	private GamePresenter _game;
	private Vector2 _destination;
	private Godot.Collections.Array<Node2D> _nodesCollided = new Godot.Collections.Array<Node2D>();
	private bool _isMoving = false;
	
	private int _num;
	private GameElementType _type;
	private int _level = 1;
	
	public int Num => _num;
	public GameElementType Type => _type;
	public int Level => _level;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_image = GetNode<Sprite2D>("Image");
		//_effect = GetNode<Node2D>("GPUParticleEffect");
		//_draggableComponent= GetNode<DraggableComponent>("DraggableComponent");
		_draggableComponent.StopDrag += _OnStopDrag;
		_draggableComponent.DoubleClick += _OnDoubleClick;
		Area2D area = GetNode<Area2D>("Area2D");
		area.AreaEntered += _OnCollisionEnter;
		area.BodyEntered += _OnCollisionEnter;
		area.AreaExited += _OnCollisionExited;
		area.BodyExited += _OnCollisionExited;
	}
	
	public void Init(int num, GameElementType type, int level, GamePresenter game)
	{
		_num = num;
		_type = type;
		_level = level;
		_game = game;
		SetLabelLevel();
	}
	
	public void SetNum(int num)
	{
		_num = num;
	}
	
	private void _OnCollisionEnter(Node2D nodeWith)
	{
		_nodesCollided.Add(nodeWith);
	}
	
	private void _OnCollisionExited(Node2D nodeWith)
	{
		_nodesCollided.Remove(nodeWith);
	}
	
	private void _OnStopDrag()
	{
		Node2D nodeCollided = null;
		if (_nodesCollided.Count > 0) nodeCollided = _nodesCollided[0];
		
		if (nodeCollided != null)
		{
			GameElement elemWith = (GameElement)(nodeCollided.GetParent());
			if (elemWith.Compare(_type, _level))
			{
				elemWith.AddLevel();
				_game.RemoveElement(this); 
			}
			else
			{
				elemWith.SetDestination(_draggableComponent.PreviousPos);
				_SetCellPosition(elemWith.Num);
				int t = elemWith.Num;
				elemWith.SetNum(_num);
				SetNum(t);
			}
		}
		else
		{
			_SetCellPosition();
			_game.ChangeElementCell(this);
		}
	}
	
	private void _SetCellPosition(int num)
	{
		float x = num % 8;
		float y = num / 8;
		this.Position = new Vector2(x * 75, y * 75);
	}
	
	private void _SetCellPosition()
	{
		float x = this.Position.X + 38;
		float y = this.Position.Y + 38;
		this.Position = new Vector2(x - x % 75, y - y % 75);
	}
	
	private void _OnDoubleClick()
	{
		if (_level == 4)
		{
			GameElementType type = 0;
			int r = GD.RandRange(0,4);
			if (_type == GameElementType.Green)
			{
				if (r == 0) type = GameElementType.Blue;
				else type = GameElementType.Red;
			}
			if (_type == GameElementType.Blue)
			{
				if (r == 0) type = GameElementType.Red;
				else type = GameElementType.Green;
			}
			if (_type == GameElementType.Red)
			{
				if (r == 0) type = GameElementType.Green;
				else type = GameElementType.Blue;
			}
			
			GameElement newElement = _game.GenerateElement(type);
			
			Vector2 dest = new Vector2(newElement.Position.X, newElement.Position.Y);
			newElement.Position = new Vector2(Position.X, Position.Y);
			newElement.SetDestination(dest);
		}
	}
	
	private void SetLabelLevel()
	{
		GetNode<Label>("Level").Text = "" + _level;
	}
	
	public void AddLevel()
	{
		_level++;
		if (_level > 4) _level = 4;
		SetLabelLevel();
	}
	
	public bool Compare(GameElementType type, int level)
	{
		return ((type == _type) && (level == _level) && level < 4);
	}
	
	public bool Compare(GameElement element)
	{
		return element.Compare(_type, _level);
	}
	
	public bool CheckOrder(GameElement element)
	{
		return (element._type == _type) && (element._level == _level);
	}
	
	public int GetWinPoints()
	{
		return _winLevelPoints[_level];
	}
	
	public void SetDestination(Vector2 destination)
	{
		_destination = destination;
		_effect.Show();
		MoveToFront();
		_draggableComponent.Hide();
		_isMoving = true;
	}

	public override void _Process(double delta)
	{
		if (_isMoving)
		{
			float speed = 600;
			float moveAmount = speed * (float)delta;
			float len = (_destination - Position).Length();			
			if (len < 6)
			{
				_isMoving = false;
				_effect.Hide();
				_draggableComponent.Show();
				_SetCellPosition();
			} else
			{
				Vector2 moveDirection = (_destination - Position).Normalized();
				Position += moveDirection * moveAmount;
			}
		}
	}
}
