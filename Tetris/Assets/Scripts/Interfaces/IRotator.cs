using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tetris.Interfaces
{
    public interface IRotator
    {
        Vector3 CalculateCenter(Vector3[] vectorsArg, bool isByClockArg);
        Vector3Int CalculateCenter(Vector3Int[] vectorsArg, bool isByClockArg);

        bool IsCanRotate(Vector3Int curPositionArg, Vector3Int centerArg, bool isByClockArg);

        Vector3 Rotate(Vector3 curPositionArg, Vector3 centerArg, bool isByClockArg);
        Vector3Int Rotate(Vector3Int curPositionArg, Vector3Int centerArg, bool isByClockArg);
    }
}
