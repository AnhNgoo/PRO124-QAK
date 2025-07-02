class_name LeaderboardEntry
extends Resource
## Represents one entry in the leaderboard.

#region VARIABLES
## The name of the player.
@export var name: String = 'Isaac Jordan'
## The distance of the entry.
@export var distance: int = 0
## The date when the entry was created.
@export var date: String = Time.get_date_string_from_system()
#endregion

#region FUNCTIONS
## Construct of the entry data.
func _init(n: String = 'Isaac Jordan', d: int = 0):
	name = n
	distance = d
	date = Time.get_date_string_from_system()

## Is this entry greater than the provided entry?
func is_greater_than(entry: LeaderboardEntry) -> bool:
	return self.distance > entry.distance
#endregion
