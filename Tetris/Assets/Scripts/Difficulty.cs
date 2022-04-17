using UnityEngine;

namespace Tetris
{
    [System.Serializable]
    public class Difficulty
    {
        public float SpeedFalling => _speedFalling;
        public float SpeedFallingBoosted => _speedFallingBoost;
        public float TimeMoveToSide => _timeMoveToSide;


        
        [SerializeField] protected float _speedFalling;
        [SerializeField] protected float _speedFallingBoost;
        [SerializeField] protected float _timeMoveToSide;
    }
}

