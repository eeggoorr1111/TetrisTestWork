using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tetris.Interfaces;
using Tetris.Global;
using System.Linq;

namespace Tetris.Transform
{
    public class Rotator : IRotator
    {
        private Vector3Int _sizeMap;
        private IMover _mover;
        private GameObject _empty;
        private UnityEngine.Transform _transform;

        /// <summary>
        /// Осуществляет вращение объектов
        /// </summary>
        /// <param name="moverArg"> Класс для передвижения объектов </param>
        /// <param name="sizeMapArg"> Размер карты </param>
        public Rotator(IMover moverArg, Vector3Int sizeMapArg)
        {
            _sizeMap = sizeMapArg;
            _mover = moverArg;
            _empty = new GameObject();
            GameObject.DontDestroyOnLoad(_empty);
            _transform = _empty.transform;
        }

        /// <summary> Рассчитывает центр для группы объектов </summary>
        /// <param name="transformArg"> Изначальная позиция элемента </param>
        /// <param name="isByClockArg"> true - поворот по часовой стрелке </param>
        /// <returns> Возвращает центр вращения для последнего объекта </returns>
        public Vector3 CalculateCenter(Vector3[] vectorsArg, bool isByClockArg)
        {
            Vector3 pointRotate = Vector3.down;
            Vector3[] tempArr;
            float minY;
            float extremeX;

            minY = vectorsArg.Min((point) => point.y);
            tempArr = vectorsArg.Where(point => point.y == minY).ToArray();
            if (isByClockArg)
                extremeX = tempArr.Min(point => point.x);
            else
                extremeX = tempArr.Max(point => point.x);

            foreach (var vector in tempArr)
            {
                if (vector.x == extremeX && vector.y == minY)
                    pointRotate = new Vector3(vector.x, vector.y, vector.z);
            }

            return pointRotate;
        }
        public Vector3Int CalculateCenter(Vector3Int[] vectorsArg, bool isByClockArg)
        {
            Vector3Int pointRotate = Vector3Int.down;
            Vector3Int[] tempArr;
            float minY;
            float extremeX;

            minY = vectorsArg.Min((point) => point.y);
            tempArr = vectorsArg.Where(point => point.y == minY).ToArray();
            if (isByClockArg)
                extremeX = tempArr.Min(point => point.x);
            else
                extremeX = tempArr.Max(point => point.x);

            foreach (var vector in tempArr)
            {
                if (vector.x == extremeX && vector.y == minY)
                    pointRotate = new Vector3Int(vector.x, vector.y, vector.z);
            }

            return pointRotate;
        }

        /// <summary> Проверяет можно ли осуществить поворот </summary>
        /// <param name="transformArg"> Изначальная позиция </param>
        /// <param name="isByClockArg"> true - поворот по часовой стрелке </param>
        /// <returns> Возвращает можно ли осуществить поворот </returns>
        public bool IsCanRotate(Vector3Int curPositionArg, Vector3Int centerArg, bool isByClockArg)
        {
            bool isCanRotate = false;
            float angle = (isByClockArg) ? 90 : -90;
            Vector3Int newPosition;

            _transform.position = curPositionArg;
            _transform.RotateAround(centerArg, new Vector3(0, 0, 1), angle);
            newPosition = Vector3Int.RoundToInt(_transform.position);

            if (_mover.IsCanMove(newPosition))
                isCanRotate = true;

            return isCanRotate;
        }

        /// <summary> Вычисляет координаты элемента после поворота вокруг точки </summary>
        /// <param name="transformArg"> Изначальная позиция </param>
        /// <param name="centerArg"> Центр вращения </param>
        /// <param name="isByClockArg"> true - поворот по часовой стрелке </param>
        /// <returns> Возвращает новую позицию </returns>
        public Vector3 Rotate(Vector3 curPositionArg, Vector3 centerArg, bool isByClockArg)
        {
            float angle = (isByClockArg) ? 90 : -90;
            Vector3 newPosition;
            Vector3Int curPositionInt = Converter.VectorMove(curPositionArg);
            Vector3Int centerInt = Converter.VectorMove(centerArg);

            if (IsCanRotate(curPositionInt, centerInt, isByClockArg))
            {
                _transform.position = curPositionArg;
                _transform.RotateAround(centerArg, new Vector3(0, 0, 1), angle);
                newPosition = _transform.position;
                return newPosition;
            }
            return curPositionArg;
        }
        public Vector3Int Rotate(Vector3Int curPositionArg, Vector3Int centerArg, bool isByClockArg)
        {
            float angle = (isByClockArg) ? 90 : -90;
            Vector3Int newPosition;

            if (IsCanRotate(curPositionArg, centerArg, isByClockArg))
            {
                _transform.position = curPositionArg;
                _transform.RotateAround(centerArg, new Vector3(0, 0, 1), angle);
                newPosition = Vector3Int.RoundToInt(_transform.position);
                return newPosition;
            }
            return curPositionArg;
        }
    }
}