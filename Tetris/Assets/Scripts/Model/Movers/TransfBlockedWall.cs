using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Tetris
{
    public class TransfBlockedWall : Transformator
    {
        public TransfBlockedWall(HeapFigures heapArg, Difficulty difficultyArg, MapData mapArg, CalculateParams paramsArg, CheckCollisionHeap collisionHeapArg) : 
            base(heapArg, difficultyArg, mapArg, paramsArg, collisionHeapArg) { }


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
            Vector3 vectorFall = _difficulty.SpeedFalling * timeMoveToSide * FallDirection;
            Vector3 deltaMoveWithFall = vectorFall + deltaMove;
            Bounds figure = collider.Bounds.WithDeltaPos(deltaMoveWithFall);

            if (_collisionHeap.CheckMoveToSide(figure, collider.Blocks, deltaMoveWithFall))
            {
                _moveToSide = DOTween.To(() => collider.Center, (pos) => collider.ToMoveTo(pos), figure.center, timeMoveToSide).SetEase(Ease.OutSine);
                return true;
            }

            return false;
        }
        public override void Rotate(ColliderFigure collider)
        {
            if (_rotate.IsActive() || _moveToSide.IsActive())
                return;

            Quaternion targetRotate = (Matrix4x4.Rotate(collider.Rotate) * MatrixRotate).rotation;

            Bounds afterRotate;
            Bounds beforeRotate = collider.Bounds;

            collider.GetDataAfterRotate(targetRotate, _blocksAfterRotate, out afterRotate);

            if (afterRotate.GetMaxCell().x > _map.MaxCell.x ||
                afterRotate.GetMinCell().x < _map.MinCell.x)
                return;

            if (_collisionHeap.CheckRotate(beforeRotate, afterRotate, _blocksAfterRotate))
                _rotate = DOTween.To(() => collider.Rotate, collider.ToRotate, targetRotate.eulerAngles, _difficulty.TimeRotate).SetEase(Ease.OutSine);
        }
    }
}

