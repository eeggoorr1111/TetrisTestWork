using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tetris.Global
{
    public class Vector3IntWrap
    {
        Vector3Int _vector;

        public int x {
            set { _vector.x = value; }
            get { return _vector.x; }
        }
        public int y
        {
            set { _vector.y = value; }
            get { return _vector.y; }
        }
        public int z
        {
            set { _vector.z = value; }
            get { return _vector.z; }
        }

        /// <summary>
        /// Класс обертка вектора, для работы с ним по ссылке
        /// </summary>
        /// <param name="vectorArg"></param>
        public Vector3IntWrap(Vector3Int vectorArg)
        {
            _vector = vectorArg;
        }
        public Vector3Int GetVector()
        {
            return _vector;
        }
    }
}
