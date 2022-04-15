using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tetris
{
    public abstract class Mover
    {
        protected static readonly Vector3 _fallDirection = Vector3Int.down;


        public Mover(HeapFigures heapArg, Difficulty difficultyArg)
        {
            _heapFigures = heapArg;
            _difficulty = difficultyArg;
        }


        protected HeapFigures _heapFigures;
        protected Difficulty _difficulty;


        public bool ToFall(bool boostedFallArg, IReadOnlyList<Bounds> boundsBottomArg, List<Bounds> boundsBlocks, ref Bounds boundsFigure)
        {
            float distanceToBottomHeap = boundsFigure.min.y - _heapFigures.BottomByY;
            if (distanceToBottomHeap < float.Epsilon)
                return false;

            float speed = _difficulty.SpeedFalling;
            if (boostedFallArg)
                speed = _difficulty.SpeedFallingBoosted;

            Vector3 delta = _fallDirection * speed * Time.deltaTime;
            if (Mathf.Abs(delta.y) > distanceToBottomHeap)
                delta.y = -distanceToBottomHeap;

            Bounds newBoundsFigure = boundsFigure.WithDeltaPos(delta);
            if (IsCollisionWithHeap(newBoundsFigure, boundsBottomArg, delta))
                return false;

            boundsFigure = newBoundsFigure;
            for (int i = 0; i < boundsBlocks.Count; i++)
                boundsBlocks[i] = boundsBlocks[i].WithDeltaPos(delta);

            return true;

        }
        public abstract bool ToMove(bool toRightArg);


        protected bool IsCollisionWithHeap(Bounds boundsFigureArg, IReadOnlyList<Bounds> boundsArg, Vector3 deltaArg)
        {
            if (!boundsFigureArg.Intersects(_heapFigures.Bounds))
                return false;

            for (int i = 0; i < boundsArg.Count; i++)
                for (int j = 0; j < _heapFigures.BoundsOfTop.Count; j++)
                {
                    Bounds newBounds = boundsArg[i].WithDeltaPos(deltaArg);
                    if (newBounds.Intersects(_heapFigures.BoundsOfTop[j]))
                        return true;
                }
                    

            return false;
        }
    }
}
