extends Node


export (int) var offset
var leading_children = 0
var editing = false


func _ready():
	for child in self.get_children():
		if child is ColorPickerButton:
			break
		else:
			leading_children += 1

	for index in range(leading_children, self.get_child_count()):
		var child = self.get_child(index)
		child.connect("color_changed", self, "_on_color_changed", [index])


func reset_color_preview(except: int = -1):
	for index in range(leading_children, self.get_child_count()):
		var value = index + offset - leading_children
		if except == index:
			continue

		var color = GameTheme.get_value(value)

		var child = self.get_child(index)
		child.color = color


func _on_color_changed(color: Color, index: int) -> void:
	var value = index + offset - leading_children
	GameTheme.set_value(value, color)
	self.reset_color_preview(index)
	Main.try_update_colors()
	Global.save_game_theme(self.get_theme_name())


func get_theme_name() -> String:
	return $VBoxContainer/Label.text

func set_theme_name(name: String):
	$VBoxContainer/Label.text = name


func _on_RenameButton_pressed():
	if editing:
		editing = false
		done_editing()
	else:
		editing = true
		edit_label()


func edit_label():
	$VBoxContainer/Label.editable = true
	$VBoxContainer/RenameButton.text = "Done"

func done_editing():
	$VBoxContainer/Label.editable = false
	$VBoxContainer/RenameButton.text = "Rename"
	Global.save_game_theme(self.get_theme_name())
