using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ThreeRow.Engine;
using ThreeRow.Engine.Components;

namespace ThreeRow.ThreeRow
{
    class LineBonus : GameObject, IBonus
    {
        public static readonly int WIDTH = 24;
        public static readonly int HEIGHT = 24;

        public enum LineType
        {
            VERTICAL,
            HORIZONTAL,
            BOTH
        }
        public LineType Type {
            private set;
            get;
        }

        private static readonly string TEXTURE_HORIZ = "horiz";
        private static readonly string TEXTURE_VERT = "vert";
        private static readonly string TEXTURE_BOTH = "both";

        private Texture2D _texture;

        public LineBonus(LineType type)
        {
            Type = type;
            switch(Type)
            {
                case LineType.VERTICAL: _texture = TextureManager.GetTexture(TEXTURE_VERT); break;
                case LineType.HORIZONTAL: _texture = TextureManager.GetTexture(TEXTURE_HORIZ);  break;
                case LineType.BOTH: _texture = TextureManager.GetTexture(TEXTURE_BOTH);  break;
            }
            GetComponent<Transform>().Size = new Vector2(WIDTH, HEIGHT);
            GetComponent<Sprite>().SetTexture(_texture);
            SetLayer(2);
        }

        public void Activate()
        {
            Cell cell;
            if (transform.Parent != null && transform.Parent.gameObject is Cell)
            {
                cell = transform.Parent.gameObject as Cell;
                switch(Type)
                {
                    case LineType.HORIZONTAL:
                        new Destroyer(cell, Destroyer.Direction.RIGHT);
                        new Destroyer(cell, Destroyer.Direction.LEFT);
                        break;
                    case LineType.VERTICAL:
                        new Destroyer(cell, Destroyer.Direction.UP);
                        new Destroyer(cell, Destroyer.Direction.DOWN);
                        break;
                    case LineType.BOTH:
                        new Destroyer(cell, Destroyer.Direction.RIGHT);
                        new Destroyer(cell, Destroyer.Direction.LEFT);
                        new Destroyer(cell, Destroyer.Direction.UP);
                        new Destroyer(cell, Destroyer.Direction.DOWN);
                        break;
                }
            }
        }

        override public void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
