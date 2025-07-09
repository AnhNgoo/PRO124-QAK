class_name PauseMenu
extends Control
## The pause menu.

#region VARIABLES
## Audio to be played when the game is paused.
@export var audio_paused: AudioStream
## Audio to be played when a button is pressed.
@export var audio_select: AudioStream

## The index for the sfx audio bus.
@onready var audio_bus_sfx_index: int = AudioServer.get_bus_index('sfx')
## The index for the music audio bus.
@onready var audio_bus_music_index: int = AudioServer.get_bus_index('music')
## Audio stream player to play any audio related to this UI component.
@onready var audio_stream_player: AudioStreamPlayer = $AudioStreamPlayer
## Button to toggle music.
@onready var button_music: TextureButton = $VBoxContainer/TextureRect/HBoxContainer/button_music
## Button to toggle SFX.
@onready var button_sfx: TextureButton = $VBoxContainer/TextureRect/HBoxContainer/button_sfx
## Button to go back home.
@onready var home_button: Button = $VBoxContainer/VBoxContainer/home_button
## Button to restart the a run.
@onready var restart_button: Button = $VBoxContainer/VBoxContainer/restart_button
## Button to unpause the game.
@onready var resume_button: Button = $VBoxContainer/VBoxContainer/resume_button
#endregion

#region FUNCTIONS
func _unhandled_input(event: InputEvent) -> void:
	if event.is_action_pressed('ui_cancel') and GameManager.game == GameManager.Game.PLAYING:
		GameManager.paused = !GameManager.paused
		get_viewport().set_input_as_handled()
		if GameManager.paused:
			return
		play_audio(audio_paused)

func _ready() -> void:
	button_music.toggled.connect(on_music_toggled)
	button_sfx.toggled.connect(on_sfx_toggled)
	GameManager.paused_changed.connect(on_paused_changed)
	home_button.pressed.connect(on_home_button_pressed)
	resume_button.pressed.connect(on_resume_button_pressed)
	restart_button.pressed.connect(on_restart_button_pressed)
	
	button_music.set_pressed_no_signal(!SaveSystem.settings.audio.music)
	button_sfx.set_pressed_no_signal(!SaveSystem.settings.audio.sfx)
	
	AudioServer.set_bus_mute(audio_bus_music_index, !SaveSystem.settings.audio.music)
	AudioServer.set_bus_mute(audio_bus_sfx_index, !SaveSystem.settings.audio.sfx)

## What to do when the home button is pressed.
func on_home_button_pressed() -> void:
	GameManager.game = GameManager.Game.NEW
	GameManager.paused = false
	play_audio(audio_select)

## What to do when the music is toggled.
func on_music_toggled(toggled_on: bool) -> void:
	play_audio(audio_select)
	AudioServer.set_bus_mute(audio_bus_music_index, toggled_on)
	SaveSystem.settings.audio.music = !toggled_on
	SaveSystem.save_settings()

## What to do when the paused state is changed.
func on_paused_changed(paused: bool) -> void:
	visible = paused
	if !paused:
		return
	play_audio(audio_paused)

## What to do when the restart button is pressed.
func on_restart_button_pressed() -> void:
	GameManager.game = GameManager.Game.NEW
	GameManager.game = GameManager.Game.PLAYING
	GameManager.paused = false
	play_audio(audio_select)

## What to do when the resume button is pressed.
func on_resume_button_pressed() -> void:
	GameManager.paused = false
	play_audio(audio_select)

## What to do when the SFX are toggled.
func on_sfx_toggled(toggled_on: bool) -> void:
	play_audio(audio_select)
	AudioServer.set_bus_mute(audio_bus_sfx_index, toggled_on)
	SaveSystem.settings.audio.sfx = !toggled_on
	SaveSystem.save_settings()

## Play the provided audio stream.
func play_audio(audio_stream: AudioStream) -> void:
	if audio_stream == null:
		return
	audio_stream_player.stream = audio_stream
	audio_stream_player.play()
#endregion
