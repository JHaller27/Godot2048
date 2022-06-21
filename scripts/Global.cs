using Godot;

namespace scripts
{
	public class Global : Node
	{
		public static Global Instance { get; private set; }

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
			this.MenuScene = menuPreload.Instance<Menu>();

			PackedScene mainPreload = GD.Load<PackedScene>("res://scenes/Main.tscn");
			this.MainScene = mainPreload.Instance<Main>();

			this._gameData = new();

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

		public void UpdateTheme() => this.MainScene.UpdateTheme();
	}
}
