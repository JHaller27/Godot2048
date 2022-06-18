extends Control

export (PackedScene) var tile_scene
export (float) var slide_time

onready var rng = RandomNumberGenerator.new()

var tiles: Array = []
var positions: Array = []
var scale: Vector2 = Vector2.ZERO

var new_values: Array = []
var to_free: Array = []
var just_moved: Array = []


func get_column(grid: Array, idx: int) -> Array:
	var col = []
	for row in grid:
		col.append(row[idx])
	return col


func reset_just_moved():
	just_moved = []
	for y in range(4):
		just_moved.append([])
		for x in range(4):
			just_moved[y].append(false)


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

	reset_just_moved()


func _find_empty_spot() -> Array:
	var free_tiles = []
	for r in range(4):
		for c in range(4):
			if tiles[r][c] == null:
				free_tiles.append([r, c])

	if free_tiles.size() == 0:
		return []

	return free_tiles[rng.randi_range(0, free_tiles.size()-1)]


func add_tile(row: int, col: int, val: int) -> void:
	var new_tile = tile_scene.instance()
	new_tile.set_position(positions[row][col])
	new_tile.set_scale(scale)

	tiles[row][col] = new_tile
	new_tile.set_value(val)

	self.add_child(new_tile)


func add_random_tile() -> bool:
	var index = _find_empty_spot()
	if index.size() == 0:
		return false

	var row = index[0]
	var col = index[1]

	var value = 1
	if rng.randi_range(0, 9) >= 9:
		value = 2
	self.add_tile(row, col, value)

	return true


func _slide_tile(from_obj: Object, from_y: int, from_x: int, to_obj: Object, to_y: int, to_x: int):
	if to_obj != null && from_obj.get_value() == to_obj.get_value():
		self.new_values.append([to_y, to_x, to_obj.get_value()+1])

	just_moved[to_y][to_x] = to_obj != null

	var from_pos = self.positions[from_y][from_x]
	var to_pos = self.positions[to_y][to_x]
	self.tiles[to_y][to_x] = self.tiles[from_y][from_x]
	self.tiles[from_y][from_x] = null
	$Tween.interpolate_property(from_obj, "position", from_pos, to_pos, self.slide_time, Tween.TRANS_SINE, Tween.EASE_IN_OUT)

	if to_obj != null:
		self.to_free.append(to_obj)


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
	self.reset_just_moved()


func _find_next_spot_desc(src_tile: Object, src_index: int, row: Array, just_moved_row: Array) -> int:
	var dst_index = src_index
	var scout_index = dst_index-1
	while scout_index >= 0:
		var scout_tile = row[scout_index]
		if scout_tile != null:
			var dst_tile = row[dst_index]
			if !just_moved_row[scout_index] && scout_tile.get_value() == src_tile.get_value():
				return scout_index
			return dst_index

		dst_index = scout_index
		scout_index -= 1

	return 0


func _find_next_spot_asc(src_tile: Object, src_index: int, row: Array, just_moved_row: Array) -> int:
	var dst_index = src_index
	var scout_index = dst_index+1
	while scout_index < row.size():
		var scout_tile = row[scout_index]
		if scout_tile != null:
			var dst_tile = row[dst_index]
			if !just_moved_row[scout_index] && scout_tile.get_value() == src_tile.get_value():
				return scout_index
			return dst_index

		dst_index = scout_index
		scout_index += 1

	return row.size()-1


func _slide_left():
	for row_index in range(self.tiles.size()):
		var row = self.tiles[row_index]
		for src_index in range(1, row.size()):
			var src_tile = row[src_index]
			if src_tile == null:
				continue
			var dst_index = _find_next_spot_desc(src_tile, src_index, row, just_moved[row_index])
			if dst_index != src_index:
				self._slide_tile(src_tile, row_index, src_index, row[dst_index], row_index, dst_index)
	$Tween.start()


func _slide_right():
	for row_index in range(self.tiles.size()):
		var row = self.tiles[row_index]
		for src_index in range(row.size()-1, -1, -1):
			var src_tile = row[src_index]
			if src_tile == null:
				continue
			var dst_index = _find_next_spot_asc(src_tile, src_index, row, just_moved[row_index])
			if dst_index != src_index:
				self._slide_tile(src_tile, row_index, src_index, row[dst_index], row_index, dst_index)
	$Tween.start()


func _slide_up():
	for col_index in range(self.tiles.size()):
		var col = get_column(self.tiles, col_index)
		for src_index in range(1, col.size()):
			var src_tile = col[src_index]
			if src_tile == null:
				continue
			var dst_index = _find_next_spot_desc(src_tile, src_index, col, get_column(just_moved, col_index))
			if dst_index != src_index:
				self._slide_tile(src_tile, src_index, col_index, col[dst_index], dst_index, col_index)
	$Tween.start()


func _slide_down():
	for col_index in range(self.tiles.size()):
		var col = get_column(self.tiles, col_index)
		for src_index in range(col.size()-1, -1, -1):
			var src_tile = col[src_index]
			if src_tile == null:
				continue
			var dst_index = _find_next_spot_asc(src_tile, src_index, col, get_column(just_moved, col_index))
			if dst_index != src_index:
				self._slide_tile(src_tile, src_index, col_index, col[dst_index], dst_index, col_index)
	$Tween.start()


func _input(event):
	if event.is_action_pressed("slide_left"):
		self._slide_left()
	elif event.is_action_pressed("slide_right"):
		self._slide_right()
	elif event.is_action_pressed("slide_up"):
		self._slide_up()
	elif event.is_action_pressed("slide_down"):
		self._slide_down()


# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass
