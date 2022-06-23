using System.Collections.Generic;
using Godot;
using scripts;

public class Menu : Control
{
	private PackedScene ThemePreviewPreload = ResourceLoader.Load<PackedScene>("res://scenes/ThemePreview.tscn");
	private Node ThemePreviewContainer => this.GetNode("./Panel/MarginContainer/VBoxContainer/ScrollContainer/ThemeContainer");

	private Global Global { get; set; }

	public override void _Ready()
	{
		this.Global = Global.Instance;
	}

	public void RefreshThemes()
	{
		foreach (Node child in this.ThemePreviewContainer.GetChildren())
		{
			this.ThemePreviewContainer.RemoveChild(child);
			child.Free();
		}

		// foreach (GameTheme gameTheme in Global.GameData.GameThemes)
		// {
		// 	this.AddTheme(gameTheme);
		// }
	}

	private void _on_PlayButton_pressed()
	{
		this.Global.GotoMain();
	}

	private void _on_QuitButton_pressed()
	{
		this.GetTree().Quit();
	}

	private void _on_SaveButton_pressed() => Global.Save();

	private void _on_LoadButton_pressed() => Global.Load();

	private void _on_NewThemeButton_pressed()
	{
		this.AddTheme();
	}

	private void AddTheme(GameTheme theme = null)
	{
		ThemePreview newChild = ThemePreviewPreload.Instance<ThemePreview>();

		Node container = this.GetNode("./Panel/MarginContainer/VBoxContainer/ScrollContainer/ThemeContainer");
		container.AddChild(newChild);

		if (theme == null) return;

		foreach (KeyValuePair<int, Color> themeKvp in theme.TileColors)
		{
			Color color = themeKvp.Value;
			int value = themeKvp.Key;

			newChild.UpdateColor(color, value);
		}
	}
}
