using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tetris.Interfaces
{
    public interface IFactoryFigure
    {
        IFigure Create(Vector3Int positionArg);
    }

}
