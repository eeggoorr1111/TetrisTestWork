using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tetris
{
    [CreateAssetMenu(fileName = "Figure", menuName = "Tetris/CreateFigureTemplate")]
    public class FigureTemplateSO : ScriptableObject
    {
        public FigureTemplate Template => _template;

        [SerializeField] protected FigureTemplate _template;
    }
}

