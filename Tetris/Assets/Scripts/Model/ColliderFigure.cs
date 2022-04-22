using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tetris
{
    public sealed class ColliderFigure
    {
        public ColliderFigure(Bounds boundsArg, Bounds[] blocksArg, int[] bottomBlocksIdxArg)
        {
            Bounds = boundsArg;

            _blocks = blocksArg;
            _bottomBlocksIdx = bottomBlocksIdxArg;
        }


        public Bounds Bounds { get; private set; }
        public Vector3 Center => Bounds.center;
        public Quaternion Rotate { get; private set; }
        public IReadOnlyList<Bounds> Blocks => _blocks;


        private readonly Bounds[] _blocks;
        private readonly int[] _bottomBlocksIdx;


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
        public void ToRotate(Quaternion rotateArg, int idxBlockPivotArg)
        {
            Bounds pivot = _blocks[idxBlockPivotArg];

            Rotate = rotateArg;
            for (int i = 0; i < _blocks.Length; i++)
            {
                Bounds block = _blocks[i];
                Vector3 localPos = block.center - pivot.center;
                Vector3 newLocalPos = Matrix4x4.Rotate(rotateArg).MultiplyPoint(localPos);

                block.center = newLocalPos + pivot.center;
            }
        }
        public bool GetBottomBlocks(List<Bounds> blocks)
        {
            blocks.Clear();

            foreach (var idx in _bottomBlocksIdx)
                blocks.Add(_blocks[idx]);

            return blocks.Count > 0;
        }
    }
}

