using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tetris
{
    public class MapData
    {
        public MapData(ILevelsParams lvlsParamsArg, CalculateParams paramsArg)
        {
            _lvlsParams = lvlsParamsArg;
            _sizeBlock = paramsArg.SizeBoundsBlock;
        }


        public Vector2Int CountCells => _lvlsParams.Current.CountCells;
        public Vector3Int CountCells3 => new Vector3Int(CountCells.x, CountCells.y, 0);
        public Vector3Int MinCell => Vector3Int.zero;
        public Vector3Int MaxCell => CountCells3 + new Vector3Int(-1, -1, 0);
        public Vector3 MinPoint =>  (MinCell - _sizeBlock / 2).WithZ(0);
        public Vector3 MaxPoint =>  (MaxCell + _sizeBlock / 2).WithZ(0);
        public float TopByY => TopRow + _sizeBlock.y / 2;
        public float BottomByY => BottomRow - _sizeBlock.y / 2;
        public int TopRow => MaxCell.y;
        public int BottomRow => MinCell.y;
        public float SizeX => MaxCell.x + _sizeBlock.x;
        public float SizeY => MaxCell.y + _sizeBlock.y;
        public Vector3 CenterTop => Center.WithY(TopRow + _sizeBlock.y / 2);
        public Vector3 CenterBottom => Center.WithY(BottomRow - _sizeBlock.y / 2);
        public Vector3 CenterLeft => Center.WithX(MinCell.x - _sizeBlock.x / 2);
        public Vector3 CenterRight => Center.WithX(MaxCell.x + _sizeBlock.x / 2);
        public Vector3 SizeBlock => _sizeBlock;
        public Vector3 HalfSizeBlockXY => (_sizeBlock / 2).WithZ(0);
        public Vector2 HalfSizeBlock2D => new Vector2(_sizeBlock.x / 2, _sizeBlock.y / 2);
        public Vector3 Center => (MinPoint + MaxPoint) / 2;


        protected Vector3 _sizeBlock;
        private readonly ILevelsParams _lvlsParams;
    }
}

