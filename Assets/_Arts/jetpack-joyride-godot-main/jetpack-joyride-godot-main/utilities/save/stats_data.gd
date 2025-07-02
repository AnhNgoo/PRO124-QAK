class_name StatsData
extends Resource
## All stats data.

#region VARIABLES
## How many times has the game been booted?
@export var game_booted_count:int = 0
## How many times has a new game been started?
@export var games_count:int = 0
## The distance of the most recent run.
@export var last_distance:int = 0
## The total coints collected.
@export var coins:int = 0
## The name of the most recent player.
@export var name: String = 'Isaac Jordan'
## The leaderboard with the top scores.
@export var leaderboard: Leaderboard = Leaderboard.new()
#endregion
