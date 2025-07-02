class_name HUD
extends Control
## The game HUD (heads-up display).
##
## This contains stats and pause button.

#region VARIABLES
## The label for the displaying the collected coins during the current run.
@onready var coins_label: Label = $HBoxContainer/VBoxContainer/MarginContainer2/HBoxContainer/coins
## The label for the displaying the current distance.
@onready var distance_label: Label = $HBoxContainer/VBoxContainer/MarginContainer/distance
## Button to pause the game.
@onready var pause_button: TextureButton = $HBoxContainer/VBoxContainer3/MarginContainer/pause_button
#endregion

#region FUNCTIONS
func _ready() -> void:
	GameManager.game_changed.connect(func(game: int):
		match game:
			GameManager.Game.OVER, GameManager.Game.NEW:
				visible = false
			GameManager.Game.PLAYING:
				visible = true
	)
	GameManager.distance_changed.connect(func(distance: float): distance_label.text = "%03dM" % floori(distance))
	GameManager.coins_changed.connect(func(coins: float): coins_label.text = "%03d" % floori(coins))
	pause_button.pressed.connect(func(): GameManager.paused = true)
#endregion
