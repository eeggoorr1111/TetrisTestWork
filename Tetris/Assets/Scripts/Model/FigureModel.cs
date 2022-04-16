using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tetris
{
    public class FigureModel
    {
        public FigureModel(FigureTemplate templateArg, int idxTemplateArg, Vector2Int cellSpawnArg, Vector3 sizeBlockArg)
        {
            Vector3 cellSpawn = new Vector3(cellSpawnArg.x, cellSpawnArg.y, 0);
            Bounds boundsFigure = GetBounds(templateArg, sizeBlockArg).WithDeltaPos(cellSpawn);

            _deltaPivot = cellSpawn - boundsFigure.center;
            _idxTemplate = idxTemplateArg;

            FillBoundsBlocks(templateArg.Blocks, sizeBlockArg, boundsFigure, boundsFigure.center + _deltaPivot);

            _blocks = new List<Vector2Int>();
            foreach (var block in templateArg.Blocks)
                _blocks.Add(block);
        }


        public IReadOnlyList<Vector2Int> Blocks => _blocks;
        public Bounds Bounds => _bounds.Figure;
        public Vector3 Pivot => _bounds.Figure.center + _deltaPivot;
        public IReadOnlyList<Bounds> BoundsBlocks => _bounds.Blocks;
        public IReadOnlyList<Bounds> BoundsOfBottom => _bounds.BlocksBottom;
        public int IdxTemplate => _idxTemplate;


        protected BoundsFigure _bounds;
        protected int _idxTemplate;
        protected Vector3 _deltaPivot;
        protected List<Vector2Int> _blocks;


        public bool MoveToSide(Mover moverArg, bool toRightArg)
        {
            return moverArg.MoveToSide(toRightArg, _bounds);
        }
        public bool ToFall(Mover moverArg, bool boostedFallArg)
        {
            return moverArg.ToFall(boostedFallArg, _bounds);
        }


        protected void FillBoundsBlocks(IReadOnlyList<Vector2Int> blocksArg, Vector3 sizeBlockArg, Bounds boundsArg, Vector3 pivotArg)
        {
            List<Bounds> boundsBlocks = new List<Bounds>();
            foreach (var block in blocksArg)
            {
                Vector3 pos = new Vector3(block.x, block.y, 0);
                Bounds boundsBlock = new Bounds(pos + pivotArg, sizeBlockArg);

                boundsBlocks.Add(boundsBlock);
            }

            _bounds = new BoundsFigure(boundsArg, boundsBlocks);
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

