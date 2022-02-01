# Utubz.Internal.Native
The internal native module. This provides a way of calling native methods through C#'s `DllImport` attribute. The bindings are generated with Mono's CppSharp, and some of the code in this directory is taken from resulting runtime binaries of CppSharp.

Things in this directory include:
- `Glad`: provides OpenGL functionality. Used to load OpenGL and call its functions.
- `Glfw`: a windowing and input library made with OpenGL in mind.
- `Stb`: an image loading library for converting different file formats into OpenGL-compatible data.
- `NativeUtil`: used to quickly load and unload different libraries without having to do it manually.