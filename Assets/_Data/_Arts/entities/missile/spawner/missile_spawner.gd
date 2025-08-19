class_name MissileSpawner
extends Spawner
## A time interval based spawner for missiles.

#region FUNCTIONS
func spawn(spawn_point: Vector2) -> void:
	spawn_point = Vector2(2000, target_node.position.y) if target_node != null else spawn_point
	super.spawn(spawn_point)
#endregion
