using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Tetris.Model
{
    public class TransfBlockedWall : Transformator
    {
        public TransfBlockedWall(HeapBlocks heapArg, ILevelsParams lvlsParamsArg, MapData mapArg, CalculateParams paramsArg, CheckerOnCollisionWithHeap collisionHeapArg) : 
            base(heapArg, lvlsParamsArg, mapArg, paramsArg, collisionHeapArg) 
        {
            _blocks = new List<Bounds>();
        }


        private readonly List<Bounds> _blocks;


        public override bool MoveToSide(bool toRightArg, ColliderFigure colliderArg)
        {
            if (_moveToSide.IsActive())
                return false;
            else if (toRightArg && colliderArg.RightColumn + 1 > _map.MaxCell.x)
                return false;
            else if (!toRightArg && colliderArg.LeftColumn - 1 < _map.MinCell.x)
                return false;
            
            Vector3 deltaMove = toRightArg ? Vector3.right : Vector3.left;
            Vector3 deltaMoveWithFall = GetFall(TimeMoveToSide) + deltaMove;
            Bounds figure = colliderArg.Bounds.WithDeltaPos(deltaMoveWithFall);

            _blocks.Clear();
            foreach (var block in colliderArg.Blocks)
                _blocks.Add(block.WithDeltaPos(deltaMoveWithFall));

            if (_collisionHeap.CheckMoveToSide(figure, _blocks))
            {
                _moveToSide = DOTween.To(() => colliderArg.Center, (pos) => ToMoveTo(colliderArg, pos), figure.center, TimeMoveToSide).SetEase(Ease.OutSine);
                return true;
            }

            return false;
        }
        public override void ToRotate(ColliderFigure colliderArg)
        {
            if (_rotate.IsActive() || _moveToSide.IsActive())
                return;

            Quaternion targetRotate = (Matrix4x4.Rotate(colliderArg.Rotate) * MatrixRotate).rotation;
            Vector3 targetRotateE = targetRotate.eulerAngles;

            GetBlocksAfterRotate(colliderArg, targetRotate, FallWhileRotate, _blocks);
            Bounds afterRotate = Helpers.GetBounds(_blocks);

            if (afterRotate.GetMaxCell().x > _map.MaxCell.x ||
                afterRotate.GetMinCell().x < _map.MinCell.x)
                return;

            if (_collisionHeap.CheckRotate(colliderArg, _blocks))
                _rotate = DOTween.To(() => colliderArg.Rotate, rotate => ToRotate(colliderArg, rotate), targetRotateE, TimeRotate).SetEase(Ease.OutSine);
        }



        private void ToMoveTo(ColliderFigure colliderArg, Vector3 pointArg)
        {
            Vector3 delta = pointArg - colliderArg.Bounds.center;
            ToMove(colliderArg, delta);
        }
        private void ToRotate(ColliderFigure colliderArg, Quaternion rotateArg)
        {
            GetBlocksAfterRotate(colliderArg, rotateArg, GetFall(Time.deltaTime), _blocks);
            Bounds bounds = Helpers.GetBounds(_blocks);

            colliderArg.Transform(bounds, _blocks, rotateArg);
        }
    }
}

