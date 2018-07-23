using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tetris.Interfaces;
using Tetris.Global;

namespace Tetris.GameObjects
{
    public class Cube : IFigure
    {
        private GameObject _model;
        private bool _isObstruction;

        public Vector3Int Size { get { return Vector3Int.one; } }
        public bool IsObstruction
        {
            set { _isObstruction = value; }
            get { return _isObstruction; }
        }
        public Vector3Int Position
        {
            get { return Converter.VectorMove(_model.transform.position); }
        }
        public Vector3 PositionFact
        {
            get { return _model.transform.position; }
        }
        public List<IFigure> InnerFigures
        {
            get { return new List<IFigure>() { this }; }
        }
        public List<Vector3Int> InnerPositions
        {
            get { return new List<Vector3Int>() { Position }; }
        }
        public List<Vector3> InnerPositionsFact
        {
            get { return new List<Vector3>() { PositionFact }; }
        }

        /// <summary> 
        /// Класс описывает сущность куб. Кубы собираются в группы в классе GroupTransformed по шаблонам TemplateGroup и образуют фигуры
        /// </summary>
        /// <param name="positionArg">Положение фигуры</param>
        /// <param name="modelArg">Модель фигуры</param>
        public Cube(Vector3Int positionArg, GameObject modelArg)
        {
            if (modelArg == null)
                throw new TetrisException("Не задана модель объекта куба");
            _model = GameObject.Instantiate(modelArg);
            _model.transform.position = positionArg;
            _isObstruction = false;
        }

        /// <summary> Передвижение объекта </summary>
        /// <param name="moverArg">Класс отвечающий за перемещения объектов</param>
        /// <param name="moveParamsArg">Вектор описывающий перемещение</param>
        /// <returns> Возвращает true, если перемещаение удалось </returns>
        public bool Move(IMover moverArg, Vector3 moveParamsArg)
        {
            Vector3 oldPosition = PositionFact;
            Vector3 newPosition;

            newPosition = moverArg.Move(this, moveParamsArg);
            if (oldPosition.x != newPosition.x ||
                oldPosition.y != newPosition.y ||
                oldPosition.z != newPosition.z)
            {
                _model.transform.position = newPosition;
                return true;
            }  
            return false;
        }
        public bool Move(IMover moverArg, Vector3Int moveParamsArg)
        {
            Vector3Int oldPosition = Position;
            Vector3Int newPosition;

            newPosition = moverArg.Move(this, moveParamsArg);
            if (oldPosition.x != newPosition.x ||
                oldPosition.y != newPosition.y ||
                oldPosition.z != newPosition.z)
            {
                _model.transform.position = newPosition;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Вращение объекта
        /// </summary>
        /// <param name="rotatorArg"> Объект используемый для вращения </param>
        /// <param name="isByClockArg"> true - по часовой стрелке </param>
        /// <returns> true - если вращение удалось </returns>
        public bool Rotate(IRotator rotatorArg, bool isByClockArg)
        {
            if (isByClockArg)
                _model.transform.Rotate(0, 0, 90);
            else
                _model.transform.Rotate(0, 0, -90);
            return true;
        }

        /// <summary>
        /// Вращение объекта вокруг точки
        /// </summary>
        /// <param name="rotatorArg"> Объект используемый для вращения </param>
        /// <param name="center"> Точка вокруг которой осуществляется вращение </param>
        /// <param name="isByClockArg"> true - по часовой стрелке </param>
        /// <returns> true - если вращение удалось </returns>
        public bool Rotate(IRotator rotatorArg, Vector3 centerArg, bool isByClockArg)
        {
            Vector3 oldPosition = PositionFact;
            Vector3 newPosition;

            newPosition = rotatorArg.Rotate(PositionFact, centerArg, isByClockArg);
            if (oldPosition.x != newPosition.x ||
                oldPosition.y != newPosition.y ||
                oldPosition.z != newPosition.z)
            {
                _model.transform.position = newPosition;
                return true;
            }
            return false;
        }
        public bool Rotate(IRotator rotatorArg, Vector3Int centerArg, bool isByClockArg)
        {
            Vector3Int oldPosition = Position;
            Vector3Int newPosition;

            newPosition = rotatorArg.Rotate(Position, centerArg, isByClockArg);
            if (oldPosition.x != newPosition.x ||
                oldPosition.y != newPosition.y ||
                oldPosition.z != newPosition.z)
            {
                _model.transform.position = newPosition;
                return true;
            }
            return false;
        }

        /// <summary> 
        /// Удаление модели коробки 
        /// </summary>
        public void Dispose()
        {
            GameObject.Destroy(_model);
        }
    }
}
