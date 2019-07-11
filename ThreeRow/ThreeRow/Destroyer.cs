using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ThreeRow.Engine;
using ThreeRow.Engine.Components;

namespace ThreeRow.ThreeRow
{
    class Destroyer : GameObject
    {
        public enum Direction
        {
            UP,
            DOWN,
            LEFT,
            RIGHT
        }

        public static readonly int WIDTH = 50;
        public static readonly int HEIGHT = 50;

        private static readonly string TEXTURE = "destroyer";

        private float _moveSpeed = 15;
        private Direction _direction;
        private Cell _cell;
        private Field _field;
        private Texture2D _texture;
        private bool _isMoving = true;
        private int[] _currTargetPos = new int[2];

        public Destroyer(Cell cell, Direction direction)
        {
            _cell = cell;
            _field = cell.Field;
            _direction = direction;
            cell.GetPos().CopyTo(_currTargetPos, 0);
            
            switch (_direction)
            {
                case Direction.UP:  _currTargetPos[0] -= 1; break;
                case Direction.DOWN: _currTargetPos[0] += 1; break;
                case Direction.LEFT: _currTargetPos[1] -= 1; break;
                case Direction.RIGHT: _currTargetPos[1] += 1;  break;
            }
            transform.Size = new Vector2(WIDTH, HEIGHT);
            _texture = TextureManager.GetTexture(TEXTURE);
            GetComponent<Sprite>().SetTexture(_texture);
            transform.Position = cell.transform.Position;
            cell.Field.transform.AddChild(transform);
            SetLayer(4);
            StartMoving();
        }

        private async void StartMoving()
        {
            Point p = transform.GetWorldPosition();
            Point fp = _field.transform.GetWorldPosition();
            Vector2 fSize = _field.transform.Size;
            while (p.X <= fp.X + fSize.X && p.Y <= fp.Y + fSize.Y && transform.Position.X >= 0 && transform.Position.Y >= 0)
            {
                p = transform.GetWorldPosition();
                fp = _field.transform.GetWorldPosition();
                switch (_direction)
                {
                    case Direction.UP:
                        transform.Position.Y -= _moveSpeed;
                        break;
                    case Direction.DOWN:
                        transform.Position.Y += _moveSpeed;
                        break;
                    case Direction.LEFT:
                        transform.Position.X -= _moveSpeed;
                        break;
                    case Direction.RIGHT:
                        transform.Position.X += _moveSpeed;
                        break;
                }
                await Task.Delay(1);
            }
            _isMoving = false;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Cell targetCell = _field.GetCell(_currTargetPos);
            if (targetCell != null)
            {
                switch (_direction)
                {
                    case Direction.UP:
                        if (targetCell.transform.Position.Y <= transform.Position.Y)
                        {
                            _currTargetPos[0] -= 1;
                            targetCell.Destroy();
                        }
                        break;
                    case Direction.DOWN:
                        if (targetCell.transform.Position.Y >= transform.Position.Y)
                        {
                            _currTargetPos[0] += 1;
                            targetCell.Destroy();
                        }
                        break;
                    case Direction.LEFT:
                        if (targetCell.transform.Position.X <= transform.Position.X)
                        {
                            _currTargetPos[1] -= 1;
                            targetCell.Destroy();
                        }
                        break;
                    case Direction.RIGHT:
                        if (targetCell.transform.Position.X <= transform.Position.X)
                        {
                            _currTargetPos[1] += 1;
                            targetCell.Destroy();
                        }
                        break;
                }
            }
            if (!_isMoving)
            {
                _field.FillField();
                Destroy();
            }
        }
    }
}
