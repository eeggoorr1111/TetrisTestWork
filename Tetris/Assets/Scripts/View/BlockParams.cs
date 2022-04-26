using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tetris
{
    [System.Serializable]
    public class BlockParams
    {
        public Material Material => _material;


        [SerializeField] protected Material _material;
    }
}

