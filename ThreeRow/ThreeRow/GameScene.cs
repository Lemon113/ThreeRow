using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreeRow.Engine;

namespace ThreeRow.ThreeRow
{
    class GameScene : IScene
    {
        public GameScene()
        {
            Game1.IsGameOver = false;
            Game1.IsStarted = false;
            Background _bg = new Background();
            _bg.SetLayer(0);
            Field f = new Field(_bg);
            new Score(Game1.Font);
            new Timer(Game1.GameTime, Game1.Font);
        }
    }
}
