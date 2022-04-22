using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

namespace Tetris
{
    public abstract class Mover
    {
        protected static readonly Vector3 _fallDirection = Vector3Int.down;


        public Mover(HeapFigures heapArg, Difficulty difficultyArg, MapData mapArg, CalculateParams paramsArg)
        {
            _heapFigures = heapArg;
            _difficulty = difficultyArg;
            _map = mapArg;
            _params = paramsArg;
        }


        protected HeapFigures _heapFigures;
        protected Difficulty _difficulty;
        protected MapData _map;
        protected Tween _moveToSide;
        protected CalculateParams _params;


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
        public abstract bool MoveToSide(bool toRightArg, ColliderFigure colliderArg);


        protected float GetDistanceToNearestObstruction(ColliderFigure colliderArg)
        {
            float distance = colliderArg.Bounds.min.y - _map.BottomByY;
            float distanceToBoundsHeap = colliderArg.Bounds.min.y - _heapFigures.Bounds.max.y;

            if (distanceToBoundsHeap > float.Epsilon)
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
    }
}
