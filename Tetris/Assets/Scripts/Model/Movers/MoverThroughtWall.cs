﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tetris
{
    public class MoverThroughtWall : Mover
    {
        public MoverThroughtWall(HeapFigures heapArg, Difficulty difficultyArg, Map mapArg) : base(heapArg, difficultyArg, mapArg) { }


        public override bool MoveToSide(bool toRightArg, BoundsFigure bounds)
        {
            throw new System.NotImplementedException();
        }
    }
}

