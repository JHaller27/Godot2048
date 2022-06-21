using Godot;
using Godot.Collections;

namespace scripts
{
	public class GameTheme : Resource
	{
		private static readonly Color UnknownColor = Colors.White;

		[Export] public string Name { get; set; }
		[Export] public readonly Dictionary<int, Color> TileColors = new();

		[Signal] public delegate void ThemeUpdated();

		public void SetTileColor(int value, Color color)
		{
			this.TileColors[value] = color;
			this.EmitSignal(nameof(ThemeUpdated));
		}

		public Color GetTileColor(int value)
		{
			return this.TileColors.ContainsKey(value) ? this.TileColors[value] : GameTheme.UnknownColor;
		}

		public Color GetTileColor(Tile tile) => GetTileColor(tile.Value);
	}
}
