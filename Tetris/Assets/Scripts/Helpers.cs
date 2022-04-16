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
        public static Vector3 WithX(this Vector3 vector, float xArg)
        {
            return new Vector3(xArg, vector.y, vector.z);
        }
        public static Vector3 WithY(this Vector3 vector, float yArg)
        {
            return new Vector3(vector.x, yArg, vector.z);
        }
        public static Vector3 WithZ(this Vector3 vector, float zArg)
        {
            return new Vector3(vector.x, vector.y, zArg);
        }
    }
}

