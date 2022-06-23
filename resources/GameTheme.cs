using System.Collections;
using Godot;
using GDC = Godot.Collections;
using System.Collections.Generic;
using System.Linq;
using scripts.Utils;

namespace scripts
{
	public class GameTheme : Resource
	{
		private static readonly Color UnknownColor = Colors.White;

		public string Name { get; set; }
		private Dictionary<int, Color> _tileColors = new();
		public IReadOnlyDictionary<int, Color> TileColors => this._tileColors;

		[Signal] public delegate void ThemeUpdated();

		public void SetTileColor(int value, Color color)
		{
			this._tileColors[value] = color;
			this.EmitSignal(nameof(ThemeUpdated));
		}

		public Color GetTileColor(int value)
		{
			return this._tileColors.ContainsKey(value) ? this._tileColors[value] : GameTheme.UnknownColor;
		}

		public Color GetTileColor(Tile tile) => GetTileColor(tile.Value);

		public Dictionary<string, object> Export()
		{
			return new()
			{
				{ "Name", this.Name },
				{ "TileColors", new GDC.Dictionary<int, string>(this._tileColors.ToDictionary(
					kvp => kvp.Key,
					kvp => kvp.Value.ToHtml()
				)) },
			};
		}

		public static GameTheme Import(GDC.Dictionary source)
		{
			GameTheme dest = new();
			dest.Name = source["Name"] as string;
			dest._tileColors = ((GDC.Dictionary)source["TileColors"])
				.CastDict<string, string>()
				.ToDictionary(kvp => int.Parse(kvp.Key), kvp => new Color(kvp.Value));

			return dest;
		}
	}
}
