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
            
            Vector3 deltaMove = toRightArg ? Vector3.right : Vector3.left;
            Vector3 deltaMoveWithFall = GetFall(_difficulty.TimeMoveToSide) + deltaMove;
            Bounds figure = collider.Bounds.WithDeltaPos(deltaMoveWithFall);

            if (_collisionHeap.CheckMoveToSide(figure, collider.Blocks, deltaMoveWithFall))
            {
                _moveToSide = DOTween.To(() => collider.Center, (pos) => ToMoveTo(collider, pos), figure.center, _difficulty.TimeMoveToSide).SetEase(Ease.OutSine);
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
            Vector3 deltaFall = GetFall(_difficulty.TimeRotate);

            GetDataAfterRotate(collider, targetRotate, _blocks, out afterRotate);
            afterRotate.WithDeltaPos(deltaFall);

            if (afterRotate.GetMaxCell().x > _map.MaxCell.x ||
                afterRotate.GetMinCell().x < _map.MinCell.x)
                return;
            
            if (_collisionHeap.CheckRotate(beforeRotate, afterRotate, _blocks, deltaFall))
                _rotate = DOTween.To(() => collider.Rotate, rotate => ToRotate(collider, rotate), targetRotate.eulerAngles, _difficulty.TimeRotate).SetEase(Ease.OutSine);
        }


        protected void GetDataAfterRotate(ColliderFigure colliderArg, Quaternion rotateArg, List<Bounds> blocks, out Bounds bounds)
        {
            Vector3 pivotPos = colliderArg.Pivot;

            blocks.Clear();
            for (int i = 0; i < colliderArg.Blocks.Count; i++)
            {
                Vector2Int blockLocalPos = colliderArg.BlocksLocalPos[i];
                Vector3 localPos = new Vector3(blockLocalPos.x, blockLocalPos.y, 0);
                Vector3 newLocalPos = Matrix4x4.Rotate(rotateArg).MultiplyPoint(localPos);

                blocks.Add(new Bounds(newLocalPos + pivotPos, colliderArg.Blocks[i].size));
            }

            bounds = Helpers.GetBounds(blocks);
        }
        protected void ToRotate(ColliderFigure colliderArg, Quaternion rotateArg)
        {
            Bounds bounds;
            GetDataAfterRotate(colliderArg, rotateArg, _blocks, out bounds);

            colliderArg.Tranasform(bounds, _blocks, rotateArg);
        }
        protected void ToMoveTo(ColliderFigure colliderArg, Vector3 pointArg)
        {
            Vector3 delta = pointArg - colliderArg.Bounds.center;
            ToMove(colliderArg, delta);
        }
    }
}

