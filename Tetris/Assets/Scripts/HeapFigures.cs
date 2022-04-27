using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Tetris
{
    public class HeapFigures
    {
        public HeapFigures(MapData mapArg)
        {
            _map = mapArg;

            _minCell = new Vector3Int(int.MaxValue, int.MaxValue, 0);
            _maxCell = new Vector3Int(int.MinValue, int.MinValue, 0);

            _blocks = new Dictionary<Vector2Int, Bounds>();
            _blocksInRange = new Dictionary<int, int>();
            _deleteRanges = new List<int>();
            _lastRange = -1;
        }


        public float BottomByY => _map.BottomByY;
        public float TopByY => _bounds.max.y;
        public Bounds Bounds => _bounds;
        public int Ranges => 0;


        protected Dictionary<Vector2Int, Bounds> _blocks;
        protected Bounds _bounds;
        protected MapData _map;
        protected Vector3Int _minCell;
        protected Vector3Int _maxCell;
        protected Dictionary<int, int> _blocksInRange;
        protected List<int> _deleteRanges;
        protected int _lastRange;


        public IReadOnlyList<int> Add(FigureModel figureArg)
        {
            _deleteRanges.Clear();
            foreach (var block in figureArg.Blocks)
            {
                int blockX = Mathf.RoundToInt(block.center.x);
                int blockY = Mathf.RoundToInt(block.center.y);
                Vector2Int pos = new Vector2Int(blockX, blockY);

                if (!_blocksInRange.ContainsKey(blockY))
                    _blocksInRange[blockY] = 0;

                _blocksInRange[blockY]++;
                _blocks[pos] = block;

                if (blockY > _lastRange)
                    _lastRange = blockY;

                if (_blocksInRange[pos.y] == _map.CountCells.x)
                    _deleteRanges.Add(blockY);
            }

            if (!_deleteRanges.IsEmpty())
            {
                _deleteRanges.Sort();
                DeleteRanges(_deleteRanges);
            }
                
            UpdateBounds();

            return _deleteRanges;
        }
        public bool GetUpperBlock(int byXArg, float bellowYArg, out Bounds upperBlock)
        {
            upperBlock = new Bounds();
            int checkFromY = Mathf.FloorToInt(bellowYArg);

            for (int y = checkFromY; y >= 0; y--)
            {
                Vector2Int check = new Vector2Int(byXArg, y);
                if (_blocks.TryGetValue(check, out upperBlock))
                    return true;
            }

            return false;
        }
        public bool Intersect(Vector3 blockPosArg)
        {
            int x = Mathf.RoundToInt(blockPosArg.x);
            int topY = Mathf.CeilToInt(blockPosArg.y);
            int bottomY = Mathf.FloorToInt(blockPosArg.y);

            if (_blocks.ContainsKey(new Vector2Int(x, topY)) ||
                _blocks.ContainsKey(new Vector2Int(x, bottomY)))
                return true;

            return false;
        }
        public bool Contains(Vector2Int cellArg)
        {
            return _blocks.ContainsKey(cellArg);
        }


        protected void DeleteRanges(List<int> rangesArg)
        {
            int putDown = 0;
            for (int y = rangesArg[0]; y <= _lastRange; y++)
            {
                bool remove = rangesArg.Contains(y);
                if (remove)
                    putDown++;

                _blocksInRange[y] = 0;
                for (int x = 0; x < _map.CountCells.x; x++)
                {
                    Vector2Int cell = new Vector2Int(x, y);

                    if (remove)
                        _blocks.Remove(cell);
                    else if (_blocks.ContainsKey(cell))
                    {
                        _blocksInRange[y - putDown]++;
                        PutDownBlock(cell, putDown);
                    }
                }
            }

            _lastRange -= rangesArg.Count;
        }
        protected void UpdateBounds()
        {
            _minCell = new Vector3Int(int.MaxValue, int.MaxValue, 0);
            _maxCell = new Vector3Int(int.MinValue, int.MinValue, 0);

            if (_blocks.IsEmpty())
            {
                _bounds = new Bounds();
                return;
            }

            foreach (var pair in _blocks)
            {
                int blockX = pair.Key.x;
                int blockY = pair.Key.y;

                if (blockX < _minCell.x)
                    _minCell.x = blockX;
                else if (blockX > _maxCell.x)
                    _maxCell.x = blockX;

                if (blockY < _minCell.y)
                    _minCell.y = blockY;
                else if (blockY > _maxCell.y)
                    _maxCell.y = blockY;
            }

            _bounds.SetMinMax(_minCell - _map.HalfSizeBlockXY, _maxCell + _map.HalfSizeBlockXY);
        }
        protected void PutDownBlock(Vector2Int cellArg, int puDownArg)
        {
            Bounds block = _blocks[cellArg];
            Vector2Int newCell = new Vector2Int(cellArg.x, cellArg.y - puDownArg);
            
            block.center = new Vector3(newCell.x, newCell.y, block.center.z);

            _blocks.Remove(cellArg);
            _blocks[newCell] = block;
        }
    }
}

