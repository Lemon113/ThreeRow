using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreeRow.Engine.Components
{
    class Label : Component
    {
        public SpriteFont Font;
        public Vector2 Pos;
        public String Text;
        public Color Color;
        public bool LeftToRight = true;

        public Label(SpriteFont font, Vector2 pos, Color color, String text)
        {
            Font = font;
            Pos = pos;
            Color = color;
            Text = text;
        }

        public Label(GameObject go) : base (go)
        {

        }

    }
}
