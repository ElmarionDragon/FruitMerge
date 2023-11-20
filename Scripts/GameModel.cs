using Godot;
using System;

public partial class GameModel : Node
{
	public static int BOARDWIDTH = 8;
	public static int ELEMENTWIDTH = 75;
	public static int STARTCOUNT = 8;
	public static GameElement.GameElementType STARTTYPE = GameElement.GameElementType.Blue;
	public static int MAXORDERS = 10;
	
	private GameView _gameView;
	private OrderController _orderController;
	
	private int _points = 0;
	public int Points => _points;
	
	private int _ordersCount;
	public int OrdersCount => _ordersCount;
	
	private Godot.Collections.Array<int> _board = new Godot.Collections.Array<int>();
	private Godot.Collections.Array<int> Board => _board;
	
	private Godot.Collections.Array<Vector2> _boardCells = new Godot.Collections.Array<Vector2>();
	public Godot.Collections.Array<Vector2> BoardCells => _boardCells;
	
	private Godot.Collections.Array<GameElement> _gameElements = new Godot.Collections.Array<GameElement>();
	public Godot.Collections.Array<GameElement> GameElements => _gameElements;
	
	public void Init(GameView gameView)
	{
		_gameView = gameView;
		InitBoard();
	}
	
	public void InitBoard()
	{
		for (int i = 0; i < BOARDWIDTH * BOARDWIDTH; i++)
		{
			_board.Add(i);
		}
	}
	
	public void AddPoints(int points)
	{
		_points += points;
		_gameView.UpdateOrderPanel();
	}
	
	public void AddOrder()
	{
		_ordersCount++;
		_gameView.UpdateOrderPanel();
		if (_ordersCount == MAXORDERS)
		{
			_gameView.ShowGameOver();
		}
	}
	
	public void AddGameElement(GameElement element)
	{
		_gameElements.Add(element);
	}
	
	
	public void SetBoard(Godot.Collections.Array<int> board)
	{
		_board = board;
	}
	
	public void SetBoardCells(Godot.Collections.Array<Vector2> cells)
	{
		_boardCells = cells;
	}
	
	public void RemoveElement(GameElement element)
	{
		_gameElements.Remove(element);
		_board.Add(element.Num);
		element.QueueFree();
	}
	
	public void ChangeElementCell(int oldNum, int newNum)
	{
		_board.Add(oldNum);
		_board.Remove(newNum); 
	}
	
	public int GetRandomFreePos()
	{
		int r = GD.RandRange(0, _board.Count-1);
		int b = _board[r];
		_board.RemoveAt(r);
		return b;
	}
	
	public Vector2 GetPosByNum(int n)
	{
		return new Vector2(n % BOARDWIDTH * ELEMENTWIDTH, n /BOARDWIDTH * ELEMENTWIDTH);
	}
	
	public int GetNumByPos(Vector2 pos)
	{
		int x = (int)pos.X / ELEMENTWIDTH;
		int y = (int)pos.Y / ELEMENTWIDTH;
		int num = y * BOARDWIDTH + x;
		return num;
	}
}
