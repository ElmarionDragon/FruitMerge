using Godot;
using System;
using Godot.Collections;

public partial class Game : Node2D
{
	[Export] private GameView _gameView;
	[Export] private GamePresenter _gamePresenter;
	[Export] private GameModel _gameModel;
	
	public override void _Ready()
	{
		_gameModel.Init(_gameView);
		_gamePresenter.Init(_gameModel, _gameView);
		_gameView.Init(_gameModel, _gamePresenter);
		
		_gamePresenter.InitGame();
	}
	
}

