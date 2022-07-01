using System.Collections.Generic;
using System.Linq;
using Godot;
using scripts;

public class ThemePreview : Control
{
	private const string TileValueMetaLabel = "TileValue";

	private GameTheme LinkedTheme = null;
	private readonly List<ColorPickerButton> ColorPickerButtons = new();

	public override void _Ready()
	{
		int value = 1;
		foreach (Node child in this.GetNode("HBoxContainer").GetChildren())
		{
			if (child is not ColorPickerButton button) continue;

			this.ColorPickerButtons.Add(button);

			button.SetMeta(TileValueMetaLabel, value);
			button.Connect("color_changed", this, nameof(UpdateColor), new(value));
			value++;

			if (this.LinkedTheme is null) continue;

			button.Color = this.LinkedTheme.GetTileColor(value);
		}
	}

	public void SetTheme(GameTheme theme = null)
	{
		Global.GameData.RemoveGameTheme(this.LinkedTheme);
		this.LinkedTheme = theme ?? new();
		Global.GameData.AddGameTheme(this.LinkedTheme);

		this.RefreshUI();
	}

	private void RefreshUI()
	{
		foreach (int value in this.ColorPickerButtons.Select(b => b.GetMeta(TileValueMetaLabel)))
		{
			Color color = this.LinkedTheme.GetTileColor(value);
			this.ColorPickerButtons[value - 1].Color = color;
		}

		this.LinkedTheme.Name = this.LinkedTheme.Name;
		this.GetNameBox().Text = this.LinkedTheme.Name;
	}

	public void UpdateColor(Color color, int value)
	{
		this.LinkedTheme.SetTileColor(value, color);
	}

	public void Deregister()
	{
		Global.GameData.RemoveGameTheme(this.LinkedTheme);
	}

	private LineEdit GetNameBox() => this.GetNode<LineEdit>("./HBoxContainer/VBoxContainer/Label");

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
