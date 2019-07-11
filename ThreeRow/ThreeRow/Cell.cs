using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Threading.Tasks;
using ThreeRow.Engine;

namespace ThreeRow.ThreeRow
{
    class Cell : GameObject
    {
        public Field Field;

        public bool HasBonus => _bonus != null;
        public BonusTypes leaveBonus = BonusTypes.NONE;
        public enum BonusTypes
        {
            LHOR,
            LVERT,
            BOMB,
            NONE
        }

        private static readonly int SCORE_VALUE = 10;
        private static readonly string[] TextureTypes = new string[] {
            "characters_0001" ,
            "characters_0002" ,
            "characters_0003" ,
            "characters_0004" ,
            "characters_0005"
        };
        private static Random rand = new Random();

        private int _width = 50;
        private int _height = 50;
        private IBonus _bonus;
        private Texture2D _texture;
        private int _moveSpeed = 10;
        private bool _isMoving = false;
        private int[] _fieldPos = new int[2] { 0, 0 };
        private int _type;
        private Vector2 _moveFromPos;
        private Vector2 _moveToPos;
        private Vector2 _moveDir;
        private float _moveDist;
        private Vector2 _deselectedPos;
        private Vector2 _deselectedSize;

        public Cell(Field field, int[] fieldPos, Vector3 screenPos)
        {
            Field = field;
            transform.SetParent(field.transform);
            _type = rand.Next(0, TextureTypes.Length - 1);
            _texture = TextureManager.GetTexture(TextureTypes[_type]);
            transform.Size = new Vector2(_width, _height);
            Random r = new Random();
            transform.Position = screenPos;
            GetComponent<Sprite>().SetTexture(_texture);
            _fieldPos = fieldPos;
            SetLayer(1);
        }

        public Cell(Field field, int[] fieldPos, Vector3 screenPos, int width, int height) : this(field, fieldPos, screenPos)
        {
            _width = width;
            _height = height;
            transform.Size = new Vector2(_width, _height);
        }

        public async Task MoveTo(Vector2 pos)
        {
            _isMoving = true;
            _moveToPos = pos;
            _moveFromPos = new Vector2(transform.Position.X, transform.Position.Y);
            _moveDir = _moveToPos - _moveFromPos;
            _moveDir.Normalize();
            _moveDist = Vector2.DistanceSquared(_moveToPos, _moveFromPos);
            pos = new Vector2(transform.Position.X, transform.Position.Y);
            do
            {
                pos = new Vector2(transform.Position.X, transform.Position.Y);
                transform.Position.X += _moveDir.X * _moveSpeed;
                transform.Position.Y += _moveDir.Y * _moveSpeed;
                await Task.Delay(1);
            }
            while (Vector2.DistanceSquared(pos, _moveFromPos) < _moveDist);
            transform.Position.X = _moveToPos.X;
            transform.Position.Y = _moveToPos.Y;
            _isMoving = false;
        }

        public int[] GetPos()
        {
            return _fieldPos;
        }

        public async Task SetPos(int[] fieldPos)
        {
            _fieldPos = fieldPos;
            Vector2 screenPos = new Vector2(
                fieldPos[1] * (Field.CELL_OFFSET + _width) + Field.CELL_OFFSET,
                fieldPos[0] * (Field.CELL_OFFSET + _width) + Field.CELL_OFFSET);
            await MoveTo(screenPos);
        }

        public async override void OnClick()
        {
            base.OnClick();
            if (!_isMoving)
            {
                await Field.SelectCell(this);
            }
        }

        public void Select()
        {
            Vector2 newSize = transform.Size * 1.5f;
            Vector2 diff = newSize - transform.Size;
            Vector2 pos = new Vector2(transform.Position.X, transform.Position.Y);
            _deselectedPos = pos;
            _deselectedSize = transform.Size;
            transform.Size = newSize;
            transform.Position.X = pos.X - diff.X / 2;
            transform.Position.Y = pos.Y - diff.Y / 2;
        }

        public void Deselect()
        {
            transform.Position.X = _deselectedPos.X;
            transform.Position.Y = _deselectedPos.Y;
            transform.Size = _deselectedSize;
        }

        override public void Destroy()
        {
            if (_bonus != null)
            {
                IBonus t = _bonus;
                _bonus = null;
                t.Activate();
            }
            if (leaveBonus != BonusTypes.NONE)
            {
                Field.Score.AddScore(SCORE_VALUE);
                switch (leaveBonus)
                {
                    case BonusTypes.LHOR: SetBonus(new LineBonus(LineBonus.LineType.HORIZONTAL)); break;
                    case BonusTypes.LVERT: SetBonus(new LineBonus(LineBonus.LineType.VERTICAL)); break;
                    case BonusTypes.BOMB: SetBonus(new BombBonus(Field, this)); break;
                }
                leaveBonus = BonusTypes.NONE;
            }
            else
            {
                Field.Score.AddScore(SCORE_VALUE);
                Field.DestroyCell(this);
                base.Destroy();
            }
            
        }

        public int GetCharType()
        {
            return _type;
        }

        public void SetBonus(IBonus newBonus)
        {
            if (_bonus == null)
            {
                _bonus = newBonus;
                if (newBonus is GameObject)
                {
                    transform.AddChild(((GameObject)newBonus).transform);
                }
            } else
            {
                if (newBonus.GetType() == typeof(BombBonus))
                {
                    _bonus = newBonus;
                    if (newBonus is GameObject)
                    {
                        transform.AddChild(((GameObject)newBonus).transform);
                    }
                }
            }
        }

    }
}
