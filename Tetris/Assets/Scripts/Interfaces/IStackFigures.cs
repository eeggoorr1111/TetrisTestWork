using System;

namespace Tetris.Interfaces
{
    public interface IStackFigures : IDisposable
    {
        int MaxArea { get; }
        int Add(IFigure[] figuresArg);
    }
}

    