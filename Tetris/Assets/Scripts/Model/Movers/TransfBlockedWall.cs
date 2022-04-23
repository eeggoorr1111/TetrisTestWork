using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Tetris
{
    public class TransfBlockedWall : Transformator
    {
        public TransfBlockedWall(HeapFigures heapArg, Difficulty difficultyArg, MapData mapArg, CalculateParams paramsArg) : base(heapArg, difficultyArg, mapArg, paramsArg) { }


        public override bool MoveToSide(bool toRightArg, ColliderFigure collider)
        {
            if (_moveToSide.IsActive())
                return false;
            else if (toRightArg && collider.RightX + 1 > _map.MaxCell.x)
                    return false;
            else if (!toRightArg && collider.LeftX - 1 < _map.MinCell.x)
                    return false;
            

            float timeMoveToSide = _difficulty.TimeMoveToSide;
            Vector3 deltaMove = toRightArg ? Vector3.right : Vector3.left;
            Vector3 vectorFall = _difficulty.SpeedFalling * timeMoveToSide * _fallDirection;
            Vector3 deltaMoveWithFall = vectorFall + deltaMove;
            Bounds figure = collider.Bounds.WithDeltaPos(deltaMoveWithFall);

            if (_heapFigures.Bounds.Intersects(figure))
            {
                foreach (var figureBlock in collider.Blocks)
                {
                    Bounds movedBlock = figureBlock.WithDeltaPos(deltaMoveWithFall);
                    if (_heapFigures.Intersect(movedBlock.center))
                        return false;
                }
            }

            _moveToSide = DOTween.To(() => collider.Center, (pos) => collider.ToMoveTo(pos), figure.center, timeMoveToSide).SetEase(Ease.OutSine);

            return true;
        }
        public override void Rotate(ColliderFigure collider)
        {
            if (_rotate.IsActive() || _moveToSide.IsActive())
                return;

            Quaternion targetRotate = (Matrix4x4.Rotate(collider.Rotate) * _matrixRotate).rotation;

            Bounds afterRotate;
            Bounds beforeRotate = collider.Bounds;

            collider.GetDataAfterRotate(targetRotate, _blocks, out afterRotate);

            if (afterRotate.GetMaxCell().x > _map.MaxCell.x ||
                afterRotate.GetMinCell().x < _map.MinCell.x)
                return;

            if (CheckOnCollisionWithHeap(beforeRotate, afterRotate, _blocks))
                _rotate = DOTween.To(() => collider.Rotate, collider.ToRotate, targetRotate.eulerAngles, _difficulty.TimeRotate).SetEase(Ease.OutSine);
        }
    }
}

