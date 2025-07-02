class_name LeaderboardEntryUI
extends Control
## The UI component for every entry in the leaderboard.

#region VARIABLES
## The label for number order on the leaderboard.
@onready var order_label: Label = $order_label
## The label for the distance.
@onready var distance_label: Label = $distance_label
## The label for the player name.
@onready var name_label: Label = $name_label
## The label for the date when this entry was created.
@onready var date_label: Label = $date_label

## The number order on the leaderboard.
var order: int = 1
## The entry data.
var resource: LeaderboardEntry = LeaderboardEntry.new()
#endregion

#region FUNCTIONS
## Used to set up the labels from the resource data.
func setup_labels() -> void:
	order_label.text = '%s.' % str(order)
	distance_label.text = '%dM' % floori(resource.distance)
	name_label.text = '%s' % resource.name
	date_label.text = '%s' % resource.date.replace('-', '.')
#endregion
