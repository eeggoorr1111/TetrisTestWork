using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Tetris
{
    public class MoverBlockedWall : Mover
    {
        public MoverBlockedWall(HeapFigures heapArg, Difficulty difficultyArg, Map mapArg) : base(heapArg, difficultyArg, mapArg) { }


        public override bool MoveToSide(bool toRightArg, BoundsFigure bounds)
        {
            if (_moveToSide.IsActive())
                return false;
            else if (toRightArg && bounds.Figure.max.x + 1 > _map.CenterRight.x + float.Epsilon)
                return false;
            else if (!toRightArg && bounds.Figure.min.x - 1 < _map.CenterLeft.x - float.Epsilon)
                return false;

            float timeMoveToSide = _difficulty.TimeMoveToSide;
            Vector3 deltaMove = toRightArg ? Vector3.right : Vector3.left;
            Vector3 vectorFall = _difficulty.SpeedFalling * timeMoveToSide * _fallDirection;
            Vector3 deltaMoveWithFall = vectorFall + deltaMove;
            Bounds figure = bounds.Figure.WithDeltaPos(deltaMoveWithFall);

            if (_heapFigures.Bounds.Intersects(figure))
            {
                foreach (var heapBlock in _heapFigures.Blocks)
                    foreach (var figureBlock in bounds.Blocks)
                        if (heapBlock.Intersects(figureBlock.WithDeltaPos(deltaMoveWithFall)))
                            return false;
            }

            _moveToSide = DOTween.To(() => bounds.Center, (pos) => bounds.MoveTo(pos), figure.center, timeMoveToSide).SetEase(Ease.OutSine);

            return true;
        }
    }
}

