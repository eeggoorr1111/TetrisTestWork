using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

namespace Tetris.Model
{
    public sealed class TransformatorThroughtWall : Transformator
    {
        public TransformatorThroughtWall(   HeapFigures heapArg, 
                                            ILevelsParams lvlsParamsArg, 
                                            MapData mapArg, 
                                            CalculateParams paramsArg, 
                                            CheckCollisionHeap collisionHeapArg) : 
            base(heapArg, lvlsParamsArg, mapArg, paramsArg, collisionHeapArg) 
        {
            _replace = new HashSet<Vector2Int>();
            _areaRotate = new HashSet<Vector2Int>();
            _blocks = new List<Bounds>(8);
            _blocks2 = new List<Bounds>(8);
        }


        private readonly HashSet<Vector2Int> _replace;
        private readonly HashSet<Vector2Int> _areaRotate;
        private readonly List<Bounds> _blocks;
        private readonly List<Bounds> _blocks2;


        public override bool MoveToSide(bool toRightArg, ColliderFigure colliderArg)
        {
            if (_moveToSide.IsActive())
                return false;

            Vector3 deltaMove = toRightArg ? Vector3.right : Vector3.left;
            Vector3 deltaMoveWithFall = GetFall(TimeMoveToSide) + deltaMove;
            Bounds figure = colliderArg.Bounds.WithDeltaPos(deltaMoveWithFall);

            _blocks.Clear();
            foreach (var block in colliderArg.Blocks)
                _blocks.Add(ApplyDeltaToBlock(block, deltaMoveWithFall));

            if (_collisionHeap.CheckMoveToSide(_blocks))
            {
                _moveToSide = DOTween.To(() => colliderArg.Center, (pos) => ToMoveTo(colliderArg, pos), figure.center, TimeMoveToSide).
                    OnComplete(() => EndedMove(colliderArg)).
                    SetEase(Ease.OutSine);
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

            GetBlocksAfterRotate(colliderArg, colliderArg.Rotate, Vector3.zero, _blocks);
            GetBlocksAfterRotate(colliderArg, targetRotate, FallWhileRotate, _blocks2);

            _collisionHeap.GetAreaRotate(colliderArg, _blocks, _blocks2, _areaRotate);
            AreaRotateThroughtWall(_areaRotate);

            if (_collisionHeap.CheckArea(_areaRotate))
                _rotate = DOTween.To(() => colliderArg.Rotate, rotate => ToRotate(colliderArg, rotate), targetRotateE, TimeRotate).SetEase(Ease.OutSine);
        }


        
        private void AreaRotateThroughtWall(HashSet<Vector2Int> areaRotate)
        {
            _replace.Clear();
            foreach (var cell in areaRotate)
                if (cell.x > _map.MaxCell.x || cell.x < _map.MinCell.x)
                    _replace.Add(cell);

            foreach (var cell in _replace)
            {
                areaRotate.Remove(cell);
                areaRotate.Add(CellThroughtWall(cell));
            }
        }
        private void ToRotate(ColliderFigure colliderArg, Quaternion rotateArg)
        {
            GetBlocksAfterRotate(colliderArg, rotateArg, GetFall(Time.deltaTime), _blocks);

            for (int i = 0; i < _blocks.Count; i++)
                _blocks[i] = BoundsThroughtWall(_blocks[i]);

            colliderArg.Transform(Helpers.GetBounds(_blocks), _blocks, rotateArg);
        }
        private void EndedMove(ColliderFigure colliderArg)
        {
            Vector3 newPos = PointThroughtWall(colliderArg.Center);
            colliderArg.Transform(new Bounds(newPos, colliderArg.Bounds.size));
        }
        private Bounds ApplyDeltaToBlock(Bounds boundsArg, Vector3 deltaArg)
        {
            Vector3 newPos = boundsArg.center + deltaArg;
            Vector3 newPosThroughtWall = PointThroughtWall(newPos);

            return new Bounds(newPosThroughtWall, boundsArg.size);
        }
        private Vector3 PointThroughtWall(Vector3 pointArg)
        {
            if (pointArg.x > _map.MaxPoint.x)
                pointArg.x = _map.MinPoint.x + pointArg.x - _map.MaxPoint.x;
            else if (pointArg.x < _map.MinPoint.x)
                pointArg.x = _map.MaxPoint.x - Mathf.Abs(_map.MinPoint.x - pointArg.x);

            return pointArg;
        }
        private Bounds BoundsThroughtWall(Bounds boundsArg)
        {
            Vector3 center = PointThroughtWall(boundsArg.center);
            return new Bounds(center, boundsArg.size);
        }
        private Vector2Int CellThroughtWall(Vector2Int cellArg)
        {
            if (cellArg.x > _map.MaxCell.x)
                cellArg.x = _map.MinCell.x + cellArg.x - _map.MaxCell.x - 1;
            else if (cellArg.x < _map.MinCell.x)
                cellArg.x = _map.MaxCell.x - Mathf.Abs(_map.MinCell.x - cellArg.x) + 1;

            return cellArg;
        }
        private void ToMoveTo(ColliderFigure colliderArg, Vector3 pointArg)
        {
            Vector3 delta = pointArg - colliderArg.Bounds.center;
            Bounds figure = new Bounds(pointArg, colliderArg.Bounds.size);

            _blocks.Clear();
            for (int i = 0; i < colliderArg.Blocks.Count; i++)
                _blocks.Add(ApplyDeltaToBlock(colliderArg.Blocks[i], delta));

            colliderArg.Transform(figure, _blocks);
        }
    }
}

