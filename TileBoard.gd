extends Control

export (PackedScene) var tile_scene
export (float) var slide_time

onready var rng = RandomNumberGenerator.new()

var tiles: Array = []
var positions: Array = []
var scale: Vector2 = Vector2.ZERO

var new_values: Array = []
var to_free: Array = []

# Called when the node enters the scene tree for the first time.
func _ready():
	rng.randomize()

	for r in range(4):
		var position_row = []
		var tiles_row = []

		for c in range(4):
			var sprite = get_node("Tile_%d_%d" % [r,c])
			position_row.append(sprite.position)
			tiles_row.append(null)

			if scale == Vector2.ZERO:
				scale = sprite.scale

			sprite.free()

		positions.append(position_row)
		tiles.append(tiles_row)

	add_random_tile()
	$Tween.connect("tween_all_completed", self, "_on_done_tweening")


func _find_empty_spot() -> Array:
	var free_tiles = []
	for r in range(4):
		for c in range(4):
			if tiles[r][c] == null:
				free_tiles.append([r, c])

	if free_tiles.size() == 0:
		return []

	return free_tiles[rng.randi_range(0, free_tiles.size()-1)]


func add_random_tile() -> bool:
	var index = _find_empty_spot()
	if index.size() == 0:
		return false

	var row = index[0]
	var col = index[1]

	var new_tile = tile_scene.instance()
	new_tile.set_position(positions[row][col])
	new_tile.set_scale(scale)

	tiles[row][col] = new_tile
	var value = 1
	if rng.randi_range(0, 9) >= 9:
		value = 2
	new_tile.set_value(value)

	self.add_child(new_tile)

	return true


func _slide_tile(from_obj: Object, from_y: int, from_x: int, to_obj: Object, to_y: int, to_x: int):
	var from_pos = self.positions[from_y][from_x]
	var to_pos = self.positions[to_y][to_x]
	self.tiles[to_y][to_x] = self.tiles[from_y][from_x]
	self.tiles[from_y][from_x] = null
	$Tween.interpolate_property(from_obj, "position", from_pos, to_pos, self.slide_time, Tween.TRANS_SINE, Tween.EASE_IN_OUT)

	if to_obj != null:
		self.to_free.append(to_obj)


func _find_next_left_tile(dst_tile: Object, dst_index: int, row: Array) -> int:
	for src_index in range(dst_index+1, row.size()):
		var src_tile = row[src_index]

		# If dst is empty, look for a non-empty tile
		if dst_tile == null:
			if src_tile != null:
				return src_index

		# If dst is not empty, look for an identical tile
		else:
			if src_tile != null:
				if src_tile.get_value() == dst_tile.get_value():
					return src_index
				return -1

	return -1


func _slide_left():
	for row_index in range(self.tiles.size()):
		var row = self.tiles[row_index]
		var dst_index = 0
		while dst_index < row.size():
			var dst_tile = row[dst_index]
			var src_index = _find_next_left_tile(dst_tile, dst_index, row)
			if src_index != -1:
				var src_tile = row[src_index]
				self._slide_tile(src_tile, row_index, src_index, dst_tile, row_index, dst_index)
				if dst_tile != null:
					self.new_values.append([row_index, dst_index, src_tile.get_value()+1])
			dst_index += 1
	$Tween.start()


func _on_done_tweening():
	for new_val in self.new_values:
		var y = new_val[0]
		var x = new_val[1]
		var val = new_val[2]
		tiles[y][x].set_value(val)
	self.new_values = []

	for obj in self.to_free:
		obj.free()
	self.to_free = []

	self.add_random_tile()


func _input(event):
	if event.is_action_pressed("slide_left"):
		self._slide_left()


# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass
