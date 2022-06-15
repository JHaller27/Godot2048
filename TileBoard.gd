extends Control


# Declare member variables here. Examples:
var _tiles = []


# Called when the node enters the scene tree for the first time.
func _ready():
	for r in range(4):
		var row = []
		for c in range(4):
			var tile = get_node("Tile_{%d}_{%d}" % [r,c])
			row.append(tile)
		_tiles.append(row)

	for row in _tiles:
		for tile in row:
			tile.visible = false


# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass
