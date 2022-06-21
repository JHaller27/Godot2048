using Godot;
using scripts;

public class Menu : Control
{
	private PackedScene ThemePreviewPreload = ResourceLoader.Load<PackedScene>("res://scenes/ThemePreview.tscn");

	private Global Global { get; set; }

	public override void _Ready()
	{
		this.Global = Global.Instance;
	}

	private void _on_PlayButton_pressed()
	{
		this.Global.GotoMain();
	}

	private void _on_QuitButton_pressed()
	{
		this.GetTree().Quit();
	}

	private void _on_SaveButton_pressed() => Global.GameData.Save();

	private void _on_LoadButton_pressed() => GameData.Load();

	private void _on_NewThemeButton_pressed()
	{
		ThemePreview newChild = ThemePreviewPreload.Instance<ThemePreview>();

		Node container = this.GetNode("./Panel/MarginContainer/VBoxContainer/ScrollContainer/ThemeContainer");
		container.AddChild(newChild);
	}
}
