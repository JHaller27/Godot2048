using Godot;
using Godot.Collections;

namespace scripts
{
	public class Global : Node
	{
		public static Global Instance { get; private set; }
		private const string SaveGamePath = "user://savegame.json";

		public static GameData GameData
		{
			get => Instance._gameData;
			set => Instance._gameData = value;
		}

		private Menu MenuScene { get; set; }
		private Main MainScene { get; set; }
		private Node CurrentScene { get; set; }

		private GameData _gameData { get; set; }

		public override void _Ready()
		{
			Viewport root = GetTree().Root;
			this.CurrentScene = root.GetChild(root.GetChildCount() - 1);

			PackedScene menuPreload = GD.Load<PackedScene>("res://scenes/Menu.tscn");
			this.MenuScene = this.CurrentScene as Menu ?? menuPreload.Instance<Menu>();

			PackedScene mainPreload = GD.Load<PackedScene>("res://scenes/Main.tscn");
			this.MainScene = this.CurrentScene as Main ?? mainPreload.Instance<Main>();

			this._gameData = new();

			Global.Instance = this;
			this.GotoMenu();
		}

		public void GotoMenu() => this.CallDeferred(nameof(GotoScene), this.MenuScene);
		public void GotoMain() => this.CallDeferred(nameof(GotoScene), this.MainScene);

		private void GotoScene(Node scene)
		{
			SceneTree tree = this.GetTree();
			Viewport root = tree.Root;

			root.RemoveChild(this.CurrentScene);
			root.AddChild(scene);
			tree.CurrentScene = scene;
			this.CurrentScene = scene;
		}

		public void UpdateTheme() => this.MainScene.UpdateTheme();

		public static void Save()
		{
			object export = Instance._gameData.Export();
			string serialized = JSON.Print(export, "  ");

			File saveGame = new();
			saveGame.Open(SaveGamePath, File.ModeFlags.Write);
			try
			{
				saveGame.StoreString(serialized);
			}
			finally
			{
				saveGame.Close();
			}

			GD.Print("Saved game to ", SaveGamePath);
		}

		public static void Load()
		{
			File saveGame = new();
			if (!saveGame.FileExists(SaveGamePath))
			{
				return;
			}

			string data;
			saveGame.Open(SaveGamePath, File.ModeFlags.Read);
			try
			{
				data = saveGame.GetAsText();
			}
			catch
			{
				return;
			}
			finally
			{
				saveGame.Close();
			}

			object obj = JSON.Parse(data).Result;

			// Godot type shenanigans
			if (obj is not Dictionary dictionary)
			{
				return;
			}

			Godot.Collections.Dictionary<string, object> deserialized = new(dictionary);

			Instance._gameData = GameData.Import(deserialized);
			GD.Print("Loaded game data from ", SaveGamePath);

			Instance.MenuScene.RefreshThemes();
		}
	}
}
