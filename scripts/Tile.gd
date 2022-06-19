extends Sprite

var power: int = 0

func _ready():
	self._update_label()

func _update_label() -> void:
	$Label.text = str(pow(2, self.power))

func update_color() -> void:
	var color = GameTheme.get_value(self.power)
	self.self_modulate = color

func set_value(new_power: int) -> void:
	self.power = new_power
	self._update_label()
	self.update_color()

func get_value() -> int:
	return self.power
