using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Tetris
{
    public sealed class Rotator
    {
        public Rotator(Difficulty difficultyArg, MapData mapArg, HeapFigures heapArg)
        {
            _difficulty = difficultyArg;
            _blocks = new List<Bounds>();
            _map = mapArg;
            _heapFigures = heapArg;
            _cellsAreaRotate = new HashSet<Vector2Int>();
            _cellsTemp = new HashSet<Vector2Int>();
        }


        private Tween _rotate;
        private Difficulty _difficulty;
        private MapData _map;
        private HeapFigures _heapFigures;
        private List<Bounds> _blocks;
        private HashSet<Vector2Int> _cellsAreaRotate;
        private HashSet<Vector2Int> _cellsTemp;


        public void Rotate(ColliderFigure collider)
        {
            if (_rotate.IsActive())
                return;

            Matrix4x4 rotate = new Matrix4x4(   new Vector4(0f, -1f, 0f, 0f),
                                                new Vector4(1f, 0f, 0f, 0f),
                                                new Vector4(0f, 0f, 1f, 0f),
                                                new Vector4(0f, 0f, 0f, 1f));

            Matrix4x4 targetRotateMatrix = Matrix4x4.Rotate(collider.Rotate) * rotate;
            Quaternion targetRotate = targetRotateMatrix.rotation;

            if (CanRotate(collider, targetRotateMatrix))
                _rotate = DOTween.To(() => collider.Rotate, collider.ToRotate, targetRotate.eulerAngles, _difficulty.TimeRotate).SetEase(Ease.OutSine);
        }


        private bool CanRotate(ColliderFigure colliderArg, Matrix4x4 rotateArg)
        {
            Bounds afterRotate;
            Bounds beforeRotate = colliderArg.Bounds;

            colliderArg.GetDataAfterRotate(rotateArg.rotation, _blocks, out afterRotate);

            if (afterRotate.GetMaxCell().x > _map.MaxCell.x ||
                afterRotate.GetMinCell().x < _map.MinCell.x)
                return false;

            Bounds areaRotate = GetAreaRotate(beforeRotate, afterRotate);
            if (!_heapFigures.Bounds.Intersects(areaRotate))
                return true;

            foreach (var block in _blocks)
                if (_heapFigures.Intersect(block.center))
                    return false;

            areaRotate.GetCells(_cellsAreaRotate);

            afterRotate.GetCells(_cellsTemp);
            foreach (var cell in _cellsTemp)
                _cellsAreaRotate.Remove(cell);

            beforeRotate.GetCells(_cellsTemp);
            foreach (var cell in _cellsTemp)
                if (_cellsAreaRotate.Contains(cell))
                    _cellsAreaRotate.Remove(cell);

            foreach (var cell in _cellsAreaRotate)
                if (_heapFigures.Contains(cell))
                    return false;

            return true;
        }
        private Bounds GetAreaRotate(Bounds beforeRotateArg, Bounds afterRotateArg)
        {
            Bounds areaRotate = new Bounds();

            float minX = Mathf.Min(beforeRotateArg.min.x, afterRotateArg.min.x);
            float minY = Mathf.Min(beforeRotateArg.min.y, afterRotateArg.min.y);

            float maxX = Mathf.Max(beforeRotateArg.max.x, afterRotateArg.max.x);
            float maxY = Mathf.Max(beforeRotateArg.max.y, afterRotateArg.max.y);

            areaRotate.SetMinMax(new Vector3(minX, minY, 0), new Vector3(maxX, maxY, 0));

            return areaRotate;
        }
    }
}

