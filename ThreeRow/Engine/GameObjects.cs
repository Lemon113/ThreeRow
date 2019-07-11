using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using ThreeRow.Engine.Systems;

namespace ThreeRow.Engine
{
    class GameObjects
    {
        private static GameObjects _INSTANCE;

        private List<GameObject> _gos;

        private GameObjects()
        {
            _gos = new List<GameObject>();
        }

        public static GameObjects GetInstance()
        {
            if (_INSTANCE == null)
            {
                _INSTANCE = new GameObjects();
            }
            return _INSTANCE;
        }

        public GameObject GetObjectOfType(Type t)
        {
            foreach(GameObject go in _gos.ToArray()) {
                if (go.GetType() == t)
                {
                    return go;
                }
            }
            return null;
        }

        public GameObject GetObjectWithName(String name)
        {
            foreach (GameObject go in _gos.ToArray())
            {
                if (nameof(go) == name)
                {
                    return go;
                }
            }
            return null;
        }

        public void Add(GameObject go)
        {
            _gos.Add(go);
            GameEngine.GetInstance().GetRenderer().AddGameObject(go);
        }

        public void Start(GameTime gameTime)
        {
            foreach (GameObject go in _INSTANCE._gos.ToArray())
            {
                if (go != null)
                {
                    go.Start(gameTime);
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            foreach (GameObject go in _INSTANCE._gos.ToArray())
            {
                if (go != null)
                {
                    go.Update(gameTime);
                }
            }
        }

        public bool TryGetObjectAtPoint(Point p, out GameObject go)
        {
            Renderer r = GameEngine.GetInstance().GetRenderer();
            return r.TryGetObjectAtPoint(p, out go);
        }

        public void Remove(GameObject go)
        {
            _gos.Remove(go);
        }

        public void RemoveAll()
        {
            _gos.Clear();
            Renderer r = GameEngine.GetInstance().GetRenderer();
            r.RemoveAll();
        }
    }
}
