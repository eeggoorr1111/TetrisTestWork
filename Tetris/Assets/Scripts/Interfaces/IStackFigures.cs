using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tetris.Interfaces
{
    public interface IStackFigures
    {
        int MaxArea { get; }
        int Add(IFigure[] figuresArg);
    }
}

    