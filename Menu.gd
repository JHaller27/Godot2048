extends Control

const game_theme_preload = preload("res://GameTheme.gd")

onready var color_tiles_container = $Panel/MarginContainer/VBoxContainer/ColorTilesContainer
onready var tile_container = $Panel/MarginContainer/VBoxContainer/ColorTilesContainer/TileContainer
onready var background = $Panel/MarginContainer/VBoxContainer/ColorTilesContainer/Background
onready var game_theme = game_theme_preload.new()
var offset = 0


func _ready():
	reset_color_preview()
	offset = max(11 - tile_container.get_child_count(), 0)
	for index in range(tile_container.get_child_count()):
		var child = tile_container.get_child(index)
		child.connect("color_changed", self, "_on_color_changed", [index])


func reset_color_preview(except: int = -1):
	for index in range(tile_container.get_child_count()):
		var value = index + offset
		if except == index:
			continue

		var color = game_theme.get_value(value)

		var child = tile_container.get_child(index)
		child.color = color


func _on_color_changed(color: Color, index: int) -> void:
	var value = index + offset
	game_theme.set_value(value, color)
	reset_color_preview(index)
