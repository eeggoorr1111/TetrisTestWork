using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tetris
{
    public class FigureModel
    {
        public static readonly Vector3 SizeBlock = new Vector3(1f, 1f, 0.1f);
        public static Vector3 HalfSizeBlock => SizeBlock / 2;


        public static Bounds GetBounds(FigureTemplate templateArg)
        {
            Vector2Int min = new Vector2Int(int.MaxValue, int.MaxValue);
            Vector2Int max = new Vector2Int(int.MinValue, int.MinValue);
            Bounds bounds = new Bounds();

            foreach (var block in templateArg.Blocks)
            {
                if (block.x < min.x)
                    min.x = block.x;

                if (block.y < min.y)
                    min.y = block.y;

                if (block.x > max.x)
                    max.x = block.x;

                if (block.y > max.y)
                    max.y = block.y;
            }

            bounds.SetMinMax(   new Vector3(min.x - HalfSizeBlock.x, min.y - HalfSizeBlock.y, 0), 
                                new Vector3(max.x + HalfSizeBlock.x, max.y + HalfSizeBlock.y, 0));
            return bounds;
        }


        public FigureModel(FigureTemplate templateArg, int idxTemplateArg, Vector3 posArg)
        {
            _bounds = GetBounds(templateArg);
            _bounds.center = posArg;
            _idxTemplate = idxTemplateArg;
            _boundsBottom = new List<Bounds>();

            FillBoundsBlocks(templateArg.Blocks);
            FillIdxsOfBottomBlocks(templateArg.Blocks);
        }


        public Bounds Bounds => _bounds;
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
        protected void FillBoundsBlocks(IReadOnlyList<Vector2Int> blocksArg)
        {
            _boundsBlocks = new List<Bounds>();
            foreach (var block in blocksArg)
            {
                Vector3 pos = new Vector3(block.x, block.y, 0);
                Bounds bounds = new Bounds(pos + _bounds.center, SizeBlock);

                _boundsBlocks.Add(bounds);
            }
        }
    }
}

