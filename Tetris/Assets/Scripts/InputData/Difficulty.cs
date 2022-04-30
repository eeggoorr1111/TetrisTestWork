﻿using UnityEngine;

namespace Tetris
{
    [System.Serializable]
    public class Difficulty
    {
        public float SpeedFalling => _speedFalling;
        public float SpeedFallingBoosted => _speedFallingBoost;
        public float TimeMoveToSide => _timeMoveToSide;
        public float TimeRotate => _timeRotate;
        public Vector2Int CountCells => _countCells;
        


        [SerializeField] protected float _speedFalling;
        [SerializeField] protected float _speedFallingBoost;
        [SerializeField] protected float _timeMoveToSide;
        [SerializeField] protected float _timeRotate;
        [SerializeField] protected Vector2Int _countCells;
        
    }
}
