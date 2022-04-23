using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tetris
{
    public class MoverThroughtWall : Transformator
    {
        public MoverThroughtWall(HeapFigures heapArg, Difficulty difficultyArg, MapData mapArg, CalculateParams paramsArg, CheckCollisionHeap collisionHeapArg) : 
            base(heapArg, difficultyArg, mapArg, paramsArg, collisionHeapArg) { }


        public override bool MoveToSide(bool toRightArg, ColliderFigure bounds)
        {
            throw new System.NotImplementedException();
        }
        public override void Rotate(ColliderFigure collider)
        {
            throw new System.NotImplementedException();
        }
    }
}

