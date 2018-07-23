using Tetris.GameObjects;

namespace Tetris.Interfaces
{
    public interface IFigureGenerator
    {
        IFigure NewFigure(IFactoryFigure factoryArg);
    }
}
