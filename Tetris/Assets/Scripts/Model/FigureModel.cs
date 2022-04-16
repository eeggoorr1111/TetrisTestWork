using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tetris
{
    public class FigureModel
    {
        public FigureModel(FigureTemplate templateArg, int idxTemplateArg, Vector3 posArg, Vector3 sizeBlockArg)
        {
            Bounds bounds = GetBounds(templateArg, sizeBlockArg);
            Vector3 localCenter = bounds.center;
            Vector3 pivotWorld = posArg;

            _bounds = bounds;
            _bounds.center = pivotWorld + bounds.center;
            _deltaPivot = pivotWorld - _bounds.center;
            _idxTemplate = idxTemplateArg;
            _boundsBottom = new List<Bounds>();

            FillBoundsBlocks(templateArg.Blocks, sizeBlockArg);
            FillIdxsOfBottomBlocks(templateArg.Blocks);
        }


        public Bounds Bounds => _bounds;
        public Vector3 Pivot => _bounds.center + _deltaPivot;
        public IReadOnlyList<Bounds> BoundsBlocks => _boundsBlocks;
        public IReadOnlyList<Bounds> BoundsOfBottom
        {
            get 
            {
                _boundsBottom.Clear();
                foreach (var index in _boundsBottomIdxs)
                    _boundsBottom.Add(_boundsBlocks[index]);

                return _boundsBottom;
            }
        }
        public int IdxTemplate => _idxTemplate;


        protected List<Bounds> _boundsBlocks;
        protected List<Bounds> _boundsBottom;
        protected List<int> _boundsBottomIdxs;
        protected Bounds _bounds;
        protected int _idxTemplate;
        protected Vector3 _deltaPivot;


        public bool MoveToSide(Mover moverArg)
        {
            return false;
        }
        public bool ToFall(Mover moverArg, bool boostedFallArg)
        {
            return moverArg.ToFall(boostedFallArg, BoundsOfBottom, _boundsBlocks, ref _bounds);
        }


        protected void FillIdxsOfBottomBlocks(IReadOnlyList<Vector2Int> blocksArg)
        {
            Dictionary<int, int> idxsBlocks = new Dictionary<int, int>();

            for (int i = 0; i < blocksArg.Count; i++)
            {
                int xPos = blocksArg[i].x;
                if (idxsBlocks.ContainsKey(xPos))
                {
                    int idxLowerBlock = idxsBlocks[xPos];
                    if (blocksArg[i].y < blocksArg[idxLowerBlock].y)
                        idxsBlocks[xPos] = i;
                }
                else
                    idxsBlocks[xPos] = i;
            }

            _boundsBottomIdxs = new List<int>();
            foreach (var pair in idxsBlocks)
                _boundsBottomIdxs.Add(pair.Value);
        }
        protected void FillBoundsBlocks(IReadOnlyList<Vector2Int> blocksArg, Vector3 sizeBlockArg)
        {
            _boundsBlocks = new List<Bounds>();
            foreach (var block in blocksArg)
            {
                Vector3 pos = new Vector3(block.x, block.y, 0);
                Bounds boundsBlock = new Bounds(pos + _bounds.center, sizeBlockArg);

                _boundsBlocks.Add(boundsBlock);
            }
        }
        protected Bounds GetBounds(FigureTemplate templateArg, Vector3 sizeBlockArg)
        {
            Vector3 min, max;
            Bounds bounds = new Bounds();

            GetMinMax(templateArg, sizeBlockArg, out min, out max);
            bounds.SetMinMax(min, max);

            return bounds;
        }
        protected void GetMinMax(FigureTemplate templateArg, Vector3 sizeBlockArg, out Vector3 min, out Vector3 max)
        {
            Vector2Int minInt = new Vector2Int(int.MaxValue, int.MaxValue);
            Vector2Int maxInt = new Vector2Int(int.MinValue, int.MinValue);
            Vector3 halfSizeBlock = sizeBlockArg / 2;

            foreach (var block in templateArg.Blocks)
            {
                if (block.x < minInt.x)
                    minInt.x = block.x;

                if (block.y < minInt.y)
                    minInt.y = block.y;

                if (block.x > maxInt.x)
                    maxInt.x = block.x;

                if (block.y > maxInt.y)
                    maxInt.y = block.y;
            }

            min = new Vector3(minInt.x - halfSizeBlock.x, minInt.y - halfSizeBlock.y, -halfSizeBlock.z);
            max = new Vector3(maxInt.x + halfSizeBlock.x, maxInt.y + halfSizeBlock.y, halfSizeBlock.z);
        }
    }
}

