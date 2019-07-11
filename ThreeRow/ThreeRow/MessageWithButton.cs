using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreeRow.Engine;
using ThreeRow.Engine.Components;

namespace ThreeRow.ThreeRow
{
    class MessageWithButton : GameObject
    {
        public static int WIDTH = 300;
        public static int HEIGHT = 400;
        public static String text = "Game Over";

        private static readonly String TEXTURE = "popup_base";

        private Texture2D _texture;
        private SpriteFont font;

        public MessageWithButton()
        {
            font = Game1.Font;
            _texture = TextureManager.GetTexture(TEXTURE);
            transform.Size = new Vector2(WIDTH, HEIGHT);
            transform.Position = new Vector3(Game1.SCREEN_WIDTH / 2 - transform.Size.X / 2, Game1.SCREEN_HEIGHT / 2 - transform.Size.Y / 2, 0);
            GetComponent<Sprite>().SetTexture(_texture);
            SetLayer(5);

            Label label = AddComponent<Label>();
            label.Color = Color.Black;
            label.Font = Game1.Font;
            label.Text = text;
            Vector2 labelMeasure = label.Font.MeasureString(label.Text);
            label.Pos = new Vector2(transform.Size.X / 2 - labelMeasure.X / 2, transform.Size.Y / 3 - labelMeasure.Y / 2);

            MenuButton btn = new MenuButton();
            btn.transform.SetParent(transform);
            btn.SetLayer(6);
        }
    
    }
}
