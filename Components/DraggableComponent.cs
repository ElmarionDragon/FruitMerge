using Godot;
using System;

public partial class DraggableComponent : TextureButton
{
	public enum StateEnum {
		Idle = 0,
		Dragged = 1
	} 
	
	private Node2D _draggedNode;
	private double _lastClick = 0.0;
	
	private StateEnum _state;
	private Vector2 _previousPos;
	
	public StateEnum State => _state;
	public Vector2 PreviousPos => _previousPos;
	
	[Signal] public delegate void StartDragEventHandler();
	[Signal] public delegate void StopDragEventHandler();
	[Signal] public delegate void DoubleClickEventHandler();
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if (_draggedNode == null)
			_draggedNode = (Node2D)this.GetParent();
		
		this.ButtonDown += _OnButtonDown;	
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (_state == StateEnum.Dragged)
			_draggedNode.GlobalPosition = GetViewport().GetMousePosition();
	}
	
	private void _OnButtonDown()
	{
		double newClick = Time.GetTicksMsec();
		if (newClick - _lastClick < 250)
		{
			_on_button_up();
			EmitSignal("DoubleClick");
		}
		_lastClick = newClick;
	}
	
	private void _on_button_down()
	{
		_state = StateEnum.Dragged;
		_previousPos = _draggedNode.Position;
		_draggedNode.MoveToFront();
		EmitSignal("StartDrag");
	}
	
	private void _on_button_up()
	{
		if (_state == StateEnum.Dragged)
		{
			_state = StateEnum.Idle;
			EmitSignal("StopDrag");
		}
	}
}
