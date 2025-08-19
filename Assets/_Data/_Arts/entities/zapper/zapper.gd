class_name Zapper
extends Node2D
## A static or rotating energy field, acting as an obstacle for the player.

#region VARIABLES
## Used to spawn and despawn the zapper.
@onready var spawnable: Spawnable = $Spawnable
## Used to notify if the obstacle enters or exits the screen.
@onready var visible_on_screen_notifier_2d: VisibleOnScreenNotifier2D = $VisibleOnScreenNotifier2D

## The initial rotation in radians.
var init_rotation: float = 0
## Is it rotating?
var is_rotating: bool = false
## The speed of the zapper.
var speed: int = 0
## Is the game playing and scrolling?
var scrolling:bool = true :
	set(s):
		if s == scrolling: return
		scrolling = s
## To make sure the zapper doesn't go outside the top boundaries.
var can_rotate_within_boundary_min: int = 325
## To make sure the zapper doesn't go outside the bottom boundaries.
var can_rotate_within_boundary_max: int = 735
#endregion

#region SIGNALS
## Emitted when the obstacle enters the screen.
signal screen_entered
#endregion

#region FUNCTIONS
func _ready() -> void:
	GameManager.game_changed.connect(func(game:int):
		match game:
			GameManager.Game.OVER:
				scrolling = false
			GameManager.Game.NEW:
				scrolling = false
				spawnable.despawn.call_deferred()
			GameManager.Game.PLAYING:
				scrolling = true
	)
	
	visible_on_screen_notifier_2d.screen_entered.connect(func(): self.screen_entered.emit())
	visible_on_screen_notifier_2d.screen_exited.connect(func(): spawnable.despawn.call_deferred())
	
	spawnable.root_node = self
	spawnable.spawned.connect(func(_spawn_point: Vector2, _target_node: Node2D):
		var is_within_rotatable_boundary: bool = position.y >= can_rotate_within_boundary_min and position.y <= can_rotate_within_boundary_max
		is_rotating = is_within_rotatable_boundary and randi_range(0, 2) == 0
		rotation_degrees = 90 if !is_within_rotatable_boundary else 0
	)

func _process(delta: float) -> void:
	if scrolling:
		position.x += (-GameManager.speed - speed) * delta
	if is_rotating:
		rotation -= 1 * 1.5 * delta
#endregion
