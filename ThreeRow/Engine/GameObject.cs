using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using ThreeRow.Engine.Components;

namespace ThreeRow.Engine
{
    class GameObject
    {
        private List<Component> _components;
        public Transform transform;
        private Sprite sprite;

        public GameObject()
        {
            //init
            _components = new List<Component>();
            transform = AddComponent<Transform>();
            sprite = AddComponent<Sprite>();
            GameObjects.GetInstance().Add(this);
        }

        public virtual void Start(GameTime gameTime)
        {

        }

        public virtual void Update(GameTime gameTime)
        {
            if (Game1.IsGameOver)
            {
                return;
            }
        }

        public virtual void OnClick()
        {
            if (Game1.IsGameOver)
            {
                return;
            }
        }

        public T AddComponent<T>() where T : Component
        {
            T component = (T)Activator.CreateInstance(typeof(T), new object[] { this });
            _components.Add(component);
            return component;
        }

        public T GetComponent<T>() where T : Component
        {
            foreach(Component c in _components)
            {
                if (c.GetType() == typeof(T))
                {
                    return c as T;
                }
            }
            return null;
        }

        public void SetLayer(int layer)
        {
            GameEngine.GetInstance().ChangeGameObjectLayer(this, layer);
        }

        virtual public void Destroy()
        {
            foreach(Transform t in transform.Childs)
            {
                Transform child = t;
                child.RemoveParent();
                child.gameObject.Destroy();
            }
            GameObjects.GetInstance().Remove(this);
            GameEngine.GetInstance().GetRenderer().Remove(this);
        }
    }
}
