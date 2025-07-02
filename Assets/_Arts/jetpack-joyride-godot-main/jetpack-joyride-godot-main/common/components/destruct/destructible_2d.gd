class_name Destructable2D
extends Area2D
## A simple component to make anything a destructable.

#region VARIABLES
## Amount of health before being being destroyed.
@export_range(1, 10) var health: int = 1 :
	set(h):
		health = h if h >= 0 else 0
		if health == 0:
			destroyed.emit()

## To play audio when hit.
@onready var audio_stream_player_2d: AudioStreamPlayer2D = $AudioStreamPlayer2D
#endregion

#region SIGNALS
## Emitted when destroyed.
signal destroyed
#endregion

#region FUNCTIONS
## Called by a destructor to make this destructable take damage.
func destruct(amount: int = health, audio_stream: AudioStream = null) -> void:
	health -= amount
	if audio_stream != null:
		audio_stream_player_2d.stream = audio_stream
		audio_stream_player_2d.play()
#endregion
