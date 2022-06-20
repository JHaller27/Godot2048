class_name SaveGame
extends Resource


const SAVE_GAME_PATH = "user://savegame.tres"

export var board: Resource


func write_save() -> void:
	board = Main.get_board()
	ResourceSaver.save(SAVE_GAME_PATH, self)


static func load_save() -> SaveGame:
	if ResourceLoader.exists(SAVE_GAME_PATH):
		var save = load(SAVE_GAME_PATH) as SaveGame
		return save
	return null
