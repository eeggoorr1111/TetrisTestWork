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
                                int[] bottomBlocksIdxArg, 
                                int idxPivotBlockArg)
        {
            Bounds = boundsArg;
            Rotate = Quaternion.Euler(0f, 0f, 0f);

            _blocks = blocksArg;
            _blockLocalPos = blocksLocalPosArg;
            _bottomBlocksIdx = bottomBlocksIdxArg;
            _idxPivotBlock = idxPivotBlockArg;
        }


        public Bounds Bounds { get; private set; }
        public Vector3 Center => Bounds.center;
        public Vector3 Pivot => _blocks[_idxPivotBlock].center;
        public Quaternion Rotate { get; private set; }
        public IReadOnlyList<Bounds> Blocks => _blocks;


        private readonly Bounds[] _blocks;
        private readonly Vector2Int[] _blockLocalPos;
        private readonly int[] _bottomBlocksIdx;
        private readonly int _idxPivotBlock;


        public void ToMove(Vector3 deltaArg)
        {
            Bounds = Bounds.WithDeltaPos(deltaArg);
            for (int i = 0; i < _blocks.Length; i++)
                _blocks[i] = _blocks[i].WithDeltaPos(deltaArg);
        }
        public void ToMoveTo(Vector3 pointArg)
        {
            Vector3 delta = pointArg - Bounds.center;
            ToMove(delta);
        }
        public void ToRotate(Quaternion rotateArg)
        {
            ToRotate(rotateArg, _idxPivotBlock);
        }
        public bool GetBottomBlocks(List<Bounds> blocks)
        {
            blocks.Clear();

            foreach (var idx in _bottomBlocksIdx)
                blocks.Add(_blocks[idx]);

            return blocks.Count > 0;
        }


        private void ToRotate(Quaternion rotateArg, int idxBlockPivotArg)
        {
            Bounds pivot = _blocks[idxBlockPivotArg];

            Rotate = rotateArg;
            for (int i = 0; i < _blocks.Length; i++)
            {
                Vector3 localPos = new Vector3(_blockLocalPos[i].x, _blockLocalPos[i].y, 0);
                Vector3 newLocalPos = Matrix4x4.Rotate(rotateArg).MultiplyPoint(localPos);

                _blocks[i].center = newLocalPos + pivot.center;
            }

            Bounds = Helpers.GetBounds(_blocks);
        }
    }
}

