using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

namespace Tetris
{
    public abstract class Transformator
    {
        protected static readonly Vector3 _fallDirection = Vector3Int.down;
        protected static readonly Matrix4x4 _matrixRotate = new Matrix4x4(  new Vector4(0f, -1f, 0f, 0f),
                                                                            new Vector4(1f, 0f, 0f, 0f),
                                                                            new Vector4(0f, 0f, 1f, 0f),
                                                                            new Vector4(0f, 0f, 0f, 1f));


        public Transformator(HeapFigures heapArg, Difficulty difficultyArg, MapData mapArg, CalculateParams paramsArg)
        {
            _heapFigures = heapArg;
            _difficulty = difficultyArg;
            _map = mapArg;
            _params = paramsArg;

            _blocks = new List<Bounds>();
            _cellsAreaRotate = new HashSet<Vector2Int>();
            _cellsTemp = new HashSet<Vector2Int>();
        }


        protected HeapFigures _heapFigures;
        protected Difficulty _difficulty;
        protected MapData _map;
        protected Tween _moveToSide;
        protected Tween _rotate;
        protected CalculateParams _params;
        protected List<Bounds> _blocks;
        private HashSet<Vector2Int> _cellsAreaRotate;
        private HashSet<Vector2Int> _cellsTemp;


        public bool ToFall(bool boostedFallArg, ColliderFigure colliderArg)
        {
            bool isMove = true;
            if (_moveToSide.IsActive())
                return isMove;

            float distance = GetDistanceToNearestObstruction(colliderArg);
            if (Mathf.Approximately(distance, 0f))
            {
                isMove = false;
                return isMove;
            }

            float speed = _difficulty.SpeedFalling;
            if (boostedFallArg)
                speed = _difficulty.SpeedFallingBoosted;

            Vector3 delta = _fallDirection * speed * Time.deltaTime;
            if (Mathf.Abs(delta.y) > distance)
                delta.y = -distance;

            colliderArg.ToMove(delta);

            return isMove;
        }
        public abstract bool MoveToSide(bool toRightArg, ColliderFigure collider);
        public abstract void Rotate(ColliderFigure collider);


        protected float GetDistanceToNearestObstruction(ColliderFigure colliderArg)
        {
            float distance = colliderArg.Bounds.min.y - _map.BottomByY;
            float distanceToBoundsHeap = colliderArg.Bounds.min.y - _heapFigures.Bounds.max.y;

            if (distanceToBoundsHeap > _params.AllowedError)
                return distanceToBoundsHeap;

            for (int i = 0; i < colliderArg.Blocks.Count; i++)
            {
                int posX = Mathf.RoundToInt(colliderArg.Blocks[i].center.x);
                Bounds figureBlock = colliderArg.Blocks[i];
                Bounds heapBlock;

                if (_heapFigures.GetUpperBlock(posX, figureBlock.center.y, out heapBlock))
                {
                    float distanceToHeap = figureBlock.min.y - heapBlock.max.y;
                    if (distanceToHeap < _params.AllowedError)
                        return 0f;

                    if (distanceToHeap < distance)
                        distance = distanceToHeap;
                }
            }

            return distance;
        }
        protected bool CheckOnCollisionWithHeap(Bounds beforeRotateArg, Bounds afterRotateArg, IReadOnlyList<Bounds> blocksAfterRotateArg)
        {
            Bounds areaRotate = GetAreaRotate(beforeRotateArg, afterRotateArg);
            if (!_heapFigures.Bounds.Intersects(areaRotate))
                return true;

            foreach (var block in blocksAfterRotateArg)
                if (_heapFigures.Intersect(block.center))
                    return false;

            areaRotate.GetCells(_cellsAreaRotate);

            afterRotateArg.GetCells(_cellsTemp);
            foreach (var cell in _cellsTemp)
                _cellsAreaRotate.Remove(cell);

            beforeRotateArg.GetCells(_cellsTemp);
            foreach (var cell in _cellsTemp)
                if (_cellsAreaRotate.Contains(cell))
                    _cellsAreaRotate.Remove(cell);

            foreach (var cell in _cellsAreaRotate)
                if (_heapFigures.Contains(cell))
                    return false;

            return true;
        }
        protected Bounds GetAreaRotate(Bounds beforeRotateArg, Bounds afterRotateArg)
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
