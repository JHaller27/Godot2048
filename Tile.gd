extends Sprite

onready var label: Label = $Label

func _ready():
	label = $Label

func set_value(power: int):
	label.text = str(2^power)
