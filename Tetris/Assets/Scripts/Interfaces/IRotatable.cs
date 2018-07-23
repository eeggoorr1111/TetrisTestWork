using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tetris.Interfaces
{
    public interface IRotatable
    {
        bool IsObstruction { get; set; }

        bool Rotate(IRotator rotatorArg, bool isByClockArg);
        bool Rotate(IRotator rotatorArg, Vector3 center, bool isByClockArg);
        bool Rotate(IRotator rotatorArg, Vector3Int center, bool isByClockArg);
    }
}
