class_name CoinSpawner
extends Spawner
## A time interval based spawner for missiles.

#region VARIABLES
## Used to spawn coins spaced out.
var between_coins_timer: Timer = Timer.new()
## Used to interupt spawning when the game state changes.
var interupt_spawning: bool = false
#endregion

#region FUNCTIONS
func _ready() -> void:
	super._ready()
	
	add_child(between_coins_timer)
	
	GameManager.game_changed.connect(func(game: int):
		match game:
			GameManager.Game.OVER, GameManager.Game.NEW:
				interupt_spawning = true
	)

func on_time_to_spawn() -> void:
	timer.stop()
	var spawn_point = SpawnerManager.get_available_spawn_point()
	if spawn_point is not Vector2:
		timer.start(get_time())
		return
	
	var amount = randi_range(5, 10)
	for c in amount:
		if interupt_spawning:
			interupt_spawning = false
			break
		
		var interval_divider = GameManager.speed / GameManager.Speed.START
		between_coins_timer.start(0.3 / interval_divider)
		spawn(spawn_point)
		await between_coins_timer.timeout
		between_coins_timer.stop()
	
	finished_spawning.emit(spawn_point)
#endregion
