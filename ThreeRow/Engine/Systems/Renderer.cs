using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using ThreeRow.Engine.Components;

namespace ThreeRow.Engine.Systems
{
    class Renderer
    {
        private SpriteBatch _sb;
        private SortedList<int, List<GameObject>> _layeredGameObjectList;

        public Renderer(SpriteBatch sb)
        {
            _sb = sb;
            _layeredGameObjectList = new SortedList<int, List<GameObject>>();
        }

        public void RemoveAll()
        {
            foreach (var kvp in _layeredGameObjectList)
            {
                kvp.Value.Clear();
            }
            _layeredGameObjectList.Clear();
        }

        public void Update(GameTime gameTime)
        {
            _sb.Begin();
            foreach (var kvp in _layeredGameObjectList)
            {
                foreach (GameObject go in kvp.Value.ToArray())
                {
                    if (go != null)
                    {
                        Transform transform = go.transform;
                        Rectangle rect = transform.Rect;
                        Vector2 offset = transform.GetOffset();
                        rect.X += (int)offset.X;
                        rect.Y += (int)offset.Y;
                        Sprite sprite = go.GetComponent<Sprite>();
                        if (sprite != null && sprite.GetTexture() != null)
                        {
                            _sb.Draw(sprite.GetTexture(), rect, Color.White);
                        }
                        Label Label = go.GetComponent<Label>();
                        if (Label != null && Label.Text != null)
                        {
                            Vector2 goPos = new Vector2(transform.Position.X, transform.Position.Y);
                            if (Label.LeftToRight)
                            {
                                _sb.DrawString(Label.Font, Label.Text, goPos + offset + Label.Pos, Label.Color);
                            } else
                            {
                                Vector2 pos = new Vector2(Label.Pos.X - Label.Font.MeasureString(Label.Text).X, Label.Pos.Y);
                                _sb.DrawString(Label.Font, Label.Text, goPos + offset + pos, Label.Color);
                            }
                        }
                    }
                }
            }
            _sb.End();
        }

        public void AddGameObject(GameObject go)
        {
            List<GameObject> gameObjects;
            if (_layeredGameObjectList.TryGetValue((int)go.transform.Position.Z, out gameObjects))
            {
                gameObjects.Add(go);
            }
            else
            {
                _layeredGameObjectList.Add((int)go.transform.Position.Z, new List<GameObject>() { go });
            }
        }

        public void AddGameObject(GameObject go, int layer)
        {
            List<GameObject> gameObjects;
            if (_layeredGameObjectList.TryGetValue(layer, out gameObjects))
            {
                gameObjects.Add(go);
            }
            else
            {
                _layeredGameObjectList.Add(layer, new List<GameObject>() { go });
            }
        }

        public void ChangeGameObjectLayer(GameObject go, int oldLayer, int newLayer)
        {
            List<GameObject> gameObjects;
            if (_layeredGameObjectList.TryGetValue(oldLayer, out gameObjects))
            {
                gameObjects.Remove(go);
            }
            AddGameObject(go, newLayer);
        }

        public bool TryGetObjectAtPoint(Point p, out GameObject go)
        {
            IList<int> keys = _layeredGameObjectList.Keys;
            var res = keys.OrderBy(t => t);
            for (int i = res.Count() - 1; i > -1; --i)
            {
                foreach (GameObject obj in _layeredGameObjectList[res.ElementAt(i)])
                {
                    if (obj != null)
                    {
                        Point worldPos = obj.transform.GetWorldPosition();
                        Vector2 size = obj.transform.Size;
                        if (IsPointInsideRect(new Rectangle(worldPos.X, worldPos.Y, (int)size.X, (int)size.Y), p))
                        {
                            go = obj;
                            return true;
                        }
                    }
                }
            }
            go = null;
            return false;
        }

        private bool IsPointInsideRect(Rectangle rect, Point p)
        {
            return p.X >= rect.X && p.X <= (rect.X + rect.Width) && 
                p.Y >= rect.Y && p.Y <= (rect.Y + rect.Height);
        }

        public void Remove(GameObject go)
        {
            foreach (var kvp in _layeredGameObjectList)
            {
                kvp.Value.Remove(go);
            }
        }
    }
}
