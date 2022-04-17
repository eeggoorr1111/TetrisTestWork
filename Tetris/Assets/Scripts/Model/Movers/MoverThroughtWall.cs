using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tetris
{
    public class MoverThroughtWall : Mover
    {
        public MoverThroughtWall(HeapFigures heapArg, Difficulty difficultyArg, Map mapArg, CalculateParams paramsArg) : base(heapArg, difficultyArg, mapArg, paramsArg) { }


        public override bool MoveToSide(bool toRightArg, BoundsFigure bounds)
        {
            throw new System.NotImplementedException();
        }
    }
}

