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
        public static bool WithItems<T>(this IReadOnlyCollection<T> collection)
        {
            if (collection == null || collection.Count == 0)
                return false;

            return true;
        }
        public static bool Contains<T>(this IReadOnlyCollection<T> collection, T itemArg)
        {
            foreach (var item in collection)
                if (item.Equals(itemArg))
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
        public static Bounds GetBounds(IReadOnlyList<Vector2Int> blocksArg, Vector3 sizeBlockArg)
        {
            Vector2Int minInt = new Vector2Int(int.MaxValue, int.MaxValue);
            Vector2Int maxInt = new Vector2Int(int.MinValue, int.MinValue);
            Vector3 halfSizeBlock = sizeBlockArg / 2;

            foreach (var block in blocksArg)
            {
                if (block.x < minInt.x)
                    minInt.x = block.x;

                if (block.y < minInt.y)
                    minInt.y = block.y;

                if (block.x > maxInt.x)
                    maxInt.x = block.x;

                if (block.y > maxInt.y)
                    maxInt.y = block.y;
            }

            Vector3 min = new Vector3(minInt.x - halfSizeBlock.x, minInt.y - halfSizeBlock.y, -halfSizeBlock.z);
            Vector3 max = new Vector3(maxInt.x + halfSizeBlock.x, maxInt.y + halfSizeBlock.y, halfSizeBlock.z);
            Bounds bounds = new Bounds();

            bounds.SetMinMax(min, max);

            return bounds;
        }
        public static Bounds GetBounds(IReadOnlyList<Bounds> blocksArg)
        {
            float halfSizeZ = blocksArg[0].size.z / 2;
            Vector3 min = new Vector3(float.MaxValue, float.MaxValue, -halfSizeZ);
            Vector3 max = new Vector3(float.MinValue, float.MinValue, halfSizeZ);

            foreach (var block in blocksArg)
            {
                if (block.min.x < min.x)
                    min.x = block.min.x;

                if (block.min.y < min.y)
                    min.y = block.min.y;

                if (block.max.x > max.x)
                    max.x = block.max.x;

                if (block.max.y > max.y)
                    max.y = block.max.y;
            }

            Bounds bounds = new Bounds();
            bounds.SetMinMax(min, max);

            return bounds;
        }
    }
}

