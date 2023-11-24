start Builds/Win64/GAME.exe -type server -posX 0 -posY 0
timeout 4
start Builds/Win64/GAME.exe -type client -name player1 -posX 0 -posY 520
timeout 3
start Builds/Win64/GAME.exe -type client -name player2 -posX 650 -posY 520