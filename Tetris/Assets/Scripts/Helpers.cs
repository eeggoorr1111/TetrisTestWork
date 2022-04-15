using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tetris
{
    public static class Helpers
    {
        public static bool IsValidIndex<T>(this IReadOnlyCollection<T> collection, int indexArg)
        {
            if (collection.IsEmpty() || indexArg < 0 || indexArg >= collection.Count)
                return false;

            return true;
        }
        public static bool IsEmpty<T>(this IReadOnlyCollection<T> collection)
        {
            if (collection == null || collection.Count == 0)
                return true;

            return false;
        }
        public static Bounds WithDeltaPos(this Bounds boundsArg, Vector3 deltaArg)
        {
            return new Bounds(boundsArg.center + deltaArg, boundsArg.size);
        }
    }
}

