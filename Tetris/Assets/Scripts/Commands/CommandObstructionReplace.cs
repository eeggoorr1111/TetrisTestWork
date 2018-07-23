using UnityEngine;
using Tetris.Interfaces;

namespace Tetris.Commands
{
    public class CommandObstructionReplace : ICommandObstructionReplace
    {
        private Obstructions _obstructions;

        public CommandObstructionReplace(Obstructions obstructionsArg)
        {
            _obstructions = obstructionsArg;
        }
        public void Replace(Vector3Int fromArg, Vector3Int toArg)
        {
            _obstructions.Replace(fromArg, toArg);
        }
    }
}

