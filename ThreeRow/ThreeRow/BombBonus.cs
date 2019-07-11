using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreeRow.Engine;

namespace ThreeRow.ThreeRow
{
    class BombBonus : GameObject, IBonus
    {
        public static readonly int WIDTH = 24;
        public static readonly int HEIGHT = 24;

        private Texture2D _texture;

        private static readonly int MS_DELAY = 250;
        private static readonly string TEXTURE = "bomb";

        private Cell _cell;
        private int[] _cellPos = new int[2];
        private Field _field;

        public BombBonus(Field f, Cell cell)
        {
            _cell = cell;
            _field = f;
            _texture = TextureManager.GetTexture(TEXTURE);
            transform.Size = new Vector2(WIDTH, HEIGHT);
            GetComponent<Sprite>().SetTexture(_texture);
            SetLayer(4);
        }

        public async void Activate()
        {
            _cell.GetPos().CopyTo(_cellPos, 0);
            await Task.Delay(MS_DELAY);
            for (int i = _cellPos[1] - 1; i < _cellPos[1] + 2; ++i)
            {
                for (int j = _cellPos[0] - 1; j < _cellPos[0] + 2; ++j)
                {
                    Cell c = _field.GetCell(new int[] { j, i });
                    if (c != null)
                    {
                        c.Destroy();
                    }
                }
            }
            await Task.Delay(100);
            _field.FillField();
        }
    }
}
