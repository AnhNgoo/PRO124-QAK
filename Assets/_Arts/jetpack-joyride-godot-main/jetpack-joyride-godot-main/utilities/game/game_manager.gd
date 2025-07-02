class_name GameManagerGlobal
extends Node
## The global game manager.
##
## A simple global manager keeping track of game essentials.[br]
## This should be added as a global script named [param GameManager].[br]
## It's then reachable by all other components under that name.

#region CONSTANTS
## Meters per second when [member speed] is [enum Speed][param .START].
const METERS_PER_SECOND:int = 8
#endregion

#region ENUMS
## All possible game states.
enum Game {
	OVER,
	NEW,
	PLAYING,
}

## All exact scrolling speed values/steps.
## The actual speed will increment from [param START] up to [param MAX] by [param STEP] * delta each frame.
## When the game resets, the speed is set to [param RESET].
enum Speed {
	RESET = 0,
	STEP = 10,
	START = 400,
	MAX = 2000,
}
#endregion

#region VARIABLES
## The current game state.[br]
## Can only be set to one of the [enum Game] enum values.[br]
## When changed, the [signal game_changed] signal will be emitted together with the new value.[br]
## [member speed] will also change depending on the new game state.
var game: int = Game.NEW :
	set(g):
		if g == game:
			return
		
		assert(g in Game.values(), '%s is not a valid Game state.' % str(g))
		
		game = g
		
		match game:
			Game.OVER:
				speed = Speed.RESET
				SaveSystem.stats.coins += coins
				SaveSystem.save_stats()
			Game.NEW:
				speed = Speed.RESET
				distance = 0
				coins = 0
			Game.PLAYING:
				speed = Speed.START
				SaveSystem.stats.games_count += 1
		
		game_changed.emit(game)
## Is the game paused?
var paused: bool = false :
	set(p):
		paused = p
		paused_changed.emit(paused)
		get_tree().paused = paused
## The current game speed.[br]
## It can't be set higher than [enum Speed][param .MAX].[br]
var speed: float = Speed.RESET :
	set(s):
		if s > Speed.MAX:
			return
		speed = s
## The current or last run distance in meters.
var distance: float = 0.0 :
	set(d):
		if d < 0:
			return
		distance = d
		SaveSystem.stats.last_distance = floori(distance)
		distance_changed.emit(distance)
## The total amount of coins collected the last run.
var coins: int = 0 :
	set(c):
		if c < 0:
			return
		coins = c
		coins_changed.emit(coins)
#endregion

#region SIGNALS
## Emitted when [member game] changes, together with the new value.
signal game_changed(game: int)
## Emitted when [member paused] changes, together with the new value.
signal paused_changed(paused: bool)
## Emitted when [member distance] changes, together with the new value.
signal distance_changed(distance: int)
## Emitted when [member coins] changes, together with the new value.
signal coins_changed(coins: int)
#endregion

#region FUNCTIONS
## Starting and resetting the game is handled here.
func _unhandled_input(event: InputEvent) -> void:
	if game == Game.NEW and event.is_action_pressed('fly'):
		game = Game.PLAYING
	if event.is_action_pressed('ui_cancel') and game == Game.PLAYING:
		paused = !paused
	get_viewport().set_input_as_handled()

## Increasing the game speed is handled here.
func _process(delta: float) -> void:
	if game == Game.PLAYING:
		speed += Speed.STEP * delta
		distance += (METERS_PER_SECOND * delta) * (speed / Speed.START)
#endregion
