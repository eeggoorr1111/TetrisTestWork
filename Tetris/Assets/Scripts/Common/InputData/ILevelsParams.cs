using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tetris
{
    public interface ILevelsParams
    {
        LevelParams Current { get; }
        int MaxLevel { get; }
    }
}

