extends Sprite

var power: int = 0
var theme = null

func _ready():
	self._update_label()

func _update_label() -> void:
	$Label.text = str(pow(2, self.power))

func _update_color() -> void:
	if self.theme == null:
		return
	var color = self.theme.get_value(self.power)
	$CanvasModulate.set_color(color)

func set_value(new_power: int) -> void:
	self.power = new_power
	self._update_label()
	self._update_color()

func get_value() -> int:
	return self.power

func set_theme(new_theme) -> void:
	self.theme = new_theme
