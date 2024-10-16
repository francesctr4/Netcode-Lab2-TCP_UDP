# Network and Online Games - Lab 2: TCP/UDP Socket Connection

## Proposed Activity

### Build a room manager for your videogame.

#### On your game create 2 scenes: 

* Server scene: 
  * Probably named “create game” or similar.
  * It starts a socket and waits for incoming messages from the client scene.
  * It answers with the name of the server.
* Client scene:
  * Probably named “join game”.
  * It allows to insert an IP ○ It connects to a server, knowing it’s IP.
  * It sends a message with the user’s name.
* Both scenes:
  * Bonus: Lead to a waiting room.
  * Bonus: Extend the waiting room with extra information and message exchanges in a chat.
  * Bonus: Accept several connections.

#### Implement both cases using UDP and using TCP. Extend it as you like.
