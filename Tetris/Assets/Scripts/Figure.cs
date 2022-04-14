using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tetris
{
    public class Figure
    {
        public IReadOnlyList<Bounds> Bounds => _bounds;


        protected List<Bounds> _bounds; 


         
    }
}

