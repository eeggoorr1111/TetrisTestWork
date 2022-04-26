using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tetris
{
    public sealed class ColliderFigure
    {
        public ColliderFigure(  Bounds boundsArg, 
                                Bounds[] blocksArg, 
                                Vector2Int[] blocksLocalPosArg, 
                                int idxPivotBlockArg)
        {
            Bounds = boundsArg;
            Rotate = Quaternion.Euler(0f, 0f, 0f);

            _blocks = blocksArg;
            _blockLocalPos = blocksLocalPosArg;
            _idxPivotBlock = idxPivotBlockArg;
        }


        public Bounds Bounds { get; private set; }
        public Vector3 Center => Bounds.center;
        public Vector3 Pivot => _blocks[_idxPivotBlock].center;
        public Quaternion Rotate { get; private set; }
        public IReadOnlyList<Bounds> Blocks => _blocks;
        public IReadOnlyList<Vector2Int> BlocksLocalPos => _blockLocalPos;
        public int RightX => Bounds.GetMaxCell().x;
        public int LeftX => Bounds.GetMinCell().x;



        private readonly Bounds[] _blocks;
        private readonly Vector2Int[] _blockLocalPos;
        private readonly int _idxPivotBlock;


        public void Tranasform(Bounds newBoundsArg)
        {
            Bounds = newBoundsArg;
        }
        public void Tranasform(Bounds newBoundsArg, IReadOnlyList<Bounds> blocksArg)
        {
            Bounds = newBoundsArg;
            for (int i = 0; i < _blocks.Length; i++)
                _blocks[i] = blocksArg[i];
        }
        public void Tranasform(Bounds newBoundsArg, IReadOnlyList<Bounds> blocksArg, Quaternion newRotateArg)
        {
            Rotate = newRotateArg;
            Tranasform(newBoundsArg, blocksArg);
        }
    }
}

