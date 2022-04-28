using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tetris
{
    [System.Serializable]
    public class CalculateParams
    {
        public float AllowedError => _allowedError;
        public Vector3 SizeBoundsBlock => _sizeBoundsBlock;


        [SerializeField] protected float _allowedError;
        [SerializeField] protected Vector3 _sizeBoundsBlock = new Vector3(1f, 1f, 1f);
    }
}

