extends Control

export (PackedScene) var tile_scene
onready var rng = RandomNumberGenerator.new()

onready var tween: Tween = $Tween

var tiles: Array = []
var positions: Array = []
var scale: Vector2 = Vector2.ZERO

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


func _input(event):
	if event.is_action_pressed("ui_accept"):
		add_random_tile()


# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass
