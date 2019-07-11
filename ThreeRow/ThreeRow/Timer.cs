using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using ThreeRow.Engine;
using ThreeRow.Engine.Components;

namespace ThreeRow.ThreeRow
{
    class Timer : GameObject
    {
        private static readonly Color SCORE_COLOR = Color.White;
        private static readonly Vector2 POSITION = new Vector2(Game1.SCREEN_WIDTH, 0);
        private static readonly String SCORE_TEXT = "Time: ";
        private static readonly int TIME = 60;

        private Label _timerLabel;
        private int _currentTime = 60;

        public Timer(GameTime gameTime, SpriteFont font)
        {
            _timerLabel = AddComponent<Label>();
            _timerLabel.Color = SCORE_COLOR;
            _timerLabel.Font = font;
            _timerLabel.Text = SCORE_TEXT + _currentTime;
            _timerLabel.Pos = POSITION;
            _timerLabel.LeftToRight = false;
            gameTime.TotalGameTime = TimeSpan.FromSeconds(0);
        }

        public override void Start(GameTime gameTime)
        {
            base.Start(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (gameTime.TotalGameTime.Seconds < TIME && gameTime.TotalGameTime.Minutes < 1)
            {
                _currentTime = TIME - gameTime.TotalGameTime.Seconds;
                _timerLabel.Text = SCORE_TEXT + _currentTime;
            } else
            {
                _currentTime = 0;
                _timerLabel.Text = SCORE_TEXT + _currentTime;
                if (!Game1.IsGameOver)
                {
                    GameObjects.GetInstance().RemoveAll();
                    new MessageWithButton();
                    Game1.IsGameOver = true;
                }
            }
            
        }
    }
}
