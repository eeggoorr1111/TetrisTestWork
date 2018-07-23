using UnityEngine;

namespace Tetris.Interfaces
{
    public interface ICommandObstructionReplace
    {
        void Replace(Vector3Int fromArg, Vector3Int toArg);
    }
}


