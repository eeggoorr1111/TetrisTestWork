using System.Collections.Generic;
using UnityEngine;

namespace Tetris
{
    public sealed class CheckCollisionHeap
    {
        public CheckCollisionHeap(HeapFigures heapArg)
        {
            _cellsAreaRotate = new HashSet<Vector2Int>();
            _cellsTemp = new HashSet<Vector2Int>();
            _heapFigures = heapArg;
        }


        private HashSet<Vector2Int> _cellsAreaRotate;
        private HashSet<Vector2Int> _cellsTemp;
        private HeapFigures _heapFigures;


        public bool CheckRotate(Bounds beforeRotateArg, Bounds afterRotateArg, IReadOnlyList<Bounds> blocksAfterRotateArg, Vector3 deltaPosArg)
        {
            Bounds areaRotate = GetAreaRotate(beforeRotateArg, afterRotateArg);
            if (!_heapFigures.Bounds.Intersects(areaRotate))
                return true;

            areaRotate.GetCells(_cellsAreaRotate);

            foreach (var block in blocksAfterRotateArg)
            {
                Bounds falledBlock = block.WithDeltaPos(deltaPosArg);

                _cellsAreaRotate.Remove(falledBlock.GetCellCenter());
                if (_heapFigures.Intersect(falledBlock.center))
                    return false;
            }

            beforeRotateArg.GetCells(_cellsTemp);
            foreach (var cell in _cellsTemp)
                if (_cellsAreaRotate.Contains(cell))
                    _cellsAreaRotate.Remove(cell);

            foreach (var cell in _cellsAreaRotate)
                if (_heapFigures.Contains(cell))
                    return false;

            return true;
        }
        public bool CheckMoveToSide(Bounds figureArg, IReadOnlyList<Bounds> blocksArg, Vector3 deltaPosArg)
        {
            if (_heapFigures.Bounds.Intersects(figureArg))
                foreach (var figureBlock in blocksArg)
                {
                    Bounds movedBlock = figureBlock.WithDeltaPos(deltaPosArg);
                    if (_heapFigures.Intersect(movedBlock.center))
                        return false;
                }
            

            return true;
        }


        private Bounds GetAreaRotate(Bounds beforeRotateArg, Bounds afterRotateArg)
        {
            Bounds areaRotate = new Bounds();

            float minX = Mathf.Min(beforeRotateArg.min.x, afterRotateArg.min.x);
            float minY = Mathf.Min(beforeRotateArg.min.y, afterRotateArg.min.y);

            float maxX = Mathf.Max(beforeRotateArg.max.x, afterRotateArg.max.x);
            float maxY = Mathf.Max(beforeRotateArg.max.y, afterRotateArg.max.y);

            areaRotate.SetMinMax(new Vector3(minX, minY, 0), new Vector3(maxX, maxY, 0));

            return areaRotate;
        }
    }
}

