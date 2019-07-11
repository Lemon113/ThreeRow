using Microsoft.Xna.Framework;
using System;
using ThreeRow.Engine;
using ThreeRow.Engine.Components;

namespace ThreeRow.ThreeRow
{
    class Score : GameObject
    {
        private static readonly Color SCORE_COLOR = Color.White;
        private static readonly Vector2 POSITION = new Vector2(0, 0);
        private static readonly String SCORE_TEXT = "Score: ";

        private Label _scoreLabel;
        private int _score = 0;

        public Score(Microsoft.Xna.Framework.Graphics.SpriteFont font)
        {
            _scoreLabel = AddComponent<Label>();
            _scoreLabel.Color = SCORE_COLOR;
            _scoreLabel.Font = font;
            _scoreLabel.Text = SCORE_TEXT + _score;
            _scoreLabel.Pos = POSITION;
        }

        public void AddScore(int val)
        {
            _score += val;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            _scoreLabel.Text = SCORE_TEXT + _score;
        }
    }
}
