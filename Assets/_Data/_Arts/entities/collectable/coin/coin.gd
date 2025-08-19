class_name Coin
extends Node2D
## A collectable coin.

#region VARIABLES
## Used to make the coin a collectable.
@onready var collectable_2d: Collectable2D = $Collectable2D
## Used to notify if the coin enters or exits the screen.
@onready var visible_on_screen_notifier_2d: VisibleOnScreenNotifier2D = $VisibleOnScreenNotifier2D
## Used to spawn and despawn the coin.
@onready var spawnable: Spawnable = $Spawnable

## The speed of the coin.
var speed: int = 0
## Is the game playing and scrolling?
var scrolling:bool = true :
	set(s):
		if s == scrolling: return
		scrolling = s
#endregion

#region SIGNALS
## Emitted when the coin enters the screen.
signal screen_entered
#endregion

#region FUNCTIONS
func _ready():
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
	
	collectable_2d.collected.connect(collected)
	
	visible_on_screen_notifier_2d.screen_entered.connect(func(): self.screen_entered.emit())
	visible_on_screen_notifier_2d.screen_exited.connect(func(): spawnable.despawn.call_deferred())
	
	spawnable.root_node = self

func _process(delta: float) -> void:
	if scrolling:
		position.x += (-GameManager.speed - speed) * delta

## What to do when the coin is collected.
func collected() -> void:
	GameManager.coins += 1
	spawnable.despawn.call_deferred()
#endregion
