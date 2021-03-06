using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tetris
{
    [System.Serializable]
    public class FigureTemplate
    {
        public IReadOnlyList<Vector2Int> Blocks => _blocks;
        public float WeightGenerate => _weightGenerate;


        [Tooltip("For optimization, first of all, set extrema blocks.That is, blocks that have the largest or smallest x or the largest or smallest y")]
        [SerializeField] protected List<Vector2Int> _blocks;
        [SerializeField] protected float _weightGenerate;


        public Vector2Int[] GetNewArrayBlocks()
        {
            return _blocks.ToArray();
        }
        public FigureTemplate Clone()
        {
            FigureTemplate template = new FigureTemplate();
            List<Vector2Int> blocks = new List<Vector2Int>();

            foreach (var block in _blocks)
                blocks.Add(block);

            template._blocks = blocks;
            template._weightGenerate = _weightGenerate;

            return template;
        }
    }
}

