using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utubz
{
    public sealed class EntityTransform : Object
    {
        private Transform t;
        public Entity Entity { get; }
        public Transform Transform { get => t; set => t = value; }

        public void SetPosition(Vector3 position) => t.Position = position;
        public void SetRotation(Vector3 rotation) => t.Rotation = rotation;
        public void SetScale(Vector3 scale) => t.Scale = scale;

        public void SetPosition(Vector2 position) { t.Position.x = position.x; t.Position.y = position.y; }
        public void SetRotation(Vector2 rotation) { t.Rotation.x = rotation.x; t.Rotation.y = rotation.y; }
        public void SetScale(Vector2 scale) { t.Scale.x = scale.x; t.Scale.y = scale.y; }

        public void SetPosition(float x, float y, float z) { t.Position.x = x; t.Position.y = y; t.Position.z = z; }
        public void SetRotation(float x, float y, float z) { t.Rotation.x = x; t.Rotation.y = y; t.Rotation.z = z; }
        public void SetScale(float x, float y, float z) { t.Scale.x = x; t.Scale.y = y; t.Scale.z = z; }

        public void SetPosition(float x, float y) { t.Position.x = x; t.Position.y = y; }
        public void SetRotation(float x, float y) { t.Rotation.x = x; t.Rotation.y = y; }
        public void SetScale(float x, float y) { t.Scale.x = x; t.Scale.y = y; }

        public void Translate(Vector3 tra) => t.Translate(tra);
        public void Rotate(Vector3 rot) => t.Rotate(rot);
        public void Dilate(Vector3 dil) => t.Dilate(dil);

        public Vector3 Relative(Vector3 vec) => t.Relative(vec);
        public Vector3 RelativeNoY(Vector3 vec) => t.RelativeNoY(vec);

        public Vector3 Position { get => t.Position; set => t.Position = value; }
        public Vector3 Rotation { get => t.Rotation; set => t.Rotation = value; }
        public Vector3 Scale { get => t.Scale; set => t.Scale = value; }

        public float xPos { get => t.Position.x; set => t.Position.x = value; }
        public float yPos { get => t.Position.y; set => t.Position.y = value; }
        public float zPos { get => t.Position.z; set => t.Position.z = value; }

        public float xRot { get => t.Rotation.x; set => t.Rotation.x = value; }
        public float yRot { get => t.Rotation.y; set => t.Rotation.y = value; }
        public float zRot { get => t.Rotation.z; set => t.Rotation.z = value; }

        public float xScl { get => t.Scale.x; set => t.Scale.x = value; }
        public float yScl { get => t.Scale.y; set => t.Scale.y = value; }
        public float zScl { get => t.Scale.z; set => t.Scale.z = value; }

        public Vector3 Right => t.Right;
        public Vector3 Up => t.Up;
        public Vector3 Forward => t.Forward;

        public override string ToString()
        {
            return t.ToString();
        }

        public EntityTransform(Entity entity)
        {
            t = new Transform();
            Entity = entity;
        }
    }
}
