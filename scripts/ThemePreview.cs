using System.Collections.Generic;
using Godot;
using scripts;

public class ThemePreview : HBoxContainer
{
	private const string TileValueMetaLabel = "TileValue";

	private GameTheme LinkedTheme = null;
	private readonly List<ColorPickerButton> ColorPickerButtons = new();

	public override void _Ready()
	{
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
			this.ColorPickerButtons.Add(button);
		}
	}

	public void SetTheme(GameTheme theme = null)
	{
		Global.GameData.RemoveGameTheme(this.LinkedTheme);
		this.LinkedTheme = theme ?? new();
		Global.GameData.AddGameTheme(this.LinkedTheme);

		this.Refresh();
	}

	private void Refresh()
	{
		foreach (KeyValuePair<int, Color> themeKvp in this.LinkedTheme.TileColors)
		{
			Color color = themeKvp.Value;
			int value = themeKvp.Key;

			this.UpdateColor(color, value);
		}

		this.SetName(this.LinkedTheme.Name);
	}

	public void UpdateColor(Color color, int value)
	{
		this.LinkedTheme.SetTileColor(value, color);
		this.ColorPickerButtons[value - 1].Color = color;
	}

	public void SetName(string name)
	{
		this.LinkedTheme.Name = name;
		this.GetNameBox().Text = this.LinkedTheme.Name;
	}

	public void Deregister()
	{
		Global.GameData.RemoveGameTheme(this.LinkedTheme);
	}

	private LineEdit GetNameBox() => this.GetNode<LineEdit>("./VBoxContainer/Label");

	private void _on_RenameButton_pressed()
	{
		LineEdit nameBox = this.GetNameBox();

		nameBox.Editable = !nameBox.Editable;

		if (!nameBox.Editable)
		{
			this.LinkedTheme.Name = nameBox.Text;
		}
	}
}
