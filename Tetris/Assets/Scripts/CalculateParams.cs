using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tetris
{
    [System.Serializable]
    public class CalculateParams
    {
        public float AllowedError => _allowedError;

        [SerializeField] protected float _allowedError;
    }
}

