using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Tetris
{
    public class HeapFigures
    {
        public HeapFigures( Map mapArg,
                            [Inject(Id = "sizeBoundsBlock")] Vector3 sizeBlockArg)
        {
            _map = mapArg;
            _boundsTopBlocks = new Dictionary<int, Bounds>();

            _minCell = new Vector3Int(int.MaxValue, int.MaxValue, 0);
            _maxCell = new Vector3Int(int.MinValue, int.MinValue, 0);

            _halfSizeBlockXY = (sizeBlockArg / 2).WithZ(0);
        }


        public float BottomByY => _map.BottomByY;
        public float TopByY => _bounds.max.y;
        public Bounds Bounds => _bounds;
        public IReadOnlyDictionary<int, Bounds> BoundsOfTop => _boundsTopBlocks;
        public int Ranges => 0;


        protected Dictionary<int, Bounds> _boundsTopBlocks;
        protected Bounds _bounds;
        protected Map _map;
        protected Vector3Int _minCell;
        protected Vector3Int _maxCell;
        protected Vector3 _halfSizeBlockXY;


        public int Add(FigureModel figureArg)
        {
            foreach (var block in figureArg.BoundsBlocks)
            {
                int blockX = Mathf.RoundToInt(block.center.x);
                int blockY = Mathf.RoundToInt(block.center.y);
                Bounds boundsTop;

                if (_boundsTopBlocks.TryGetValue(blockX, out boundsTop))
                { 
                    if (block.center.y > boundsTop.center.y)
                        _boundsTopBlocks[blockX] = block;
                }
                else
                    _boundsTopBlocks[blockX] = block;

                if (blockX < _minCell.x)
                    _minCell.x = blockX;
                else if (blockX > _maxCell.x)
                    _maxCell.x = blockX;

                if (blockY < _minCell.y)
                    _minCell.y = blockY;
                else if (blockY > _maxCell.y)
                    _maxCell.y = blockY;
            }

            _bounds.SetMinMax(_minCell - _halfSizeBlockXY, _maxCell + _halfSizeBlockXY);

            return 0;
        }
    }
}

