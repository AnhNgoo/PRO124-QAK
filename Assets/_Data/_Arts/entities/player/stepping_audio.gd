class_name SteppingAudio
extends Node2D

#region ENUMS
## The allowed values for current_step.
enum Step {
	LEFT,
	RIGHT,
}
#endregion

#region VARIABLES
@export var left_steps: Array[AudioStream] = []
@export var right_steps: Array[AudioStream] = []

@onready var audio_stream_player_2d: AudioStreamPlayer2D = $AudioStreamPlayer2D

var current_step: int = Step.LEFT
#endregion

#region FUNCTIONS
func _ready() -> void:
	audio_stream_player_2d.finished.connect(play_next_step)

## To start playing the stepping audio.
func start_playing() -> void:
	play_next_step()

## To stop playing the stepping audio.
func stop_playing() -> void:
	audio_stream_player_2d.stop()

## Play the next step audio, alternating right and left step.
func play_next_step() -> void:
	var audio: AudioStream 
	if current_step == Step.LEFT:
		current_step = Step.RIGHT
		audio = right_steps.pick_random()
	else:
		current_step = Step.LEFT
		audio = left_steps.pick_random()
	
	audio_stream_player_2d.stream = audio
	audio_stream_player_2d.play()
#endregion
