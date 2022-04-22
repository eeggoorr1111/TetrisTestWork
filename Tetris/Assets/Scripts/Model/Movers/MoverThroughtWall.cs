using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tetris
{
    public class MoverThroughtWall : Mover
    {
        public MoverThroughtWall(HeapFigures heapArg, Difficulty difficultyArg, MapData mapArg, CalculateParams paramsArg) : base(heapArg, difficultyArg, mapArg, paramsArg) { }


        public override bool MoveToSide(bool toRightArg, ColliderFigure bounds)
        {
            throw new System.NotImplementedException();
        }
    }
}

