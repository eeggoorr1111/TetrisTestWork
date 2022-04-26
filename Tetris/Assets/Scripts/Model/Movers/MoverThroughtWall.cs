using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Tetris
{
    public class MoverThroughtWall : Transformator
    {
        public MoverThroughtWall(HeapFigures heapArg, Difficulty difficultyArg, MapData mapArg, CalculateParams paramsArg, CheckCollisionHeap collisionHeapArg) : 
            base(heapArg, difficultyArg, mapArg, paramsArg, collisionHeapArg) { }


        public override bool MoveToSide(bool toRightArg, ColliderFigure colliderArg)
        {
            if (_moveToSide.IsActive())
                return false;

            float timeMoveToSide = _difficulty.TimeMoveToSide;
            Vector3 deltaMove = toRightArg ? Vector3.right : Vector3.left;
            Vector3 deltaMoveWithFall = GetFall(timeMoveToSide) + deltaMove;
            Bounds figure = colliderArg.Bounds.WithDeltaPos(deltaMoveWithFall);

            _blocks.Clear();
            foreach (var block in colliderArg.Blocks)
                _blocks.Add(WithDeltaThroughtWall(block, deltaMoveWithFall));

            if (_collisionHeap.CheckMoveToSide(_blocks))
            {
                _moveToSide = DOTween.To(() => colliderArg.Center, (pos) => ToMoveTo(colliderArg, pos), figure.center, timeMoveToSide).SetEase(Ease.OutSine).OnComplete(() => EndedMove(colliderArg));
                return true;
            }

            return false;
        }
        public override void Rotate(ColliderFigure colliderArg)
        {
            
        }


        protected void EndedMove(ColliderFigure colliderArg)
        {
            Vector3 newPos = PointThroughtWall(colliderArg.Center);
            colliderArg.Tranasform(new Bounds(newPos, colliderArg.Bounds.size));
        }
        protected Bounds WithDeltaThroughtWall(Bounds boundsArg, Vector3 deltaArg)
        {
            Vector3 newPos = boundsArg.center + deltaArg;
            Vector3 newPosThroughtWall = PointThroughtWall(newPos);

            return new Bounds(newPosThroughtWall, boundsArg.size);
        }
        protected Vector3 PointThroughtWall(Vector3 pointArg)
        {
            if (pointArg.x > _map.MaxPoint.x)
                pointArg.x = _map.MinPoint.x + pointArg.x - _map.MaxPoint.x;
            else if (pointArg.x < _map.MinPoint.x)
                pointArg.x = _map.MaxPoint.x - Mathf.Abs(_map.MinPoint.x - pointArg.x);

            return pointArg;
        }
        protected void ToMoveTo(ColliderFigure colliderArg, Vector3 pointArg)
        {
            Vector3 delta = pointArg - colliderArg.Bounds.center;
            Bounds figure = new Bounds(pointArg, colliderArg.Bounds.size);

            _blocks.Clear();
            for (int i = 0; i < colliderArg.Blocks.Count; i++)
                _blocks.Add(WithDeltaThroughtWall(colliderArg.Blocks[i], delta));

            colliderArg.Tranasform(figure, _blocks);
        }
    }
}

