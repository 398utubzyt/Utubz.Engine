# Utubz.Internal.Platforms
The internal platforms module. This provides a way of accessing native libraries without all the `#if`s in the code, resulting in too many binaries.

Things in this directory include:
- `Platform`: an abstract class to be implemented by derived classes. These classes will serve as a sort of messager between the engine and the native libraries.