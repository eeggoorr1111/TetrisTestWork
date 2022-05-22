using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

namespace Tetris.Model
{
    public abstract class Transformator
    {
        public static readonly Vector3 FallDirection = Vector3Int.down;
        public static readonly Matrix4x4 MatrixRotate = new Matrix4x4(  new Vector4(0f, -1f, 0f, 0f),
                                                                        new Vector4(1f, 0f, 0f, 0f),
                                                                        new Vector4(0f, 0f, 1f, 0f),
                                                                        new Vector4(0f, 0f, 0f, 1f));


        public Transformator(   HeapBlocks heapArg,
                                ILevelsParams lvlsParamsArg, 
                                MapData mapArg, 
                                CalculateParams paramsArg, 
                                CheckerOnCollisionWithHeap collisionArg)
        {
            _heapFigures = heapArg;
            _lvlsParams = lvlsParamsArg;
            _map = mapArg;
            _params = paramsArg;
            _collisionHeap = collisionArg;
            _blocks = new List<Bounds>(8);
        }


        public Vector3 FallWhileRotate => GetFall(TimeRotate);
        public float TimeRotate => _lvlsParams.Current.TimeRotate;
        public float TimeMoveToSide => _lvlsParams.Current.TimeMoveToSide;


        protected readonly HeapBlocks _heapFigures;
        protected readonly ILevelsParams _lvlsParams;
        protected readonly MapData _map;
        protected readonly CalculateParams _params;
        protected readonly CheckerOnCollisionWithHeap _collisionHeap;
        private readonly List<Bounds> _blocks;
        protected Tween _moveToSide;
        protected Tween _rotate;
        

        public bool ToFall(bool boostedFallArg, ColliderFigure colliderArg)
        {
            // TODO: Sometimes figures collide with a heap in the process of turning. 
            // Align blocks in this case to the centers of the cells
            bool isMove = true;
            if (_moveToSide.IsActive() || _rotate.IsActive())
                return isMove;

            float distance = GetDistanceToNearestObstruction(colliderArg);
            if (Mathf.Approximately(distance, 0f))
            {
                isMove = false;
                return isMove;
            }

            float speed = _lvlsParams.Current.SpeedFalling;
            if (boostedFallArg)
                speed = _lvlsParams.Current.SpeedFallingBoosted;

            Vector3 delta = FallDirection * speed * Time.deltaTime;
            if (Mathf.Abs(delta.y) > distance)
                delta.y = -distance;

            ToMove(colliderArg, delta);

            return isMove;
        }
        public abstract bool MoveToSide(bool toRightArg, ColliderFigure collider);
        public abstract void ToRotate(ColliderFigure collider);


        protected void GetBlocksAfterRotate(ColliderFigure colliderArg, Quaternion rotateArg, Vector3 deltaFallArg, List<Bounds> blocks)
        {
            Vector3 pivotPos = colliderArg.Pivot;

            blocks.Clear();
            for (int i = 0; i < colliderArg.Blocks.Count; i++)
            {
                Vector2Int blockLocalPos = colliderArg.BlocksLocalPos[i];
                Vector3 localPos = new Vector3(blockLocalPos.x, blockLocalPos.y, 0);
                Vector3 newLocalPos = Matrix4x4.Rotate(rotateArg).MultiplyPoint(localPos);
                Bounds block = new Bounds(newLocalPos + pivotPos, colliderArg.Blocks[i].size);

                blocks.Add(block.WithDeltaPos(deltaFallArg));
            }
        }
        protected Vector3 GetFall(float timeArg)
        {
            return _lvlsParams.Current.SpeedFalling * timeArg * FallDirection;
        }
        protected float GetDistanceToNearestObstruction(ColliderFigure colliderArg)
        {
            float distance = colliderArg.Bounds.min.y - _map.BottomByY;
            float distanceToBoundsHeap = colliderArg.Bounds.min.y - _heapFigures.Bounds.max.y;

            if (distanceToBoundsHeap > _params.AllowedError)
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
        protected void ToMove(ColliderFigure colliderArg, Vector3 deltaArg)
        {
            Bounds bounds = colliderArg.Bounds.WithDeltaPos(deltaArg);

            _blocks.Clear();
            for (int i = 0; i < colliderArg.Blocks.Count; i++)
                _blocks.Add(colliderArg.Blocks[i].WithDeltaPos(deltaArg));

            colliderArg.Transform(bounds, _blocks);
        }
    }
}
