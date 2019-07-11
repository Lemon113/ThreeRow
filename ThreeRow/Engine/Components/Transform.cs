using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace ThreeRow.Engine.Components
{
    class Transform : Component
    {
        public Vector3 Position;
        public Vector2 Size;
        public int Rotation;

        public Transform Parent = null;
        public List<Transform> Childs;

        public Rectangle Rect {
            get { return new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y); }
        }

        public Transform(GameObject go) : base(go)
        {
            Position = new Vector3(0f, 0f, 0f);
            Rotation = 0;
            Childs = new List<Transform>();
        }

        public void SetParent(Transform transform)
        {
            Parent = transform;
        }

        public void RemoveParent()
        {
            Parent = null;
        }

        public void AddChild(Transform transform)
        {
            Childs.Add(transform);
            transform.SetParent(this);
        }

        public void RemoveChild(Transform transform)
        {
            Childs.Remove(transform);
        }

        public Vector2 GetOffset()
        {
            Vector2 offset = Vector2.Zero;
            Transform parent = Parent;
            while (parent != null)
            {
                offset += new Vector2(parent.Position.X, parent.Position.Y);
                parent = parent.Parent;
            }
            return offset;
        }

        public Point GetWorldPosition()
        {
            Vector2 offset = GetOffset();
            Point pos = new Point(
                (int)(Position.X + offset.X),
                (int)(Position.Y + offset.Y)
                );
            return pos;
        }
    }
}
