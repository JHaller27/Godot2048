using System.Collections.Generic;
using Godot;
using scripts;

public class ThemePreview : HBoxContainer
{
	private const string TileValueMetaLabel = "TileValue";

	private readonly GameTheme LinkedTheme = new();
	private readonly List<ColorPickerButton> ColorPickerButtons = new();

	public override void _Ready()
	{
		Global.GameData.AddGameTheme(this.LinkedTheme);

		int value = 1;
		foreach (Node child in this.GetChildren())
		{
			if (child is not ColorPickerButton button)
			{
				continue;
			}

			button.SetMeta(TileValueMetaLabel, value);
			button.Connect("color_changed", this, nameof(UpdateColor), new(value));
			value++;

			button.Color = Global.GameData.GetCurrentGameTheme().GetTileColor(value);
		}
	}

	public void UpdateColor(Color color, int value)
	{
		this.LinkedTheme.SetTileColor(value, color);
	}
}
