using System.Collections.Generic;
using UnityEngine;

namespace Tetris
{
    public sealed class CheckCollisionHeap
    {
        public CheckCollisionHeap(HeapFigures heapArg, MapData mapArg)
        {
            _boundsRotate = new HashSet<Vector2Int>();
            _areaRotate = new HashSet<Vector2Int>();
            _pointsOfCell = new Vector2[3];
            _square = new Vector2[4];
            _heapFigures = heapArg;
            _map = mapArg;
        }


        private readonly HeapFigures _heapFigures;
        private readonly HashSet<Vector2Int> _boundsRotate;
        private readonly HashSet<Vector2Int> _areaRotate;
        private readonly Vector2[] _pointsOfCell;
        private readonly Vector2[] _square;
        private readonly MapData _map;


        public bool CheckRotate(ColliderFigure colliderArg, IReadOnlyList<Bounds> blocksAfterRotateArg)
        {
            GetAreaRotate(colliderArg, colliderArg.Blocks, blocksAfterRotateArg, _areaRotate);
            return CheckArea(_areaRotate);
        }
        public void GetAreaRotate(  ColliderFigure colliderArg,
                                    IReadOnlyList<Bounds> blocksBeforeRotateArg,
                                    IReadOnlyList<Bounds> blocksAfterRotateArg,
                                    HashSet<Vector2Int> areaRotate)
        {
            Vector3 pivot = colliderArg.Pivot;

            areaRotate.Clear();
            foreach (var idx in colliderArg.IdxsExtremeBlocks)
            {
                Bounds blockBefore = blocksBeforeRotateArg[idx];
                Bounds blockAfter = blocksAfterRotateArg[idx];
                Parallelogram figure = NewParallelogram(pivot, blockBefore.center, blockAfter.center);

                Helpers.GetBounds(GetAsArray(figure)).GetCells(_boundsRotate);

                foreach (var cell in _boundsRotate)
                    if (IsInsideCircleCell(cell, figure))
                        areaRotate.Add(cell);
            }
        }
        public bool CheckArea(IReadOnlyCollection<Vector2Int> areaArg)
        {
            foreach (var cell in areaArg)
                if (_heapFigures.Contains(cell))
                    return false;

            return true;
        }
        public bool CheckMoveToSide(Bounds figureArg, IReadOnlyList<Bounds> blocksArg)
        {
            if (_heapFigures.Bounds.Intersects(figureArg))
                return CheckMoveToSide(blocksArg);

            return true;
        }
        public bool CheckMoveToSide(IReadOnlyList<Bounds> blocksArg)
        {
            foreach (var figureBlock in blocksArg)
                if (_heapFigures.Intersect(figureBlock.center))
                    return false;

            return true;
        }


        private Parallelogram NewParallelogram(Vector3 pivotArg, Vector3 beforeRotateArg, Vector3 afterRotateArg)
        {
            Vector2 pivot = new Vector2(pivotArg.x, pivotArg.y);
            Vector2 beforeRotate = new Vector2(beforeRotateArg.x, beforeRotateArg.y);
            Vector2 afterRotate = new Vector2(afterRotateArg.x, afterRotateArg.y);

            Vector2 center = (beforeRotate + afterRotate) / 2;
            Vector2 pivotMirror = center - (pivot - center);

            pivot += (pivot - center).normalized * _map.HalfSizeBlock2D;
            afterRotate += (afterRotate - center).normalized * _map.HalfSizeBlock2D;
            pivotMirror += (pivotMirror - center).normalized * _map.HalfSizeBlock2D;
            beforeRotate += (beforeRotate - center).normalized * _map.HalfSizeBlock2D;

            return new Parallelogram(pivot, afterRotate, pivotMirror, beforeRotate);
        }
        private bool IsInsideCircleCell(Vector2Int cellArg, Parallelogram figureInscribedInCircleArg)
        {
            float circleRadius = figureInscribedInCircleArg.LargeDiagonal / 2;
            Vector2 circleCenter = figureInscribedInCircleArg.Center;

            Vector2 halfSizeBlock = _map.HalfSizeBlock2D;
            Vector2 centerOfCell = cellArg;
            Vector2 topRightOfCell = cellArg + halfSizeBlock;
            Vector2 topLeftOfCell = cellArg + new Vector2(-halfSizeBlock.x, halfSizeBlock.y);

            _pointsOfCell[0] = centerOfCell;
            _pointsOfCell[1] = topRightOfCell;
            _pointsOfCell[2] = topLeftOfCell;

            foreach (var point in _pointsOfCell)
            {
                float distanceToCenter = (circleCenter - point).magnitude;
                if (distanceToCenter < circleRadius)
                    return true;
            }
            
            return false;
        }
        private IReadOnlyList<Vector2> GetAsArray(Parallelogram figureArg)
        {
            _square[0] = figureArg.Point1;
            _square[1] = figureArg.Point2;
            _square[2] = figureArg.Point3;
            _square[3] = figureArg.Point4;

            return _square;
        }


        private struct Parallelogram
        {
            public Parallelogram(Vector2 point1Arg, Vector2 point2Arg, Vector2 point3Arg, Vector2 point4Arg)
            {
                Point1 = point1Arg;
                Point2 = point2Arg;
                Point3 = point3Arg;
                Point4 = point4Arg;
                Center = (Point1 + Point2 + Point3 + Point4) / 4;
            }

            public Vector2 Point1;
            public Vector2 Point2;
            public Vector2 Point3;
            public Vector2 Point4;
            public Vector2 Center;
            public float Diagonal1 => (Point1 - Point3).magnitude;
            public float Diagonal2 => (Point2 - Point4).magnitude;
            public float LargeDiagonal => Mathf.Max(Diagonal1, Diagonal2);
        }
    }
}

