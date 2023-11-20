using Godot;
using System;

public partial class GamePresenter : Node
{
	[Export] private FruitFabric _fabric;
	[Export] private OrderController _orderController;
	
	private GameModel _gameModel;
	private GameView _gameView;
	
	public void Init(GameModel gameModel, GameView gameView)
	{
		_gameModel = gameModel;
		_gameView = gameView;
		_orderController.Init(gameModel, gameView);
	}
	
	public void InitGame()
	{
		// Generate all cells
		Godot.Collections.Array<Vector2> boardCells = new Godot.Collections.Array<Vector2>();
		Vector2 cell;
		for (int i = 0; i < GameModel.BOARDWIDTH * GameModel.BOARDWIDTH; i++)
		{
			cell = _gameModel.GetPosByNum(i);
			boardCells.Add(cell);
		}
		// Generate start elements
		for (int i = 0; i < GameModel.STARTCOUNT; i++)
		{
			GenerateElement(GameModel.STARTTYPE);
		}
		// Set gameModel states
		_gameModel.SetBoardCells(boardCells);
		// Show start board
		_gameView.ShowBoard();
	}
	
	public void InitOrders(TextureButton orderButton1, TextureButton orderButton2)
	{
		// Generate start orders
		_orderController.GenerateNewOrder(orderButton1);
		_orderController.GenerateNewOrder(orderButton2);
	}
	
	public GameElement GenerateElement(GameElement.GameElementType rt)
	{
		int r = _gameModel.GetRandomFreePos();
		GameElement element = _fabric.CreateGameElement(r, rt, 1, true);
		element.Position = _gameModel.GetPosByNum(r);
		_gameView.ShowElement(element);
		_gameModel.AddGameElement(element);
		return element;
	}
	
	public void ChangeElementCell(GameElement element)
	{
		int oldNum = element.Num;
		int newNum = _gameModel.GetNumByPos(element.Position);
		element.SetNum(newNum);
		_gameModel.ChangeElementCell(oldNum, newNum);
	}
	
	public void RemoveElement(GameElement element)
	{
		_gameModel.RemoveElement(element);
	}
	
	public void OnOrder(TextureButton orderButton)
	{
		_orderController.DoOrder(orderButton);
	}
}
