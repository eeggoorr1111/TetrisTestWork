using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Tetris
{
    public class Rotator
    {
        protected Tween _rotate; 


        public void Rotate(ColliderFigure boundsArg)
        {
            if (_rotate.IsActive())
                return;


        }
    }
}

