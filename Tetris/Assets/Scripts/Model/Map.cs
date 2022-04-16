using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tetris
{
    public class Map
    {
        public Map( BoundsInt boundsArg, Vector3 sizeBlockArg)
        {
            _bounds = boundsArg;
            _sizeBlock = sizeBlockArg;
        }


        public Vector3Int MinCell => _bounds.min;
        public Vector3 MinPoint =>  (_bounds.min - _sizeBlock).WithZ(_bounds.min.z);
        public Vector3Int MaxCell => _bounds.max;
        public float TopByY => TopCell + _sizeBlock.y / 2;
        public float BottomByY => BottomCell - _sizeBlock.y / 2;
        public int TopCell => _bounds.max.y;
        public int BottomCell => _bounds.min.y;
        public int SizeX => _bounds.max.x - _bounds.min.x;
        public int SizeY => _bounds.max.y - _bounds.min.y;
        public Vector3 CenterTop => _bounds.center.WithY(TopCell);
        public Vector3 CenterBottom => _bounds.center.WithY(BottomCell);
        public Vector3 CenterLeft => _bounds.center.WithX(_bounds.min.x);
        public Vector3 CenterRight => _bounds.center.WithX(_bounds.max.x);


        protected BoundsInt _bounds;
        protected Vector3 _sizeBlock;
    }
}

