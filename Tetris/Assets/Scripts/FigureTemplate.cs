using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tetris
{
    [CreateAssetMenu(fileName = "Figure", menuName = "Tetris/CreateFigure")]
    public class FigureTemplate : ScriptableObject
    {
        public IReadOnlyList<Vector2Int> Blocks => _blocks;
        public float WeightGenerate => _weightGenerate;


        [SerializeField] protected List<Vector2Int> _blocks;
        [SerializeField] protected float _weightGenerate;
    }
}

