class_name GameOverScreen
extends Control
## The game over UI.

#region VARIABLES
## Audio to be played when the game is paused.
@export var audio_highscore: AudioStream
## Audio to be played when a button is pressed.
@export var audio_select: AudioStream
## Leaderboard entry scene to populate the leaderboard with.
@export var leaderboard_entry_scene: PackedScene

## Audio stream player to play any audio related to this UI component.
@onready var audio_stream_player: AudioStreamPlayer = $AudioStreamPlayer
## Button used to save the leaderboard name and continue.
@onready var button_continue: Button = $HBoxContainer/MarginContainer/highscore/continue_button
## The home button, which will reset but wait on the player to start a run.
@onready var button_home: Button = $HBoxContainer/MarginContainer/leaderboard/home_button
## The restart button, which will start a run instantly.
@onready var button_restart: Button = $HBoxContainer/MarginContainer/leaderboard/restart_button
## The container for editing the name for the most recent score on the leaderboard.
@onready var container_highscore: VBoxContainer = $HBoxContainer/MarginContainer/highscore
## The container to display the leaderboard.
@onready var container_leaderboard: VBoxContainer = $HBoxContainer/MarginContainer/leaderboard
## The container for leaderboard entries.
@onready var container_leaderboard_entries: VBoxContainer = $HBoxContainer/MarginContainer/leaderboard/TabContainer/Leaderboard/MarginContainer/VBoxContainer
## The label for the displaying the collected coins.
@onready var label_coins: Label = $HBoxContainer/VBoxContainer/HBoxContainer/coins
## The label for the displaying the current distance.
@onready var label_distance: Label = $HBoxContainer/VBoxContainer/distance
## LineEdit node for entering the name for the leaderboard entry.
@onready var line_edit_leaderboard_name: LineEdit = $HBoxContainer/MarginContainer/highscore/leaderboard_name

## Is the latest run a new highscore?
var new_highscore: bool = false :
	set(h):
		new_highscore = h
		line_edit_leaderboard_name.text = SaveSystem.stats.name
		container_highscore.visible = new_highscore
		show_leaderboard = !new_highscore
## Should the leaderboard be visible?
var show_leaderboard: bool = false :
	set(l):
		show_leaderboard = l
		if show_leaderboard:
			generate_leaderboard()
		container_leaderboard.visible = show_leaderboard
		container_highscore.visible = !show_leaderboard
#endregion

#region FUNCTIONS
func _ready() -> void:
	GameManager.game_changed.connect(on_game_changed)
	button_continue.pressed.connect(on_continue)
	button_home.pressed.connect(on_home)
	button_restart.pressed.connect(on_restart)

## Generating the leaderboard from the data in the save system.
func generate_leaderboard():
	for i in SaveSystem.stats.leaderboard.entries.size():
		var entry: LeaderboardEntry = SaveSystem.stats.leaderboard.entries[i]
		var entry_ui: LeaderboardEntryUI
		if container_leaderboard_entries.get_child_count() > i and container_leaderboard_entries.get_child(i) is LeaderboardEntryUI:
			entry_ui = container_leaderboard_entries.get_child(i)
		else:
			entry_ui = leaderboard_entry_scene.instantiate()
			container_leaderboard_entries.add_child(entry_ui)
		entry_ui.order = i + 1
		entry_ui.resource = entry
		entry_ui.setup_labels()

## What to do when continuing.
func on_continue() -> void:
	SaveSystem.stats.name = line_edit_leaderboard_name.text if !line_edit_leaderboard_name.text.is_empty() else SaveSystem.stats.name
	SaveSystem.add_to_leaderboard()
	show_leaderboard = true
	play_audio(audio_highscore)

## What to do when the game state changed.
func on_game_changed(game: int) -> void:
	match game:
		GameManager.Game.NEW:
			visible = false
		GameManager.Game.OVER:
			label_distance.text = "%dM" % floori(GameManager.distance)
			label_coins.text = "%d" % floori(GameManager.coins)
			new_highscore = SaveSystem.stats.leaderboard.is_highscore(int(GameManager.distance))
			visible = true

## What to do when going back to home.
func on_home() -> void:
	GameManager.game = GameManager.Game.NEW
	play_audio(audio_select)

## What to do when restarting.
func on_restart() -> void:
	GameManager.game = GameManager.Game.NEW; GameManager.game = GameManager.Game.PLAYING
	play_audio(audio_select)

## Play the provided audio stream.
func play_audio(audio_stream: AudioStream) -> void:
	if audio_stream == null:
		return
	audio_stream_player.stream = audio_stream
	audio_stream_player.play()
#endregion
