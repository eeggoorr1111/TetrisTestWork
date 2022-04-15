using UnityEngine;

namespace Tetris
{
    [System.Serializable]
    public class Difficulty
    {
        public float IntervalGenerate => _intervalGenerate;
        public float SpeedFalling => _speedFalling;
        public float SpeedFallingBoosted => _speedFallingBoost;
        public float SpeedMoveToSide => _speedMoveToSide;


        [SerializeField] protected float _intervalGenerate;
        [SerializeField] protected float _speedFalling;
        [SerializeField] protected float _speedFallingBoost;
        [SerializeField] protected float _speedMoveToSide;
    }
}

