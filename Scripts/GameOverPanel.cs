using Godot;
using System;

public partial class GameOverPanel : Panel
{
	[Export] TextureButton _btnOk;
	[Export] Label _points;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_btnOk.Pressed += _OnButtonOk;
		this.Hide();
	}
	
	public void ShowPanel(int points)
	{
		_points.Text = "" + points;
		this.Show();
	}
	
	private void _OnButtonOk()
	{
		//GD.Print("Ok");
		GetTree().ChangeSceneToFile("start_game.tscn");
	}
}
