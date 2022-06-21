using Godot;
using scripts;

public class Main : Control
{
	private Global Global { get; set; }

	public override void _Ready()
	{
		this.Global = Global.Instance(this);
	}

	public override void _Input(InputEvent inputEvent)
	{
		if (inputEvent.IsAction("ui_cancel"))
		{
			this.Global.GotoMenu();
		}
	}
}
