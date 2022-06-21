using Godot;
using scripts;

public class Menu : Control
{
	private Global Global { get; set; }

	public override void _Ready()
	{
		this.Global = Global.Instance(this);
	}

	private void _on_PlayButton_pressed()
	{
		this.Global.GotoMain();
	}


	private void _on_QuitButton_pressed()
	{
		this.GetTree().Quit();
	}


	private void _on_SaveButton_pressed()
	{
		// Replace with function body.
	}


	private void _on_LoadButton_pressed()
	{
		// Replace with function body.
	}
}
