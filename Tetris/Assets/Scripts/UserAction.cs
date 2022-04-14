using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tetris
{
    public struct UserAction
    {
        public static UserAction ToMove(Vector3Int moveArg)
        {
            return new UserAction(moveArg, RotateKey.None);
        }
        public static UserAction ToRotate(RotateKey rotateArg)
        {
            return new UserAction(Vector3Int.zero, rotateArg);
        }
        public UserAction(Vector3Int moveArg, RotateKey rotateArg)
        {
            _move = moveArg;
            _rotate = rotateArg;
        }


        public Vector3Int Move => _move;
        public RotateKey Rotate => _rotate;


        private Vector3Int _move;
        private RotateKey _rotate;
    }
}

