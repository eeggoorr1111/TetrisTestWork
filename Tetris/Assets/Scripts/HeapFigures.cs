using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Tetris
{
    public class HeapFigures
    {
        public HeapFigures(Map mapArg)
        {
            _map = mapArg;

            _minCell = new Vector3Int(int.MaxValue, int.MaxValue, 0);
            _maxCell = new Vector3Int(int.MinValue, int.MinValue, 0);

            _blocks = new Dictionary<Vector2Int, Bounds>();
        }


        public float BottomByY => _map.BottomByY;
        public float TopByY => _bounds.max.y;
        public Bounds Bounds => _bounds;
        public int Ranges => 0;


        protected Dictionary<Vector2Int, Bounds> _blocks;
        protected Bounds _bounds;
        protected Map _map;
        protected Vector3Int _minCell;
        protected Vector3Int _maxCell;


        public int Add(FigureModel figureArg)
        {
            foreach (var block in figureArg.BoundsBlocks)
            {
                int blockX = Mathf.RoundToInt(block.center.x);
                int blockY = Mathf.RoundToInt(block.center.y);
                Vector2Int pos = new Vector2Int(blockX, blockY);

                if (blockX < _minCell.x)
                    _minCell.x = blockX;
                else if (blockX > _maxCell.x)
                    _maxCell.x = blockX;

                if (blockY < _minCell.y)
                    _minCell.y = blockY;
                else if (blockY > _maxCell.y)
                    _maxCell.y = blockY;

                _blocks[pos] = block;
            }

            _bounds.SetMinMax(_minCell - _map.HalfSizeBlockXY, _maxCell + _map.HalfSizeBlockXY);

            return 0;
        }
        /// <summary>
        /// For example, if player build a structure like to symbol "C", we must give the opportunity to put a new figure on the lower arc "C"
        /// </summary>
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
        public bool Intersect(Bounds blockArg)
        {
            int x = Mathf.RoundToInt(blockArg.center.x);
            int topY = Mathf.CeilToInt(blockArg.center.y);
            int bottomY = Mathf.FloorToInt(blockArg.center.y);

            if (_blocks.ContainsKey(new Vector2Int(x, topY)) ||
                _blocks.ContainsKey(new Vector2Int(x, bottomY)))
                return true;

            return false;
        }
    }
}

