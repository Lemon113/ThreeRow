using Microsoft.Xna.Framework;
using System.Collections.Generic;
using ThreeRow.Engine.Systems;

namespace ThreeRow.Engine
{
    class GameEngine
    {
        private static GameEngine _INSTANCE;

        private Renderer _renderer;
        private List<ISystem> _systems;

        public static void Init()
        {
            _INSTANCE = new GameEngine();
        }

        public GameEngine()
        {
            _systems = new List<ISystem>();
            _systems.Add(new InputSystem());
            _systems.Add(SceneManager.GetInstance());
        }

        public void Start(GameTime gameTime)
        {
            GameObjects.GetInstance().Start(gameTime);
        }

        public void Update(GameTime gameTime)
        {
            foreach(ISystem sys in _systems)
            {
                sys.Update(gameTime);
            }
            GameObjects.GetInstance().Update(gameTime);
        }

        public void SetRenderer(Renderer renderer)
        {
            _renderer = renderer;
        }

        public Renderer GetRenderer()
        {
            return _renderer;
        }

        public static GameEngine GetInstance()
        {
            return _INSTANCE;
        }

        public void ChangeGameObjectLayer(GameObject go, int layer)
        {
            _renderer.ChangeGameObjectLayer(go, (int)go.transform.Position.Z, layer);
        }
    }
}
