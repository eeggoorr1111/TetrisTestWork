using UnityEngine;
using Tetris.Transform;

namespace Tetris.Interfaces
{
    public interface IMovable
    {
        Vector3 PositionFact { get; }
        Vector3Int Position { get; }

        bool IsObstruction { get; set; }
        bool Move(IMover moverArg, Vector3 moveParamsArg);
        bool Move(IMover moverArg, Vector3Int moveParamsArg);
    }
}
