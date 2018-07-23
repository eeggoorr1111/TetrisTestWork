using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tetris.Interfaces
{
    public interface IPlayer
    {
        void HandlerAction(IMover moverArg, IRotator rotatorArg);
    }
}
    
