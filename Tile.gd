extends Sprite

var power: int = 0

func _ready():
	self._update_label()

func _update_label():
	$Label.text = str(pow(2, self.power))

func set_value(new_power: int):
	self.power = new_power
	self._update_label()
