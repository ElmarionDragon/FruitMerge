using Godot;
using System;

public partial class StartGame : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GetNode<TextureButton>("StartButton").Pressed +=
			() => GetTree().ChangeSceneToFile("Scenes/game.tscn");
	}
}
