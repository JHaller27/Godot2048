using System;
using Godot;
using scripts;
using scripts.Utils;

public class Menu : Control
{
	private PackedScene ThemePreviewPreload = ResourceLoader.Load<PackedScene>("res://scenes/ThemePreview.tscn");
	private Node ThemePreviewContainer => this.GetNode("./Panel/MarginContainer/VBoxContainer/ScrollContainer/ThemeContainer");

	private Global Global { get; set; }

	public override void _Ready()
	{
		this.Global = Global.Instance;
		this.AddTheme();
	}

	public void RefreshThemes()
	{
		foreach (ThemePreview child in this.ThemePreviewContainer.GetChildren<ThemePreview>())
		{
			this.ThemePreviewContainer.RemoveChild(child);
			child.QueueFree();
			child.Deregister();
		}

		foreach (GameTheme gameTheme in Global.GameData.GameThemes)
		{
			this.AddTheme(gameTheme);
		}
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
		Console.WriteLine("Adding new theme: " + theme?.Name);

		ThemePreview newChild = ThemePreviewPreload.Instance<ThemePreview>();

		this.ThemePreviewContainer.AddChild(newChild);

		newChild.SetTheme(theme);
	}
}
