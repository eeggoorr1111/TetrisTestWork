using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tetris.Global
{
    public static class Converter
    {
        /// <summary>
        /// Преобразует вектор из float в int округляя всегда в большую по модулю сторону
        /// </summary>
        /// <param name="vectorMoveArg"> Вектор который нужно преобразовать </param>
        /// <returns></returns>
        public static Vector3Int VectorMove(Vector3 vectorMoveArg)
        {
            int x = 0;
            int y = 0;
            int z = 0;


            if (vectorMoveArg.x != 0)
                x = (vectorMoveArg.x > 0) ? Mathf.CeilToInt(vectorMoveArg.x) : Mathf.FloorToInt(vectorMoveArg.x);

            if (vectorMoveArg.y != 0)
                y = (vectorMoveArg.y > 0) ? Mathf.CeilToInt(vectorMoveArg.y) : Mathf.FloorToInt(vectorMoveArg.y);

            if (vectorMoveArg.z != 0)
                z = (vectorMoveArg.z > 0) ? Mathf.CeilToInt(vectorMoveArg.z) : Mathf.FloorToInt(vectorMoveArg.z);

            return new Vector3Int(x, y, z);
        }
    }
}
