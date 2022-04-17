﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Tetris
{
    public class FigureGenerator
    {
        public FigureGenerator( IReadOnlyList<FigureTemplate> templatesArg,
                                Map mapArg)
        {
            _templates = templatesArg;
            _map = mapArg;
            _sumWeights = 0;

            foreach (var template in _templates)
                _sumWeights += template.WeightGenerate;
        }


        protected IReadOnlyList<FigureTemplate> _templates;
        protected float _sumWeights = 0;
        protected Map _map;


        public FigureModel NewFigure()
        {
            if (_templates.Count == 1)
                return new FigureModel(_templates[0], 0, GetSpawnPoint(_templates[0]), _map.SizeBlock);

            float rand = Random.Range(0, _sumWeights);
            float prevWeights = 0f;

            for (int i = 0; i < _templates.Count; i++)
            {
                prevWeights += _templates[i].WeightGenerate;
                if (rand < prevWeights)
                    return new FigureModel(_templates[i], i, GetSpawnPoint(_templates[i]), _map.SizeBlock);
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
    }
}

