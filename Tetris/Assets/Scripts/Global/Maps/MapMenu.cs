using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tetris.Global
{
    public class MapMenu : IMap
    {
        private Vector3Int _sizeMap;

        public Vector3Int Size { get { return _sizeMap; } }

        /// <summary> Класс ответчает за карту в меню </summary>
        /// <param name="sizeMapArg"> Размер карты </param>
        /// <remarks> На данный момент карта только настраивает положение камеры так, чтобы все фигуры блоки были видны</remarks>
        public MapMenu(Vector3Int sizeMapArg)
        {
            _sizeMap = sizeMapArg;
        }

        /// <summary> Позционирование камеры так, чтобы все блоки были видны </summary>
        /// <param name="_camera"> Камера, которую нужно позиционировать </param>
        public void SetCamera(Camera _camera)
        {
            Vector3 position = Vector3.zero;
            int frustumHeight = _sizeMap.y/2;
            float distance;

            if (frustumHeight < 1)
                throw new TetrisException("Слишком маленький размер карты!");

            if (_camera == null)
                throw new TetrisException("Не установлена камера!");

            distance = frustumHeight * 0.5f / Mathf.Tan(_camera.fieldOfView * 0.5f * Mathf.Deg2Rad);

            position.x = _sizeMap.x / 2;
            position.y = _sizeMap.y / 2 + 2;
            position.z = -distance;

            _camera.transform.position = position;
        }
    }
}