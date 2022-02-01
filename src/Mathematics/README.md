# Utubz.Mathematics
The mathematics module. This was put in a separate directory as to not make the root directory so messy. It's still contained within the main `Utubz` directory for easy access however.

Things in this directory include:
- `Vectors`: provides a way of representing points or vectors in multiple dimensions within a single struct.
- `Transform`: contains transformation data for an object, such as position, rotation, and scale.
- `TMatrix`: a transformation matrix that is used internally to transform and display graphics. Unless you know what you're doing, it's better to stick with `Transform` or `EntityTransform`.