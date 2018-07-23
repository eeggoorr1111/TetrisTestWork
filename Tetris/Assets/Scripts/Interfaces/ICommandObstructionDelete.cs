using UnityEngine;

namespace Tetris.Interfaces
{
    public interface ICommandObstructionDelete
    {
        bool Delete(Vector3Int pointArg);
    }
}
