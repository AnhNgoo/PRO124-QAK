extends Control

#region VARIABLES
## Audio stream player to play any audio related to this UI component.
@onready var audio_stream_player: AudioStreamPlayer = $AudioStreamPlayer
## Label for displaying the total amount of collected coins.
@onready var coins_label: Label = $VBoxContainer/MarginContainer/HBoxContainer/coins
#endregion

#region FUNCTIONS
func _ready() -> void:
	coins_label.text = "%d" % floori(SaveSystem.stats.coins)
	GameManager.game_changed.connect(func(game:int):
		match game:
			GameManager.Game.NEW:
				coins_label.text = "%d" % floori(SaveSystem.stats.coins)
				visible = true
			GameManager.Game.PLAYING:
				audio_stream_player.play()
				visible = false
	)
#endregion
