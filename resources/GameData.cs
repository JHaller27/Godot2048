using System.Collections.Generic;
using System.Linq;
using Godot;
using GDC = Godot.Collections;
using scripts;
using scripts.Utils;

public class GameData : Resource
{
	private List<GameTheme> _gameThemes = new();
	private int CurrentGameThemeIndex = -1;

	public IEnumerable<GameTheme> GameThemes => this._gameThemes;

	public void AddGameTheme(GameTheme gameTheme)
	{
		if (this._gameThemes.Contains(gameTheme)) return;

		this._gameThemes.Add(gameTheme);
		this.CurrentGameThemeIndex = this._gameThemes.Count - 1;

		gameTheme.Connect(nameof(GameTheme.ThemeUpdated), Global.Instance, nameof(Global.UpdateTheme));
		Global.Instance.UpdateTheme();
	}

	public void RemoveGameTheme(GameTheme gameTheme)
	{
		if (!this._gameThemes.Contains(gameTheme)) return;

		this._gameThemes.Remove(gameTheme);
		this.CurrentGameThemeIndex = 0;
		Global.Instance.UpdateTheme();
	}

	public GameTheme GetCurrentGameTheme()
	{
		return this._gameThemes[this.CurrentGameThemeIndex];
	}

	public Dictionary<string, object> Export()
	{
		return new()
		{
			{ "CurrentIndex", this.CurrentGameThemeIndex },
			{ "GameThemes", new GDC.Array(this._gameThemes.Select(gt => gt.Export()).ToList()) },
		};
	}

	public static GameData Import(GDC.Dictionary<string, object> source)
	{
		GameData dest = new();
		dest.CurrentGameThemeIndex = source["CurrentIndex"] as int? ?? 0;
		dest._gameThemes = ((GDC.Array)source["GameThemes"])
			.ToSys<GDC.Dictionary>()
			.Select(GameTheme.Import)
			.ToList();

		return dest;
	}
}
