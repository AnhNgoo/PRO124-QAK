class_name SaveSystemGlobal
extends Node
## The global save system.
##
## A simple global system for saving data.[br]
## This should be added as a global script named [param SaveSystem].[br]
## It's then reachable by all other components under that name.

#region CONSTANTS
## Path to the save folder.
const SAVE_FOLDER_PATH: String = 'user://save/'
## Name of the save file.
const SAVE_FILE_NAME: String = 'stats_data.tres'
## Path to the settings config file.
const SETTINGS_FILE_PATH: String = 'user://SETTINGS.cfg'
## Default values for the settings config file.
const SETTINGS_DEFAULT: Dictionary = {
  'audio': {
	'master': true,
	'music': true,
	'sfx': true,
  }
}
#endregion

#region VARIABLES
## In-game settings config.
var settings: Dictionary = {}
## In-game stats data.
var stats: StatsData = StatsData.new()
#endregion

#region FUNCTIONS
func _ready() -> void:
	if !DirAccess.dir_exists_absolute(SAVE_FOLDER_PATH):
		DirAccess.make_dir_absolute(SAVE_FOLDER_PATH)
	
	load_stats()
	load_settings()
	
	stats.game_booted_count += 1
	save_stats()

func _notification(what):
	# make sure to always save before closing down the game
	# NOTIFICATION_WM_GO_BACK_REQUEST is used for an android back-button
	if what == NOTIFICATION_WM_CLOSE_REQUEST or what == NOTIFICATION_WM_GO_BACK_REQUEST:
		save_stats()
		get_tree().quit()

## Saves the stats data.
func save_stats():
	ResourceSaver.save(stats, SAVE_FOLDER_PATH + SAVE_FILE_NAME)

## Saves the settings config.
func save_settings():
	# create new ConfigFile object
	var config = ConfigFile.new()
	
	for config_section in settings:
		for config_section_key in settings[config_section]:
			config.set_value(config_section, config_section_key, settings[config_section][config_section_key])
	
	# finally save to file
	config.save(SETTINGS_FILE_PATH)

## Loads the stats data.
func load_stats():
	if ResourceLoader.exists(SAVE_FOLDER_PATH + SAVE_FILE_NAME):
		stats = ResourceLoader.load(SAVE_FOLDER_PATH + SAVE_FILE_NAME).duplicate(true)

## Loads the settings config.
func load_settings():
	# create new ConfigFile object
	var config = ConfigFile.new()
	# load in existing settings from file
	var error = config.load(SETTINGS_FILE_PATH)
	
	# if no file
	if error != OK:
		# get default data from constant
		settings = SETTINGS_DEFAULT.duplicate(true)
		# save and return
		save_settings()
		return
	
	# set data from config file
	for config_section in config.get_sections():
		settings[config_section] = {}
		for config_section_key in config.get_section_keys(config_section):
			settings[config_section][config_section_key] = config.get_value(config_section, config_section_key)

## If the last distance was good enough, add it to the leaderboard.
func add_to_leaderboard() -> void:
	if !stats.leaderboard.is_highscore(stats.last_distance):
		return
	
	var new_entry: LeaderboardEntry = LeaderboardEntry.new(stats.name, stats.last_distance)
	stats.leaderboard.add_new_entry(new_entry)
	save_stats()
#endregion
