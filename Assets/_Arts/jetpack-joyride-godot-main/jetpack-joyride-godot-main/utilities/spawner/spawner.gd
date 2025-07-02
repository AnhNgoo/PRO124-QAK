class_name Spawner
extends Node2D
## A time interval based spawner.

#region CONSTANTS
## An array of possible spawn points.
const possible_spawn_points: Array[Vector2] = []
#endregion

#region VARIABLES
## Node scene to be instantiated and spawned.
@export var node_scene: PackedScene
## The minimum time between spawnables.[br]
## This will decrease while the game speed increases.
@export var interval_min: float = 1.0
## The maximum time between spawnables.[br]
## This will decrease while the game speed increases.
@export var interval_max: float = 5.0
## The optional target node (to look, aim, follow, etc).
@export var target_node: Node2D = null
## All instantiated but despawned spawnables are collected here.
var spawnable_pool: Array[Spawnable] = []
## The timer for this spawner.
var timer: Timer = Timer.new()
## Time left before spawning.
var time_left: float = 0.0
#endregion

#region SIGNALS
## Emitted when the spawner just finished spawning.
signal finished_spawning(spawn_point: Variant)
#endregion

#region FUNCTIONS
func _ready() -> void:
	timer.timeout.connect(on_time_to_spawn)
	add_child(timer)
	
	GameManager.game_changed.connect(func(game: int):
		match game:
			GameManager.Game.PLAYING:
				timer.start(get_time())
			GameManager.Game.OVER, GameManager.Game.NEW:
				timer.stop()
	)
	
	finished_spawning.connect(on_finished_spawning)

## Returns a random time depending on the game speed.
func get_time() -> float:
	var interval_divider = GameManager.speed / GameManager.Speed.START
	return randf_range(
		interval_min / interval_divider,
		interval_max / interval_divider
	)

## What should happen when it's time to spawn?
func on_time_to_spawn() -> void:
	timer.stop()
	var spawn_point = SpawnerManager.get_available_spawn_point()
	if spawn_point is not Vector2:
		timer.start(get_time())
		return
	
	spawn(spawn_point)
	finished_spawning.emit(spawn_point)

## What should happen when the spawner just finished spawning?
func on_finished_spawning(spawn_point: Vector2) -> void:
	SpawnerManager.make_spawn_point_available(spawn_point)
	timer.start(get_time())

## Get spawnable component from node.
func get_spawnable_from_node(node: Node) -> Spawnable:
	if node is Spawnable: return node
	for child in node.get_children(true):
		if child is Spawnable: return child
	return null

## Tries to add the provided scene as a node to the scene tree, and return a spawnable.
func add_node_and_get_spawnable() -> Spawnable:
	var spawnable: Spawnable = spawnable_pool.pop_front()
	if spawnable == null:
		var node: Node = node_scene.instantiate()
		spawnable = get_spawnable_from_node(node)
		if spawnable == null:
			return null
		
		add_child(node)
		spawnable.despawned.connect(func(_new_position: Vector2): spawnable_pool.append(spawnable))
	
	return spawnable

## Spawn the specific spawnable assigned to this spawner.
func spawn(spawn_point: Vector2) -> void:
	# check if movement has stopped before continuing
	if GameManager.game != GameManager.Game.PLAYING:
		return
	
	var spawnable: Spawnable = add_node_and_get_spawnable()
	if spawnable == null:
		return
	
	spawnable.spawn(spawn_point, target_node)
#endregion
