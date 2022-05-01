using UnityEngine;
using System.Collections.Generic;

namespace Tetris
{
    [System.Serializable]
    public class LevelParams
    {
        public float SpeedFalling => _speedFalling;
        public float SpeedFallingBoosted => _speedFallingBoost;
        public float TimeMoveToSide => _timeMoveToSide;
        public float TimeRotate => _timeRotate;
        public bool CanMoveThroughtWall => _canMoveThroughtWall;
        public Vector2Int CountCells => _countCells;
        public int CountCellsAll => _countCells.x * _countCells.y;
        public IReadOnlyList<FigureTemplate> FiguresTemplates => _templates;



        [Header("FALL FIGURE")]
        [SerializeField] private float _speedFalling;
        [SerializeField] private float _speedFallingBoost;

        [Header("MOVE AND ROTATE")]
        [SerializeField] private float _timeMoveToSide;
        [SerializeField] private float _timeRotate;
        [SerializeField] private bool _canMoveThroughtWall;

        [Header("MAP")]
        [SerializeField] private Vector2Int _countCells;

        [Header("FIGURES")]
        [SerializeField] private List<FigureTemplate> _templates;


        public LevelParams Clone()
        {
            LevelParams lvlParams = new LevelParams();

            lvlParams._speedFalling = _speedFalling;
            lvlParams._speedFallingBoost = _speedFallingBoost;

            lvlParams._timeMoveToSide = _timeMoveToSide;
            lvlParams._timeRotate = _timeRotate;
            lvlParams._canMoveThroughtWall = _canMoveThroughtWall;

            lvlParams._countCells = _countCells;

            List<FigureTemplate> templates = new List<FigureTemplate>();
            foreach (var template in _templates)
                templates.Add(template.Clone());

            lvlParams._templates = templates;

            return lvlParams;
        }
    }
}

