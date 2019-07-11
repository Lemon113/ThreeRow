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
    class PlayButton : GameObject
    {
        public static readonly int WIDTH = 400;
        public static readonly int HEIGHT = 340;

        private static readonly String TEXTURE = "button_bg";

        private Texture2D _texture;

        public PlayButton()
        {
            _texture = TextureManager.GetTexture(TEXTURE);
            GetComponent<Sprite>().SetTexture(_texture);
            int midX = Game1.SCREEN_WIDTH / 2 - WIDTH / 2;
            int midY = Game1.SCREEN_HEIGHT / 2 - HEIGHT / 2;
            transform.Position = new Vector3(midX, midY, 0);
            transform.Size = new Vector2(WIDTH, HEIGHT);
            Label label = AddComponent<Label>();
            label.Color = Color.White;
            label.Font = Game1.Font;
            label.Text = "Play";
            Vector2 labelMeasure = label.Font.MeasureString(label.Text);
            label.Pos = new Vector2(transform.Size.X / 2 - labelMeasure.X / 2, transform.Size.Y / 2 - labelMeasure.Y / 2);
            SetLayer(1);
        }

        public override void OnClick()
        {
            base.OnClick();
            SceneManager.GetInstance().LoadScene<GameScene>();
        }
    }
}
