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

            _blocksTemp = new List<Bounds>();
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
        private List<Bounds> _blocksTemp;


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
            Bounds bounds;
            GetDataAfterRotate(rotateArg, _blocksTemp, out bounds);

            Rotate = rotateArg;
            Bounds = bounds;

            for (int i = 0; i < _blocks.Length; i++)
                _blocks[i] = _blocksTemp[i];
        }
        public void GetDataAfterRotate(Quaternion rotateArg, List<Bounds> blocks, out Bounds bounds)
        {
            Bounds pivot = _blocks[_idxPivotBlock];

            blocks.Clear();
            for (int i = 0; i < _blocks.Length; i++)
            {
                Vector3 localPos = new Vector3(_blockLocalPos[i].x, _blockLocalPos[i].y, 0);
                Vector3 newLocalPos = Matrix4x4.Rotate(rotateArg).MultiplyPoint(localPos);

                blocks.Add(new Bounds(newLocalPos + pivot.center, _blocks[i].size));
            }

            bounds = Helpers.GetBounds(blocks);
        }
    }
}

