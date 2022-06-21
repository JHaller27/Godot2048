using System;
using Godot;
using Godot.Collections;
using scripts;

public class GameData : Resource
{
	private const string SaveGamePath = "user://savegame.tres";

	[Export] public Array<GameTheme> GameThemes = new();
	[Export] public int CurrentGameThemeIndex = -1;

	public void AddGameTheme(GameTheme gameTheme)
	{
		this.GameThemes.Add(gameTheme);
		this.CurrentGameThemeIndex = this.GameThemes.Count - 1;

		gameTheme.Connect(nameof(GameTheme.ThemeUpdated), Global.Instance, nameof(Global.UpdateTheme));
		Global.Instance.UpdateTheme();
	}

	public GameTheme GetCurrentGameTheme()
	{
		return this.GameThemes[this.CurrentGameThemeIndex];
	}

	public void Save()
	{
		Error err = ResourceSaver.Save(SaveGamePath, this);
		Console.WriteLine(err);
	}

	public static void Load()
	{
		Resource gameData = ResourceLoader.Load(SaveGamePath);
		Global.GameData = (GameData)gameData;
	}
}
