using UnityEngine;
using Tetris.Interfaces;

namespace Tetris.Commands
{
    public class CommandObstructionIsExists : ICommandObstructionIsExists
    {
        private Obstructions _obstructions;

        public CommandObstructionIsExists(Obstructions obstructionsArg)
        {
            _obstructions = obstructionsArg;
        }
        public bool IsExists(Vector3Int pointArg)
        {
            return _obstructions.IsExists(pointArg);
        }
    }
}
