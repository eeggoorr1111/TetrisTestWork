using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tetris.View
{
    public interface IFigure
    {
        IReadOnlyList<Block> Blocks { get; }


        void StartCustom();
        void Transform(Vector3 posArg, Quaternion rotateArg);
        void NewFigrue(Vector3 posArg, FigureTemplate templateArg);
    }
}

