using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using ThreeRow.Engine.Systems;

namespace ThreeRow.Engine
{
    public class SceneManager : ISystem
    {
        private static SceneManager INSTANCE = null;
        private Type t;

        public void LoadScene<T>() where T : IScene, new()
        {
            t = typeof(T);
        }

        public void Update(GameTime gameTime)
        {
            if (t != null)
            {
                GameObjects.GetInstance().RemoveAll();
                Activator.CreateInstance(t);
                t = null;
            }
        }

        public static SceneManager GetInstance()
        {
            if (INSTANCE == null)
            {
                INSTANCE = new SceneManager();
            }
            return INSTANCE;
        }
    }
}
