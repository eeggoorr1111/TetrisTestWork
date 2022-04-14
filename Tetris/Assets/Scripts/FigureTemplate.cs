using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tetris
{
    [CreateAssetMenu(fileName = "Figure", menuName = "Tetris/CreateFigure")]
    public class FigureTemplate : ScriptableObject
    {
        public IReadOnlyList<Vector2Int> Blocks => _blocks;

        [SerializeField] protected List<Vector2Int> _blocks;
    }
}

