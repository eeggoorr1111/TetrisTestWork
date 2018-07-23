using UnityEngine;

namespace Tetris.Interfaces
{
    public interface ICommandObstructionAdd
    {
        void Add(Vector3Int[] pointArg);
    }
}