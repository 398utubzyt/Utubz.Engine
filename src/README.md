# Utubz
The base engine. This provides basic engine functionality.

Things inside this directory include:
- `Application`: contains things related to the main process such as its location, the main window, and even stopping the process.
- `Input`: allows you to access the current state of the main keyboard, mouse, controller, etc.
- `Math`: so that you don't need to access `System.Math`.
- `Time`: to get the details about how long it's been since the last frame, or when the process started.
- `Component`: allows for an automatic game-loop that runs code every frame. 