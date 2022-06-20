class_name Board
extends Resource


const _size = 4
export var _values: Array = []


func init():
	for _r in range(_size):
		var row = []
		for _c in range(_size):
			row.append(null)
		_values.append(row)


func size() -> int:
	return _size


func set_at(y: int, x: int, val: int) -> void:
	_values[y][x] = val


func get_at(y: int, x: int) -> int:
	return _values[y][x]


func itr_coords() -> Array:
	var coords = []
	for y in range(_size):
		for x in range(_size):
			coords.append([y, x, get_at(y, x)])
	return coords
