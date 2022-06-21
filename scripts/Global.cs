using Godot.Collections;
using Godot;

namespace scripts
{
	public class Global : Node
	{
		public static Global Instance { get; private set; }

		private Menu MenuScene { get; set; }
		private Main MainScene { get; set; }
		private Node CurrentScene { get; set; }

		[Export] public Array<GameTheme> GameThemes = new();
		[Export] public int CurrentGameThemeIndex = -1;

		public override void _Ready()
		{
			Viewport root = GetTree().Root;
			this.CurrentScene = root.GetChild(root.GetChildCount() - 1);

			PackedScene menuPreload = GD.Load<PackedScene>("res://scenes/Menu.tscn");
			this.MenuScene = menuPreload.Instance<Menu>();

			PackedScene mainPreload = GD.Load<PackedScene>("res://scenes/Main.tscn");
			this.MainScene = mainPreload.Instance<Main>();

			Global.Instance = this;
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

		public void AddGameTheme(GameTheme gameTheme)
		{
			this.GameThemes.Add(gameTheme);
			this.CurrentGameThemeIndex = this.GameThemes.Count - 1;

			gameTheme.Connect(nameof(GameTheme.ThemeUpdated), this, nameof(UpdateTheme));
			this.UpdateTheme();
		}

		public GameTheme GetCurrentGameTheme()
		{
			return this.GameThemes[this.CurrentGameThemeIndex];
		}

		public void UpdateTheme() => this.MainScene.UpdateTheme();
	}
}
