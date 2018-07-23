using UnityEngine;
using Tetris.Interfaces;

namespace Tetris.Commands
{
    public class CommandObstructionAdd : ICommandObstructionAdd 
    {
        private Obstructions _obstructions;

        public CommandObstructionAdd(Obstructions obstructionsArg)
        {
            _obstructions = obstructionsArg;
        }
        public void Add(Vector3Int[] pointsArg)
        {
            _obstructions.Add(pointsArg);
        }
    }
}