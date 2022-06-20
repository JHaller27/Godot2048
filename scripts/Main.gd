extends Control


func try_update_colors() -> void:
	$ColorRect/TileBoard.try_update_colors()


func get_board() -> Board:
	return $ColorRect/TileBoard.to_board()


func arrange_from_save(save: SaveGame) -> void:
	$ColorRect/TileBoard.from_board(save.board)
