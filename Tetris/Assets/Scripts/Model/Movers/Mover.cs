using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Tetris
{
    public abstract class Mover
    {
        protected static readonly Vector3 _fallDirection = Vector3Int.down;


        public Mover(HeapFigures heapArg, Difficulty difficultyArg, Map mapArg, CalculateParams paramsArg)
        {
            _heapFigures = heapArg;
            _difficulty = difficultyArg;
            _map = mapArg;
            _params = paramsArg;
        }


        protected HeapFigures _heapFigures;
        protected Difficulty _difficulty;
        protected Map _map;
        protected Tween _moveToSide;
        protected CalculateParams _params;


        public bool ToFall(bool boostedFallArg, BoundsFigure bounds)
        {
            if (_moveToSide.IsActive())
                return true;

            float distance = GetDistanceToNearestObstruction(bounds);
            if (Mathf.Approximately(distance, 0f))
                return false;

            float speed = _difficulty.SpeedFalling;
            if (boostedFallArg)
                speed = _difficulty.SpeedFallingBoosted;

            Vector3 delta = _fallDirection * speed * Time.deltaTime;
            if (Mathf.Abs(delta.y) > distance)
                delta.y = -distance;

            bounds.Move(delta);

            return true;

        }
        public abstract bool MoveToSide(bool toRightArg, BoundsFigure bounds);


        protected float GetDistanceToNearestObstruction(BoundsFigure boundsArg)
        {
            float distance = boundsArg.Figure.min.y - _map.BottomByY;
            float distanceToBoundsHeap = boundsArg.Figure.min.y - _heapFigures.Bounds.max.y;

            if (distanceToBoundsHeap > float.Epsilon)
                return distanceToBoundsHeap;

            for (int i = 0; i < boundsArg.Blocks.Count; i++)
            {
                int posX = Mathf.RoundToInt(boundsArg.Blocks[i].center.x);
                Bounds figureBlock = boundsArg.Blocks[i];
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
    }
}
