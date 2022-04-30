using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tetris.Model
{
    public sealed class ColliderFigure
    {
        public ColliderFigure(  Bounds boundsArg, 
                                Bounds[] blocksArg, 
                                Vector2Int[] blocksLocalPosArg, 
                                int idxPivotBlockArg,
                                int[] idxsExtremeBlocksArg)
        {
            Bounds = boundsArg;
            Rotate = Quaternion.Euler(0f, 0f, 0f);

            _blocks = blocksArg;
            _blockLocalPos = blocksLocalPosArg;
            _idxPivot = idxPivotBlockArg;
            _idxsExtremeBlocks = idxsExtremeBlocksArg;
        }


        public Bounds Bounds { get; private set; }
        public Vector3 Center => Bounds.center;
        public Vector3 Pivot => _blocks[_idxPivot].center;
        public Quaternion Rotate { get; private set; }
        public IReadOnlyList<Bounds> Blocks => _blocks;
        public IReadOnlyList<Vector2Int> BlocksLocalPos => _blockLocalPos;
        public IReadOnlyList<int> IdxsExtremeBlocks => _idxsExtremeBlocks;
        public int RightColumn => Bounds.GetMaxCell().x;
        public int LeftColumn => Bounds.GetMinCell().x;


        private readonly Bounds[] _blocks;
        private readonly Vector2Int[] _blockLocalPos;
        private readonly int _idxPivot;
        private readonly int[] _idxsExtremeBlocks;


        public void Transform(Bounds boundsArg)
        {
            Bounds = boundsArg;
        }
        public void Transform(Bounds boundsArg, IReadOnlyList<Bounds> blocksArg)
        {
            Bounds = boundsArg;
            for (int i = 0; i < _blocks.Length; i++)
                _blocks[i] = blocksArg[i];
        }
        public void Transform(Bounds boundsArg, IReadOnlyList<Bounds> blocksArg, Quaternion newRotateArg)
        {
            Rotate = newRotateArg;
            Transform(boundsArg, blocksArg);
        }
    }
}

