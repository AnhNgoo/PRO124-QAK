class_name Player
extends CharacterBody2D
## The main script for the player.

#region CONSTANTS
## How much the force is increasing every frame * delta when the jetpack is activated.
const JETPACK_FORCE_INCREMENT_STEP:int = 4000
## Highest amount of force for the jetpack.
const JETPACK_MAX_FORCE:int = 5000
## Highest fall speed.
const MAX_FALL_SPEED = 1200
#endregion

#region VARIABLES
## Basic jetpack particles.
@onready var bullet_particles: GPUParticles2D = $bullet_particles
## The destructable is responsible to take damage when colliding with destructors.
@onready var destructable_2d: Destructable2D = $Destructable2D
## The audio stream player for when the player runs.
@onready var stepping_audio: SteppingAudio = $SteppingAudio
## The audio stream player for when the jetpack is activated.
@onready var jetpack_audio_stream_player: AudioStreamPlayer2D = $jetpack_audio_stream_player

## Current jetpack force applied.[br]
## Can only be set between [param 0] and [constant JETPACK_MAX_FORCE].[br]
## It will be set to [param 0] if [member jetpack_activated] is set to [param false].
var jetpack_force:float = 0 :
	set(f):
		if f < 0 or f > JETPACK_MAX_FORCE:
			return
		jetpack_force = f
## Is the jetpack on?[br]
## If [param true]: [member jetpack_force] will be applied to the velocity, and [member bullet_particles] will start emitting particles.[br]
## If [param false]: [member jetpack_force] will be set to [param 0].
var jetpack_activated:bool = false :
	set(a):
		if a == jetpack_activated:
			return
		
		jetpack_activated = a
		bullet_particles.emitting = jetpack_activated
		if jetpack_activated:
			jetpack_audio_stream_player.play()
		else:
			jetpack_audio_stream_player.stop()
			jetpack_force = 0
var grounded:bool = false :
	set(g):
		if g == grounded:
			return
		grounded = g
		if grounded:
			stepping_audio.start_playing()
		else:
			stepping_audio.stop_playing()
#endregion

#region FUNCTIONS
func _ready() -> void:
	GameManager.game_changed.connect(func(game:int):
		match game:
			GameManager.Game.NEW:
				reset()
	)
	
	destructable_2d.destroyed.connect(func(): GameManager.game = GameManager.Game.OVER)

## Used to handle input (only flying)
func _unhandled_input(event: InputEvent) -> void:
	if GameManager.game != GameManager.Game.PLAYING:
		return
	
	if event.is_action('fly'):
		jetpack_activated = Input.is_action_pressed('fly')

func _process(delta: float) -> void:
	# for now, just ignore all physics if the game is over
	if GameManager.game == GameManager.Game.OVER:
		return
	
	if jetpack_activated:
		jetpack_force += JETPACK_FORCE_INCREMENT_STEP * delta

func _physics_process(delta: float) -> void:
	# for now, just ignore all physics if the game is over
	if GameManager.game == GameManager.Game.OVER:
		return
	
	grounded = is_on_floor()
	
	if jetpack_activated:
		velocity.y = -jetpack_force
	if !jetpack_activated and not is_on_floor() and velocity.y < MAX_FALL_SPEED:
		velocity.y += get_gravity().y * delta

	move_and_slide()

## Reset the player.[br]
## Triggered by the [signal GameManagerGlobal.game] signal when set to [enum GameManagerGlobal.Game][param .NEW].
func reset():
	velocity = Vector2.ZERO
	bullet_particles.restart()
	bullet_particles.emitting = false
	jetpack_activated = false
	position = Vector2(600, 940)
#endregion
