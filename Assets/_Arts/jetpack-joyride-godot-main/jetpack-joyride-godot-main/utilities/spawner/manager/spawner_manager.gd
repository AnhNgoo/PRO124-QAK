class_name SpawnerManagerGlobal
extends Node
## The global spawner manager.
##
## A simple global manager keeping track of spawner essentials.[br]
## This should be added as a global script named [param SpawnerManager].[br]
## It's then reachable by all other components under that name.

#region CONSTANTS
## All possible spawn points.
const possible_spawn_points: Array[Vector2] = [
	Vector2(2000, 120),
	Vector2(2000, 325),
	Vector2(2000, 530),
	Vector2(2000, 735),
	Vector2(2000, 940),
]
#endregion

#region VARIABLES
## All the available spawn points.
var available_spawn_points: Array[Vector2] = []
#endregion

#region FUNCTIONS
func _ready() -> void:
	GameManager.game_changed.connect(func(game: int):
		match game:
			GameManager.Game.PLAYING:
				reset_available_spawn_points()
	)

## Make all possible spawn points available again.
func reset_available_spawn_points() -> void:
	for spawn_point in possible_spawn_points:
		make_spawn_point_available(spawn_point, 0.0)

## Used to get an available spawn point.
## Will return a Vector2 if any spawn points are available.
## Otherwise null will be returned.
func get_available_spawn_point() -> Variant:
	if available_spawn_points.is_empty():
		return null
	
	var random_index = randi_range(0, available_spawn_points.size() - 1)
	return available_spawn_points.pop_at(random_index)

## Used to make a spawn point available.
func make_spawn_point_available(spawn_point: Vector2, cool_down: float = 0.3) -> void:
	if available_spawn_points.has(spawn_point):
		return
	await get_tree().create_timer(cool_down).timeout
	available_spawn_points.append(spawn_point)
#endregion
