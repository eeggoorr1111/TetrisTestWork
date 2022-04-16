using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tetris
{
    public class BoundsFigure
    {
        public BoundsFigure(Bounds boundsArg, List<Bounds> blocksArg)
        {
            _blocks = blocksArg;
            _figure = boundsArg;

            Dictionary<int, int> idxsBlocks = new Dictionary<int, int>();

            for (int i = 0; i < _blocks.Count; i++)
            {
                int xPos = Mathf.RoundToInt(_blocks[i].center.x);
                if (idxsBlocks.ContainsKey(xPos))
                {
                    int idxLowerBlock = idxsBlocks[xPos];
                    if (_blocks[i].center.y < _blocks[idxLowerBlock].center.y)
                        idxsBlocks[xPos] = i;
                }
                else
                    idxsBlocks[xPos] = i;
            }

            _bottomBlocksIdx = new List<int>();
            foreach (var pair in idxsBlocks)
                _bottomBlocksIdx.Add(pair.Value);

            _bounds = new List<Bounds>();
        }


        public Vector3 Center => _figure.center;
        public Bounds Figure => _figure;
        public IReadOnlyList<Bounds> Blocks => _blocks;
        public IReadOnlyList<Bounds> BlocksBottom
        { 
            get
            {
                _bounds.Clear();
                foreach (var idx in _bottomBlocksIdx)
                    _bounds.Add(_blocks[idx]);

                return _bounds;
            }
        }


        protected List<Bounds> _blocks;
        protected List<Bounds> _bounds;
        protected List<int> _bottomBlocksIdx;
        protected Bounds _figure;


        public void Move(Vector3 deltaArg)
        {
            _figure = _figure.WithDeltaPos(deltaArg);
            for (int i = 0; i < _blocks.Count; i++)
                _blocks[i] = _blocks[i].WithDeltaPos(deltaArg);
        }
        public void MoveTo(Vector3 pointArg)
        {
            Vector3 delta = pointArg - _figure.center;
            Move(delta);
        }
    }
}

