using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tetris.Global;
using Tetris.Interfaces;

namespace Tetris.GameObjects
{
    public class CubeFactory : IFactoryFigure
    {
        private GameObject _model;

        /// <summary> Класс генерирующий кубы. Является частью реализации паттерна "Фабричный метод" </summary>
        /// <param name="modelArg"> Модель куба, которая будет использоваться в качестве представления </param>
        public CubeFactory(GameObject modelArg)
        {
            if (modelArg == null)
                throw new TetrisException("Отсутствует модель куба!");
            _model = modelArg;
        }

        /// <summary> Создаем куб </summary>
        /// <param name="positionArg"> Позиция куба </param>
        /// <returns> Ссылка на только что созданный куб </returns>
        public IFigure Create(Vector3Int positionArg)
        {
            return new Cube(positionArg, _model);
        }
    }

}
