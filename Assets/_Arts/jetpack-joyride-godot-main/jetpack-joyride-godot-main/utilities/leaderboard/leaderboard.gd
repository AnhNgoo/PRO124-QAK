class_name Leaderboard
extends Resource
## The leaderboard data.

#region CONSTANTS
## The max amount of highscores on the leadeboard.[br]
## Lowest entry will be replaced by a new entry.
const MAX_ENTRIES: int = 10
#endregion

#region VARIABLES
## All entries on this leaderboard.
@export var entries: Array[LeaderboardEntry] = []
#endregion

#region FUNCTIONS
## Is the distance good enough for the leaderboard?
func is_highscore(distance: int) -> bool:
	return should_entry_be_added(LeaderboardEntry.new('', distance))

## Is the entry good enough for the leaderboard?
func should_entry_be_added(entry: LeaderboardEntry) -> bool:
	if entries.size() < MAX_ENTRIES:
		return true
	return entry.is_greater_than(entries[-1])

## Add a new entry to the leaderboard if it's good enough.
func add_new_entry(new_entry: LeaderboardEntry) -> LeaderboardEntry:
	if !should_entry_be_added(new_entry):
		return null
	
	if entries.is_empty() or entries[-1].is_greater_than(new_entry):
		entries.append(new_entry)
		return new_entry
	
	for i in entries.size():
		if new_entry.is_greater_than(entries[i]):
			entries.insert(i, new_entry)
			resize_entries()
			return new_entry
	return null

## Resize the entries array if it's bigger than allowed.
func resize_entries():
	if entries.size() > MAX_ENTRIES:
		entries.resize(MAX_ENTRIES)
#endregion
