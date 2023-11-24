# osascript <<'END'
# display dialog "It Works"
# END
open -n Builds/MacOS/GAME_NAME.app --args -type server
sleep 5
open -n Builds/MacOS/GAME_NAME.app --args -type client -name player1
sleep 5
open -n Builds/MacOS/GAME_NAME.app --args -type client -name player2
sleep 5
osascript <<'END'
set thePosition to {100, 100}
tell application "System Events"
    set pidList to the unix id of (processes whose name contains "GAME PROCESS ID")
    repeat with someID in pidList -- loop
        tell (first process whose unix id is someID)
            set position of window "GAME_NAME" to thePosition
        end tell
        set item 1 of thePosition to (item 1 of thePosition) + 500 -- add 500 to the left (for the next window)
    end repeat
end tell
END
