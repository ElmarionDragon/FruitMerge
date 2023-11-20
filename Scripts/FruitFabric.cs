using Godot;
using System;

public partial class FruitFabric : Node
{
	PackedScene _gameElementPrefab;
	[Export] private PackedScene[] _fruits;
	[Export] private GamePresenter _game;
	
	public FruitFabric()
	{
	}
	
	public override void _Ready()
	{
		_gameElementPrefab = (PackedScene)GD.Load("res://Scenes/game_element.tscn");
	}
	
	public GameElement CreateGameElement(int num, GameElement.GameElementType type, int level, bool isDnd)
	{
		GameElement element = _gameElementPrefab.Instantiate<GameElement>();
		element.Init(num, type, level, _game);
		element.GetNode("Image").AddChild(_fruits[(int)type].Instantiate());		
		if (!isDnd) element.GetNode<TextureButton>("DraggableComponent").QueueFree(); //Disabled = false;
		return element;
	}
}
