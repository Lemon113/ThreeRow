using Microsoft.Xna.Framework.Graphics;

namespace ThreeRow.Engine
{
    class Sprite : Component
    {
        private Texture2D texture;

        public Sprite(GameObject go) : base(go)
        {

        }

        public void SetTexture(Texture2D _texture)
        {
            texture = _texture;
        }

        public Texture2D GetTexture()
        {
            return texture;
        }
    }
}
