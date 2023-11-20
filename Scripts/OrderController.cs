using Godot;
using System;

public partial class OrderController : Node
{
	[Export] private FruitFabric _fabric;
	[Export] private int _MINLEVEL = 1;
	[Export] private int _MAXLEVEL = 4;
	
	private GameModel _gameModel;
	private GameView _gameView;
	
	
	public void Init(GameModel gameModel, GameView gameView)
	{
		_gameModel = gameModel;
		_gameView = gameView;
	}
	
	public void DoOrder(TextureButton orderButton)
	{
		Sprite2D spr = orderButton.GetNode<Sprite2D>("Sprite2D");
		GameElement orderElement = spr.GetChild<GameElement>(0);
		
		// Надо узнать, есть ли на доске такой элемент?
		GameElement element;
		for (int i = 0; i < _gameModel.GameElements.Count; i++)
		{
			element = _gameModel.GameElements[i];
			if (element.CheckOrder(orderElement))
			{
				int points = orderElement.GetWinPoints();
				_gameModel.AddPoints(points);
				
				_gameModel.RemoveElement(element);
				GenerateNewOrder(orderButton);
				_gameModel.AddOrder();
				break;
			}
		}
	}
	
	public void GenerateNewOrder(TextureButton orderButton)
	{
		// Удалим вначале предыдущий заказ, если такой есть!
		Sprite2D spr = orderButton.GetNode<Sprite2D>("Sprite2D");
		Node2D orderElement = spr.GetChild<Node2D>(0);
		if (orderElement != null) orderElement.QueueFree();
		
		// Сгенеририуем случайный тип и уровень элемента
		int typesCount = GameElement.GameElementType.GetNames(typeof(GameElement.GameElementType)).Length;
		int rType = GD.RandRange(0,typesCount-1);
		int rLevel = GD.RandRange(_MINLEVEL,_MAXLEVEL);
		
		Node2D element = (Node2D)_fabric.CreateGameElement(-1, (GameElement.GameElementType)rType, rLevel, false);
		orderButton.GetNode("Sprite2D").AddChild(element);
	}
}
