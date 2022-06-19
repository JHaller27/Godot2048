extends Node


var current_scene: Node = null
onready var ROOT = get_tree().get_root()


func _ready():
	ROOT.connect("ready", self, "_all_ready")


func _all_ready() -> void:
	_remove_all_children()
	goto_scene(Menu)


func _remove_all_children() -> void:
	for child in ROOT.get_children():
		ROOT.remove_child(child)


func goto_scene(new_scene: Node) -> void:
	call_deferred("_deferred_goto_scene", new_scene)

func _deferred_goto_scene(new_scene: Node) -> void:
	if current_scene != null:
		ROOT.remove_child(current_scene)
	ROOT.add_child(new_scene)
	current_scene = new_scene
	current_scene.visible = true
