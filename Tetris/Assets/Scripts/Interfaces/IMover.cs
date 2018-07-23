using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tetris.Interfaces
{
    public delegate void ResistanceMove(Vector3Int unsuccessfulMoveArg, IMovable moveItemArg);
    public delegate void ItemMove(Vector3Int fromArg, Vector3Int toArg);

    public interface IMover
    {
        bool IsCanMove(Vector3Int moveToArg);
        bool IsCanMove(Vector3Int moveFromArg, Vector3Int moveParamsArg);

        Vector3 Move(IMovable itemArg, Vector3 moveParamsArg);
        Vector3Int Move(IMovable itemArg, Vector3Int moveParamsArg);
    }
}
