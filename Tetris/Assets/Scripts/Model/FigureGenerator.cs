using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Tetris
{
    public class FigureGenerator
    {
        public FigureGenerator( IReadOnlyList<FigureTemplate> templatesArg,
                                MapData mapArg,
                                Rotator rotatorArg)
        {
            _templates = templatesArg;
            _map = mapArg;
            _sumWeights = 0;
            _rotator = rotatorArg;

            foreach (var template in _templates)
                _sumWeights += template.WeightGenerate;
        }


        protected IReadOnlyList<FigureTemplate> _templates;
        protected float _sumWeights = 0;
        protected MapData _map;
        protected Rotator _rotator;


        public FigureModel NewFigure()
        {
            if (_templates.Count == 1)
                return CreateFigure(0);

            float rand = Random.Range(0, _sumWeights);
            float prevWeights = 0f;

            for (int i = 0; i < _templates.Count; i++)
            {
                prevWeights += _templates[i].WeightGenerate;
                if (rand < prevWeights)
                    return CreateFigure(i);
            }

            return null;
        }


        protected Vector2Int GetSpawnPoint(FigureTemplate templateArg)
        {
            int fromZeroBlockToLeft = 0;
            int fromZeroBlockToRight = 0;
            int fromZeroBlockToBottom = 0;

            foreach (var block in templateArg.Blocks)
            {
                if (block.x > fromZeroBlockToRight)
                    fromZeroBlockToRight = block.x;

                if (block.x < fromZeroBlockToLeft)
                    fromZeroBlockToLeft = block.x;

                if (block.y < fromZeroBlockToBottom)
                    fromZeroBlockToBottom = block.y;
            }
                
            int cellX = Random.Range(  _map.MinCell.x + Mathf.Abs(fromZeroBlockToLeft),
                                       _map.MaxCell.x - fromZeroBlockToRight + 1);

            return new Vector2Int(cellX, _map.TopCell + Mathf.Abs(fromZeroBlockToBottom) + 1);
        }
        protected FigureModel CreateFigure(int idxTemplateArg)
        {
            FigureTemplate template = _templates[idxTemplateArg];
            Vector2Int cellSpawn = GetSpawnPoint(template);
            Vector3 pointSpawn = new Vector3(cellSpawn.x, cellSpawn.y, 0);
            Bounds boundsFigure = Helpers.GetBounds(template.Blocks, _map.SizeBlock).WithDeltaPos(pointSpawn);
            Vector3 deltaPivot = pointSpawn - boundsFigure.center;
            Bounds[] blocks = GetBlocks(template.Blocks, _map.SizeBlock, boundsFigure.center + deltaPivot);
            int idxPivot = GetIdxPivotBlock(template.Blocks);
            ColliderFigure collider = new ColliderFigure(boundsFigure, blocks, template.GetNewArrayBlocks(), idxPivot);

            return new FigureModel(_rotator, idxTemplateArg, collider);
        }
        protected int GetIdxPivotBlock(IReadOnlyList<Vector2Int> blocksArg)
        {
            for (int i = 0; i < blocksArg.Count; i++)
                if (blocksArg[i].x == 0 && blocksArg[i].y == 0)
                    return i;

            return 0;
        }
        protected Bounds[] GetBlocks(IReadOnlyList<Vector2Int> blocksArg, Vector3 sizeBlockArg, Vector3 pivotArg)
        {
            Bounds[] boundsBlocks = new Bounds[blocksArg.Count];
            for (int i = 0; i < blocksArg.Count; i++)
            {
                Vector2Int block = blocksArg[i];
                Vector3 pos = new Vector3(block.x, block.y, 0);
                Bounds boundsBlock = new Bounds(pos + pivotArg, sizeBlockArg);

                boundsBlocks[i] = boundsBlock;
            }

            return boundsBlocks;
        }
        protected int[] GetBottomIdxs(IReadOnlyList<Bounds> blocksArg, Bounds boundsArg)
        {
            Dictionary<int, int> idxsBlocks = new Dictionary<int, int>();

            for (int i = 0; i < blocksArg.Count; i++)
            {
                int xPos = Mathf.RoundToInt(blocksArg[i].center.x);
                if (idxsBlocks.ContainsKey(xPos))
                {
                    int idxLowerBlock = idxsBlocks[xPos];
                    if (blocksArg[i].center.y < blocksArg[idxLowerBlock].center.y)
                        idxsBlocks[xPos] = i;
                }
                else
                    idxsBlocks[xPos] = i;
            }

            int index = 0;

            int[] bottomBlocksIdx = new int[idxsBlocks.Count];
            foreach (var pair in idxsBlocks)
            {
                bottomBlocksIdx[index] = pair.Value;
                index++;
            }

            return bottomBlocksIdx;
        }
    }
}

