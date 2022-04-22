using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tetris
{
    public class MapData
    {
        /// <summary>
        /// Если тут задать в качесвте sizeBlock уменьшенные блоки, то границы UI будут криво отображаться. 
        /// Нужно определиться нужны ли нам разные sizeBlock размеры для представленяи и модели. Если не нужны,
        /// то не париться и сделать единую переменную для размера блока. Если не получиться,
        /// тогда надо думать разделять сущность Map на сущност для View и для Model
        /// </summary>
        public MapData( Difficulty difficultyArg, CalculateParams paramsArg)
        {
            _sizeMap = difficultyArg.SizeMap;
            _bounds = new BoundsInt();
            _bounds.SetMinMax(Vector3Int.zero, new Vector3Int(_sizeMap.x - 1, _sizeMap.y - 1, 0));
            _sizeBlock = paramsArg.SizeBoundsBlock;
        }


        public Vector3Int MinCell => _bounds.min;
        public Vector3 MinPoint =>  (_bounds.min - _sizeBlock / 2).WithZ(_bounds.min.z);
        public Vector3 MaxPoint =>  (_bounds.max + _sizeBlock / 2).WithZ(_bounds.max.z);
        public Vector3Int MaxCell => _bounds.max;
        public Vector2Int SizeMap => _sizeMap;
        public float TopByY => TopCell + _sizeBlock.y / 2;
        public float BottomByY => BottomCell - _sizeBlock.y / 2;
        public int TopCell => _bounds.max.y;
        public int BottomCell => _bounds.min.y;
        public float SizeX => _bounds.max.x + _sizeBlock.x;
        public float SizeY => _bounds.max.y + _sizeBlock.y;
        public Vector3 CenterTop => _bounds.center.WithY(TopCell + _sizeBlock.y / 2);
        public Vector3 CenterBottom => _bounds.center.WithY(BottomCell - _sizeBlock.y / 2);
        public Vector3 CenterLeft => _bounds.center.WithX(_bounds.min.x - _sizeBlock.x / 2);
        public Vector3 CenterRight => _bounds.center.WithX(_bounds.max.x + _sizeBlock.x / 2);
        public Vector3 SizeBlock => _sizeBlock;
        public Vector3 HalfSizeBlockXY => (_sizeBlock / 2).WithZ(0);


        protected BoundsInt _bounds;
        protected Vector3 _sizeBlock;
        protected Vector2Int _sizeMap;
    }
}

