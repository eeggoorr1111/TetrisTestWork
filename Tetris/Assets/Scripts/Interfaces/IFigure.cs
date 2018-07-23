using System;
using UnityEngine;
using System.Collections.Generic;

namespace Tetris.Interfaces
{
    public interface IFigure : IMovable, IRotatable, IDisposable
    {
        Vector3Int Size { get; }
        List<IFigure> InnerFigures { get; }
        List<Vector3Int> InnerPositions { get; }
        List<Vector3> InnerPositionsFact { get; }
    }
}
