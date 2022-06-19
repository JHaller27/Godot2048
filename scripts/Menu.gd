extends Control

onready var color_tiles_container = $Panel/MarginContainer/VBoxContainer/ColorTilesContainer
onready var tile_container = $Panel/MarginContainer/VBoxContainer/TileContainer
onready var background = $Panel/MarginContainer/VBoxContainer/ColorTilesContainer/Background
var offset = -1


func _ready():
	reset_color_preview()
	offset = max(11 - tile_container.get_child_count(), 0)
	for index in range(tile_container.get_child_count()):
		var child = tile_container.get_child(index)
		child.connect("color_changed", self, "_on_color_changed", [index])

	Main.visible = false


func reset_color_preview(except: int = -1):
	for index in range(1, tile_container.get_child_count()):
		var value = index + offset
		if except == index:
			continue

		var color = GameTheme.get_value(value)

		var child = tile_container.get_child(index)
		child.color = color


func _on_color_changed(color: Color, index: int) -> void:
	var value = index + offset
	GameTheme.set_value(value, color)
	reset_color_preview(index)
	Main.try_update_colors()


func _on_PlayButton_pressed():
	Global.goto_scene(Main)


func _on_QuitButton_pressed():
	get_tree().quit()
