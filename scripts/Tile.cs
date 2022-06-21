using System;
using Godot;

public class Tile : Sprite
{
	private Label Label { get; set; }

	// ReSharper disable once InconsistentNaming
	private int _value;
	public int Value
	{
		get => this._value;
		set => this.SetValue(value);
	}

	public override void _Ready()
	{
		this.Label = this.GetNode<Label>("Label");
		this.Value = 7;
	}

	private void SetValue(int value)
	{
		this._value = value;
		this.Label.Text = $"{Math.Pow(2, this.Value)}";
	}
}
