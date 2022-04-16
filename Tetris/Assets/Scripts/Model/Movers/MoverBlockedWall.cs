using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tetris
{
    public class MoverBlockedWall : Mover
    {
        public MoverBlockedWall(HeapFigures heapArg, Difficulty difficultyArg, Map mapArg) : base(heapArg, difficultyArg, mapArg) { }


        public override bool ToMove(bool toRightArg)
        {
            return false;
        }
    }
}

