using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using ThreeRow.Engine;

namespace ThreeRow.ThreeRow
{
    class Field : GameObject
    {
        public static readonly int DIM = 8;
        public static readonly int CELL_OFFSET = 10;
        public Score Score;

        private static readonly int REVERT_DELAY = 150;

        private bool _fillTask = false;
        private List<List<Cell>> _cells;
        private Cell _moving1;
        private Cell _moving2;
        private static int _cellWidth, _cellHeight;
        private Cell _selectedCell;
        private List<Cell> _cellsToDestroy = new List<Cell>();

        public Field(Background bg)
        {
            transform.SetParent(bg.transform);
            transform.Position.Y = Background.HEIGHT - Background.WIDTH;
            transform.Size = new Vector2(Background.WIDTH, Background.WIDTH);
            CalcCellSize();
            _cells = new List<List<Cell>>();
            for (int i = 0; i < DIM; ++i)
            {
                _cells.Add(new List<Cell>());
                for (int j = 0; j < DIM; ++j)
                {
                    _cells[i].Add(null);
                }
            }
            FillField();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                Console.WriteLine();
                for (int i = 0; i < DIM; ++i)
                {
                    for (int j = 0; j < DIM; ++j)
                    {
                        Console.Write(_cells[i][j] == null ? "-" : _cells[i][j].GetCharType().ToString());
                        Console.Write(" ");
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();
            }
        }

        public override void Start(GameTime gameTime)
        {
            base.Start(gameTime);
            Score = GameObjects.GetInstance().GetObjectOfType(typeof(Score)) as Score;
        }

        private void CalcCellSize()
        {
            _cellWidth = _cellHeight = (int)((transform.Size.X - (CELL_OFFSET * (DIM + 1))) / DIM);
        }

        public async void FillField()
        {
            if (_fillTask == false)
            {
                _fillTask = true;
                while (!IsFilled())
                {
                    for (int i = 0; i < DIM; ++i)
                    {
                        if (_cells[0][i] == null)
                        {
                            Vector3 position = new Vector3(i * (CELL_OFFSET + _cellWidth) + CELL_OFFSET, CELL_OFFSET, 1);
                            _cells[0][i] = new Cell(this, new int[2] { 0, i }, position, _cellWidth, _cellHeight);
                        }
                    }
                    await DropCells();
                }
                _fillTask = false;
                Solve();
            }
        }

        private void DestroySolved()
        {
            bool isDestroyed = false;
            if (_cellsToDestroy != null)
            {
                foreach(Cell cell in _cellsToDestroy.ToArray())
                {
                    if (cell != null)
                    {
                        cell.Destroy();
                        isDestroyed = true;
                    }
                }
            }
            if (isDestroyed)
            {
                FillField();
            }
        }

        private bool Solve()
        {
            bool isSolved;
            _cellsToDestroy.Clear();
            CheckRows();
            CheckCols();
            isSolved = _cellsToDestroy.Count > 0;
            DestroySolved();
            return isSolved;
        }

        private void CheckRows()
        {
            for (int i = 0; i < DIM; ++i)
            {
                List<Cell> cellsSameType = new List<Cell>();
                cellsSameType.Add(_cells[i][0]);
                int type = _cells[i][0] != null ? _cells[i][0].GetCharType() : -1;
                for (int j = 1; j < DIM; ++j)
                {
                    if (_cells[i][j] != null)
                    {
                        if (type == _cells[i][j].GetCharType())
                        {
                            cellsSameType.Add(_cells[i][j]);
                        }
                        else
                        {
                            if (cellsSameType.Count > 2)
                            {
                                if (cellsSameType.Count == 4)
                                {
                                    AddBonusLineHor(cellsSameType, _moving1, _moving2);
                                }
                                if (cellsSameType.Count >= 5)
                                {
                                    AddBonusLineBomb(cellsSameType, _moving1, _moving2);
                                }
                                _cellsToDestroy.AddRange(cellsSameType);
                            }
                            cellsSameType = new List<Cell>();
                            cellsSameType.Add(_cells[i][j]);
                            type = _cells[i][j].GetCharType();
                        }
                    } else
                    {
                        cellsSameType = new List<Cell>();
                        type = -1;
                    }
                }
                if (cellsSameType.Count > 2)
                {
                    if (cellsSameType.Count == 4)
                    {
                        AddBonusLineHor(cellsSameType, _moving1, _moving2);
                    }
                    if (cellsSameType.Count >= 5)
                    {
                        AddBonusLineBomb(cellsSameType, _moving1, _moving2);
                    }
                    _cellsToDestroy.AddRange(cellsSameType);
                }
            }
        }

        private void CheckCols()
        {
            for (int i = 0; i < DIM; ++i)
            {
                List<Cell> cellsSameType = new List<Cell>();
                cellsSameType.Add(_cells[0][i]);
                int type = _cells[0][i] != null ? _cells[0][i].GetCharType() : -1;
                for (int j = 1; j < DIM; ++j)
                {
                    if (_cells[j][i] != null)
                    {
                        if (type == _cells[j][i].GetCharType())
                        {
                            cellsSameType.Add(_cells[j][i]);
                        }
                        else
                        {
                            if (cellsSameType.Count > 2)
                            {
                                if (cellsSameType.Count == 4)
                                {
                                    AddBonusLineVert(cellsSameType, _moving1, _moving2);
                                }
                                if (cellsSameType.Count >= 5)
                                {
                                    AddBonusLineBomb(cellsSameType, _moving1, _moving2);
                                }
                                _cellsToDestroy.AddRange(cellsSameType);
                            }
                            cellsSameType = new List<Cell>();
                            cellsSameType.Add(_cells[j][i]);
                            type = _cells[j][i].GetCharType();
                        }
                    }
                    else
                    {
                        cellsSameType = new List<Cell>();
                        type = -1;
                    }
                }
                if (cellsSameType.Count > 2)
                {
                    if (cellsSameType.Count == 4)
                    {
                        AddBonusLineVert(cellsSameType, _moving1, _moving2);
                    }
                    if (cellsSameType.Count >= 5)
                    {
                        AddBonusLineBomb(cellsSameType, _moving1, _moving2);
                    }
                    _cellsToDestroy.AddRange(cellsSameType);
                }
            }
        }

        private void AddBonusLineHor(List<Cell> cellList, Cell lft, Cell rgt)
        {
            foreach (Cell c in cellList)
            {
                if (c == lft || c == rgt)
                {
                    c.leaveBonus = Cell.BonusTypes.LHOR;
                }
            }
        }

        private void AddBonusLineVert(List<Cell> cellList, Cell lft, Cell rgt)
        {
            foreach (Cell c in cellList)
            {
                if (c == lft || c == rgt)
                {
                    c.leaveBonus = Cell.BonusTypes.LVERT;
                }
            }
        }

        private void AddBonusLineBomb(List<Cell> cellList, Cell lft, Cell rgt)
        {
            foreach (Cell c in cellList)
            {
                if (c == lft || c == rgt)
                {
                    c.leaveBonus = Cell.BonusTypes.BOMB;
                }
            }
        }
        
        private async Task DropCells()
        {
            List<Task> cellsToDrop = new List<Task>();
            for (int i = 0; i < DIM; ++i)
            {
                for (int j = 0; j < DIM - 1; ++j)
                {
                    if (_cells[j + 1][i] == null && _cells[j][i] != null)
                    {
                        _cells[j + 1][i] = _cells[j][i];
                        _cells[j][i] = null;
                        cellsToDrop.Add(_cells[j + 1][i].SetPos(new int[2] { j + 1, i }));
                    }
                }
            }
            await Task.WhenAll(cellsToDrop);
        }

        public bool IsFilled()
        {
            for (int i = 0; i < DIM; ++i)
            {
                for (int j = 0; j < DIM; ++j)
                {
                    if (_cells[i][j] == null)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public async Task SelectCell(Cell cell)
        {
            try
            {
                if (_selectedCell == null)
                {
                    _selectedCell = cell;
                    _selectedCell.Select();
                }
                else
                {
                    _selectedCell.Deselect();
                    if (_selectedCell == cell)
                    {
                        _selectedCell = null;
                    }
                    else
                    {
                        int[] pos1 = cell.GetPos();
                        int[] pos2 = _selectedCell.GetPos();
                        Cell selCell = _selectedCell;
                        _selectedCell.Deselect();
                        _selectedCell = null;
                        if (IsPossibleMove(pos1, pos2))
                        {
                            await SwapCells(cell, selCell);
                            _moving1 = cell;
                            _moving2 = selCell;
                            bool isSolved = Solve();
                            _moving1 = null;
                            _moving2 = null;
                            if (!isSolved)
                            {
                                await Task.Delay(REVERT_DELAY);
                                await SwapCells(cell, selCell);
                            }
                        }
                    }
                }
            } catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }

        private async Task SwapCells(Cell cell1, Cell cell2)
        {
            int[] pos1 = cell1.GetPos();
            int[] pos2 = cell2.GetPos();
            await Task.WhenAll(cell1.SetPos(pos2), cell2.SetPos(pos1));
            Cell tmpCell = _cells[pos1[0]][pos1[1]];
            _cells[pos1[0]][pos1[1]] = _cells[pos2[0]][pos2[1]];
            _cells[pos2[0]][pos2[1]] = tmpCell;
        }

        public bool IsPossibleMove(int[] pos1, int[] pos2)
        {
            int x = pos1[0] - pos2[0];
            int y = pos1[1] - pos2[1];
            return x * x + y * y == 1;
        }

        public void DestroyCell(Cell cell)
        {
            int[] pos = cell.GetPos();
            _cells[pos[0]][pos[1]] = null;
        }

        public Cell GetCell(int[] pos)
        {
            if (pos[0] >= 0 && pos[1] >= 0 && pos[0] < DIM && pos[1] < DIM)
            {
                return _cells[pos[0]][pos[1]];
            }
            return null;
        }

    }
}
