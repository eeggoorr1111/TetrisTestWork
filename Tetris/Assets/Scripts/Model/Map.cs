using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tetris
{
    public class Map
    {
        public Map(BoundsInt boundsArg)
        {
            _bounds = boundsArg;
        }


        public BoundsInt Bounds => _bounds;
        public float TopByY => _bounds.max.y;


        protected BoundsInt _bounds;
    }
}

