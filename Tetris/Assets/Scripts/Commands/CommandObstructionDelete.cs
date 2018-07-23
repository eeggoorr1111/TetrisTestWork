using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tetris.Interfaces;

namespace Tetris.Commands
{
    public class CommandObstructionDelete : ICommandObstructionDelete
    {
        private Obstructions _obstructions;

        public CommandObstructionDelete(Obstructions obstructionsArg)
        {
            _obstructions = obstructionsArg;
        }
        public bool Delete(Vector3Int pointArg)
        {
            return _obstructions.Delete(pointArg);
        }
    }
}
