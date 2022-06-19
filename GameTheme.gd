extends Resource

var UNKNOWN_COLOR = Color("#ffffff")
var colors = [UNKNOWN_COLOR, UNKNOWN_COLOR]

var bg_color = Color("#baa56c")


func get_value(val: int) -> Color:
	if val < 0 || val >= colors.size():
		return UNKNOWN_COLOR
	return colors[val]


func list_values() -> Array:
	return colors


func set_value(val: int, color: Color) -> void:
	while colors.size()-1 < val:
		colors.append(UNKNOWN_COLOR)
	colors[val] = color


func get_bg() -> Color:
	return bg_color


func set_bg(color: Color) -> void:
	bg_color = color