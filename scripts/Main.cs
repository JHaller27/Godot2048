using System.Collections.Generic;
using Godot;
using scripts;

public class Main : Control
{
	private Global Global { get; set; }

	private readonly List<Tile> Tiles = new();

	private ColorRect GetBackground() => this.GetNode<ColorRect>("ColorRect");

	public override void _Ready()
	{
		this.Global = Global.Instance;

		foreach (Tile tile in this.GetNode("ColorRect/TileBoard").GetChildren())
		{
			this.Tiles.Add(tile);
		}
	}

	public override void _Input(InputEvent inputEvent)
	{
		if (inputEvent.IsAction("ui_cancel"))
		{
			this.Global.GotoMenu();
		}
	}

	public void UpdateTheme()
	{
		this.Tiles.ForEach(t => t.RefreshColor());
		this.GetBackground().Color = Global.GameData.GetCurrentGameTheme().BackgroundColor;
	}
}
