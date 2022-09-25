# Hi Lo Guess Game challenge

## General Information

A simple implementation of a guessing game.

Players use a console application to connect to a central server, via SignalR.

The secret number is generated on start up, or everytime a player wins. The secret number is chosen from a number range defined in:

** GameServer **
> appsettings.json

## How to Play

In order to play the game you need to host the GameServer Web API, and only afterwards run the game client, you can connect has many players you wish

## Side notes
<sub>
Due to time restrictions the game client is a console application, so if another player wins/plays in the mean time, since it's a loop function, messages are only shown after player submission of a guess
Connection ports are set by default with the solution/project creation
Same applies to the proper lack of exception/error handling
</sub>