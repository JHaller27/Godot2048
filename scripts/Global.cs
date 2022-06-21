using Godot;

namespace scripts
{
	public class Global : Node
	{
		private Node MenuScene { get; set; }
		private Node MainScene { get; set; }
		private Node CurrentScene { get; set; }

		public static Global Instance(Node node) => node.GetNode<Global>("/root/Global");

		public override void _Ready()
		{
			Viewport root = GetTree().Root;
			this.CurrentScene = root.GetChild(root.GetChildCount() - 1);

			PackedScene menuPreload = GD.Load<PackedScene>("res://scenes/Menu.tscn");
			this.MenuScene = menuPreload.Instance();

			PackedScene mainPreload = GD.Load<PackedScene>("res://scenes/Main.tscn");
			this.MainScene = mainPreload.Instance();
		}

		public void GotoMenu() => this.GotoScene(this.MenuScene);
		public void GotoMain() => this.GotoScene(this.MainScene);

		private void GotoScene(Node scene)
		{
			SceneTree tree = this.GetTree();
			Viewport root = tree.Root;

			root.RemoveChild(this.CurrentScene);
			root.AddChild(scene);
			tree.CurrentScene = scene;
		}
	}
}
