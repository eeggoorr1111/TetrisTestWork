using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tetris
{
    public abstract class Mover
    {
        protected static readonly Vector3 _fallDirection = Vector3Int.down;


        public Mover(HeapFigures heapArg, Difficulty difficultyArg, Map mapArg)
        {
            _heapFigures = heapArg;
            _difficulty = difficultyArg;
            _map = mapArg;
        }


        protected HeapFigures _heapFigures;
        protected Difficulty _difficulty;
        protected Map _map;


        public bool ToFall(bool boostedFallArg, IReadOnlyList<Bounds> boundsBottomArg, List<Bounds> boundsBlocks, ref Bounds boundsFigure)
        {
            float distance = GetDistanceToNearestObstruction(boundsFigure, boundsBottomArg);
            if (distance < float.Epsilon)
                return false;

            float speed = _difficulty.SpeedFalling;
            if (boostedFallArg)
                speed = _difficulty.SpeedFallingBoosted;

            Vector3 delta = _fallDirection * speed * Time.deltaTime;
            if (Mathf.Abs(delta.y) > distance)
                delta.y = -distance;

            boundsFigure = boundsFigure.WithDeltaPos(delta);
            for (int i = 0; i < boundsBlocks.Count; i++)
                boundsBlocks[i] = boundsBlocks[i].WithDeltaPos(delta);

            return true;

        }
        public abstract bool ToMove(bool toRightArg);


        protected float GetDistanceToNearestObstruction(Bounds boundsFigureArg, IReadOnlyList<Bounds> blocksFigureArg)
        {
            float distance = boundsFigureArg.min.y - _map.BottomByY;

            IReadOnlyDictionary<int, Bounds> blocksTopHeap = _heapFigures.BoundsOfTop;
            for (int i = 0; i < blocksFigureArg.Count; i++)
            {
                int posX = Mathf.RoundToInt(blocksFigureArg[i].center.x);
                Bounds blockFigure = blocksFigureArg[i];
                Bounds blockTopHeap;

                if (blocksTopHeap.TryGetValue(posX, out blockTopHeap))
                {
                    float distanceToHeap = Mathf.Abs(blockTopHeap.max.y - blockFigure.min.y);
                    if (distanceToHeap < distance)
                        distance = distanceToHeap;
                }
            }

            return distance;
        }
    }
}
