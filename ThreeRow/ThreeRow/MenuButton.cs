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
    class MenuButton : GameObject
    {
        public static readonly int WIDTH = 400;
        public static readonly int HEIGHT = 340;

        private static readonly String TEXTURE = "button_bg";

        private int _midX;
        private int _midY;
        private Texture2D _texture;

        public MenuButton()
        {
            _texture = TextureManager.GetTexture(TEXTURE);
            GetComponent<Sprite>().SetTexture(_texture);
            _midX = Game1.SCREEN_WIDTH / 2 - WIDTH / 2;
            _midY = Game1.SCREEN_HEIGHT / 2 - HEIGHT / 2;
            transform.Position = new Vector3(_midX, _midY, 0);
            transform.Size = new Vector2(WIDTH, HEIGHT);
            Label label = AddComponent<Label>();
            label.Color = Color.White;
            label.Font = Game1.Font;
            label.Text = "Menu";
            Vector2 labelMeasure = label.Font.MeasureString(label.Text);
            label.Pos = new Vector2(transform.Size.X / 2 - labelMeasure.X / 2, transform.Size.Y / 2 - labelMeasure.Y / 2);
            SetLayer(1);
        }

        public override void OnClick()
        {
            base.OnClick();
            SceneManager.GetInstance().LoadScene<MenuScene>();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (transform.Parent != null)
            {
                _midX = (int)transform.Parent.Size.X / 2 - WIDTH / 2;
                _midY = (int)transform.Parent.Size.Y / 2 - HEIGHT / 2;
                transform.Position = new Vector3(_midX, _midY + 100, 0);
            }
        }
    }
}
