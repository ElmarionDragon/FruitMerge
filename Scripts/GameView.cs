using Godot;
using System;

public partial class GameView : Node
{
	[Export] private TextureButton _orderButton1;
	[Export] private TextureButton _orderButton2;
	[Export] private Label _pointsLabel;
	[Export] private Label _ordersLabel;
	[Export] private GameOverPanel _gameOverPanel;
	[Export] private Node2D _cellsContainer;
	[Export] private Node2D _elementsContainer;
	
	private GamePresenter _gamePresenter;
	private GameModel _gameModel;
	private PackedScene _cellPrefab;
	
	public override void _Ready()
	{
		_cellPrefab = (PackedScene)GD.Load("res://Scenes/cell.tscn");
	}
	
	public void Init(GameModel gameModel, GamePresenter gamePresenter)
	{
		_gameModel = gameModel;
		_gamePresenter = gamePresenter;
		_orderButton1.ButtonDown += _OnOrder1;
		_orderButton2.ButtonDown += _OnOrder2;
		_gamePresenter.InitOrders(_orderButton1, _orderButton2);
	}
	
	public void ShowBoard()
	{
		Node2D cell;
		Godot.Collections.Array<Vector2> cells = _gameModel.BoardCells;
		for (int i = 0; i < cells.Count; i++)
		{
			cell = (Node2D)_cellPrefab.Instantiate<Node2D>();
			cell.Position = cells[i];
			_cellsContainer.AddChild(cell);
		}
	}
	
	public void ShowElement(GameElement element)
	{
		_elementsContainer.AddChild(element);
	}
	
	public void ShowGameOver()
	{
		_gameOverPanel.ShowPanel(_gameModel.Points);
	}
	
	public void UpdateOrderPanel()
	{
		_pointsLabel.Text = "" + _gameModel.Points;
		_ordersLabel.Text = "" + _gameModel.OrdersCount + "/10";
	}
	
	private void _OnOrder1()
	{
		_gamePresenter.OnOrder(_orderButton1);
	}
	
	private void _OnOrder2()
	{
		_gamePresenter.OnOrder(_orderButton2);
	}
	
}
