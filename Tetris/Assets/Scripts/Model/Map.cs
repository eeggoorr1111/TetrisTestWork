using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tetris
{
    public class Map
    {
        /// <summary>
        /// Если тут задать в качесвте sizeBlock уменьшенные блоки, то границы UI будут криво отображаться. 
        /// Нужно определиться нужны ли нам разные sizeBlock размеры для представленяи и модели. Если не нужны,
        /// то не париться и сделать единую переменную для размера блока. Если не получиться,
        /// тогда надо думать разделять сущность Map на сущност для View и для Model
        /// </summary>
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
        public Vector3 CenterTop => _bounds.center.WithY(TopCell + _sizeBlock.y / 2);
        public Vector3 CenterBottom => _bounds.center.WithY(BottomCell - _sizeBlock.y / 2);
        public Vector3 CenterLeft => _bounds.center.WithX(_bounds.min.x - _sizeBlock.x / 2);
        public Vector3 CenterRight => _bounds.center.WithX(_bounds.max.x + _sizeBlock.x / 2);


        protected BoundsInt _bounds;
        protected Vector3 _sizeBlock;
    }
}

