using UnityEngine;

namespace Tetris.Interfaces
{
    public interface ICommandObstructionIsExists
    {
        bool IsExists(Vector3Int pointArg);
    }
}
