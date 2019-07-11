using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ThreeRow.Engine;
using ThreeRow.Engine.Components;

namespace ThreeRow.ThreeRow
{
    class Background : GameObject
    {
        private Texture2D _texture;
        private static readonly string TEXTURE = "bg";

        public static readonly int WIDTH = 600;
        public static readonly int HEIGHT = 700;

        public Background()
        {
            _texture = TextureManager.GetTexture(TEXTURE);
            GetComponent<Transform>().Size = new Vector2(WIDTH, HEIGHT);
            GetComponent<Sprite>().SetTexture(_texture);
        }
    }
}
