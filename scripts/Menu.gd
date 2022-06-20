extends Control


onready var tile_container = $Panel/MarginContainer/VBoxContainer/ThemePreview


func _ready():
	tile_container.reset_color_preview()
	Main.visible = false


func _on_PlayButton_pressed():
	Global.goto_scene(Main)


func _on_QuitButton_pressed():
	get_tree().quit()


func _on_SaveButton_pressed():
	Global.save_game()


func _on_LoadButton_pressed():
	Global.load_game()
	Global.goto_scene(Main)
