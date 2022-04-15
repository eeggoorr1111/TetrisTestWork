using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tetris
{
    public class MoverThroughtWall : Mover
    {
        public MoverThroughtWall(HeapFigures heapArg, Difficulty difficultyArg) : base(heapArg, difficultyArg) { }


        public override bool ToMove(bool toRightArg)
        {
            return false;
        }
    }
}

