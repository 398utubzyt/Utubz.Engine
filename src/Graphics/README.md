# Utubz.Graphics
The graphics module. This directory provides most of the functions and data conversions to send stuff to the GPU through the GL library.

Things in this directory include:
- `GL`: the lowest level you can get outside of the engine's project. This class provides functions that resemble an OpenGL-like system.
- `Color`: a way of representing color through a single struct.
- `Camera`: the "point-of-view" of the user. Provides transformation data to fully transform the scene into window coordinates.
- `Shader`: a representation of a shader. You can convert GLSL into this shader class through `Shader.ParseGLSL(string)`.
- `Viewport`: provides the viewport size, position and aspect ratio. It doesn't have much functionality other than that.
- `Texture`: contains data that represents an image. At the moment, it cannot be read from or written to.
- `Renderer`: the class that sends data to the GPU for rendering, such as shader, texture, and vertex data.