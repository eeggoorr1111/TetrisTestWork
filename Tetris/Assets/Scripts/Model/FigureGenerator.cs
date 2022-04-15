using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
                return new FigureModel(_templates[0], 0, GetSpawnPoint(_templates[0]));

            float rand = Random.Range(0, _sumWeights);
            float prevWeights = 0f;

            for (int i = 0; i < _templates.Count; i++)
            {
                prevWeights += _templates[i].WeightGenerate;
                if (rand < prevWeights)
                    return new FigureModel(_templates[i], i, GetSpawnPoint(_templates[i]));
            }

            return null;
        }


        protected Vector3 GetSpawnPoint(FigureTemplate templateArg)
        {
            Vector3 sizeFigure = FigureModel.GetBounds(templateArg).size;
            float halfSizeX = sizeFigure.x / 2;
            float halfSizeY = sizeFigure.y / 2;
            float posX = Random.Range(  _map.Bounds.min.x + halfSizeX,
                                        _map.Bounds.max.x - halfSizeX);

            return new Vector3(posX, _map.TopByY + halfSizeY, 0);
        }
    }
}

