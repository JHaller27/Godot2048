using System;
using Godot;
using scripts;

public class Tile : Sprite
{
	private Label Label { get; set; }

	// ReSharper disable once InconsistentNaming
	[Export] public int _value;
	public int Value
	{
		get => this._value;
		set => this.SetValue(value);
	}

	public override void _Ready()
	{
		this.Label = this.GetNode<Label>("Label");
		this.Value = 1;
	}

	private void SetValue(int value)
	{
		this._value = value;
		this.Label.Text = $"{Math.Pow(2, this.Value)}";

		this.RefreshColor();
	}

	public void RefreshColor()
	{
		GameTheme gameTheme = Global.GameData.GetCurrentGameTheme();
		Color color = gameTheme.GetTileColor(this);
		this.SelfModulate = color;
	}
}
