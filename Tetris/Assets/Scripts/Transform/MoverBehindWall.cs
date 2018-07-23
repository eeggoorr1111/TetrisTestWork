using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tetris.Interfaces;
using Tetris.Global;

namespace Tetris.Transform
{
    public class MoverBehindWall : IMover
    {
        private Vector3Int _mapSize;
        private ICommandObstructionIsExists _cObstructionIsExists;
        private ICommandObstructionReplace _cObstructionReplace;

        /// <summary>
        /// Класс использующийся для передвижения объектов. Позволяет перемещать объекты за стену
        /// </summary>
        /// <param name="mapSizeArg"> Размер карты </param>
        /// <param name="cObstructionIsExistsArg"> Команда для проверкисуществования препятствия </param>
        /// <param name="cObstructionReplaceArg"> Команда для информирования об перемещении препятствия </param>
        public MoverBehindWall(Vector3Int mapSizeArg,
                        ICommandObstructionIsExists cObstructionIsExistsArg,
                        ICommandObstructionReplace cObstructionReplaceArg)
        {
            _mapSize = mapSizeArg;
            if (cObstructionIsExistsArg == null)
                throw new TetrisException("Отсутствует команда проверки наличия препятствия!");

            if (cObstructionReplaceArg == null)
                throw new TetrisException("Отсутствует команда перемещения препятствия!");

            _cObstructionIsExists = cObstructionIsExistsArg;
            _cObstructionReplace = cObstructionReplaceArg;
        }

        private Vector3 CalculateSum(Vector3 vector1, Vector3 vector2)
        {
            Vector3 sum = Vector3.zero;

            sum.x = vector1.x + vector2.x;
            sum.y = vector1.y + vector2.y;
            sum.z = vector1.z + vector2.z;

            if (sum.x >= _mapSize.x)
                sum.x = sum.x - _mapSize.x;
            if (sum.x < 0)
                sum.x = _mapSize.x + sum.x;

            if (sum.z >= _mapSize.z)
                sum.z = sum.z - _mapSize.z;
            if (sum.z < 0)
                sum.z = _mapSize.z + sum.z;

            return sum;
        }
        private Vector3Int CalculateSum(Vector3Int vector1, Vector3Int vector2)
        {
            Vector3Int sum = Vector3Int.zero;

            sum.x = vector1.x + vector2.x;
            sum.y = vector1.y + vector2.y;
            sum.z = vector1.z + vector2.z;

            if (sum.x >= _mapSize.x)
                sum.x = sum.x - _mapSize.x;
            if (sum.x < 0)
                sum.x = _mapSize.x + sum.x;

            if (sum.z >= _mapSize.z)
                sum.z = sum.z - _mapSize.z;
            if (sum.z < 0)
                sum.z = _mapSize.z + sum.z;

            return sum;
        }

        /// <summary> Осуществляет проверку на возможность произвести передвижение </summary>
        /// <param name="moveToArg"> Точка в которую планируется передвижение </param>
        /// <returns> True передвижение возможно, false - не возможно </returns>
        public bool IsCanMove(Vector3Int moveToArg)
        {
            if (moveToArg.y >= _mapSize.y || moveToArg.y < 0)
                return false;
            return !_cObstructionIsExists.IsExists(moveToArg);
        }

        /// <summary> Осуществляет проверку на возможность произвести передвижение </summary>
        /// <param name="moveToArg"> Точка в которую планируется передвижение </param>
        /// <returns> True передвижение возможно, false - не возможно </returns>
        public bool IsCanMove(Vector3Int moveFromArg, Vector3Int moveParamsArg)
        {
            Axis[] allAxis = System.Enum.GetValues(typeof(Axis)) as Axis[];
            PropertyInfo infoParamsMove = null;
            PropertyInfo infoFromMove = null;
            Vector3IntWrap moveTo = new Vector3IntWrap(moveFromArg);
            int moveByAxis = 0;
            int nowByAxis = 0;
            bool direction = false;
            int interimVal = 0;
            int mapSizeByAxis = 0;


            if (allAxis == null)
                throw new TetrisException("Не удалось получить список осей!");

            foreach (var enumAxis in allAxis)
            {
                if (enumAxis != Axis.none)
                {
                    infoParamsMove = typeof(Vector3Int).GetProperty(enumAxis.ToString());
                    moveByAxis = (int)infoParamsMove.GetValue(moveParamsArg, null);
                    if (moveByAxis != 0)
                    {
                        direction = (moveByAxis > 0);
                        moveByAxis = Math.Abs(moveByAxis);
                        infoFromMove = typeof(Vector3IntWrap).GetProperty(enumAxis.ToString());
                        nowByAxis = (int)infoFromMove.GetValue(moveTo, null);
                        mapSizeByAxis = (int)infoParamsMove.GetValue(_mapSize, null);
                        break;
                    }
                }
            }

            for (int i = 1; i <= moveByAxis; i++)
            {
                interimVal = (direction) ? nowByAxis + i : nowByAxis - i;
                if (infoParamsMove.Name != "y")
                {
                    if (interimVal < 0)
                        interimVal = mapSizeByAxis + interimVal;
                    if (interimVal >= mapSizeByAxis)
                        interimVal = interimVal - mapSizeByAxis;
                }
                infoFromMove.SetValue(moveTo, interimVal, null);

                if (moveTo.y >= _mapSize.y || moveTo.y < 0)
                    return false;

                if (_cObstructionIsExists.IsExists(moveTo.GetVector()))
                    return false;
            }
            return true;
        }

        /// <summary> Осуществляет вычисление координат для передвижения </summary>
        /// <param name="moveParamsArg"> Параметры движения, которое нужно выполнить </param>
        /// <param name="posArg"> Текущая позиция элемента </param>
        /// <returns> Возвращает новую точку на которую нужно переместить объект </returns>
        public Vector3 Move(IMovable itemArg, Vector3 moveParamsArg)
        {
            Vector3 positionItemFact = itemArg.PositionFact;
            Vector3Int positionItem = itemArg.Position;

            Vector3 moveParamsFact = moveParamsArg;
            Vector3Int moveParams = Converter.VectorMove(moveParamsArg);

            Vector3 newPositionFact = Vector3.zero;
            Vector3Int newPosition = Vector3Int.zero;

            if (IsCanMove(positionItem, moveParams))
            {
                newPositionFact = CalculateSum(positionItemFact, moveParamsFact);
                if (itemArg.IsObstruction)
                {
                    newPosition = CalculateSum(positionItem, moveParams);
                    _cObstructionReplace.Replace(positionItem, newPosition);
                }
                return newPositionFact;
            }
            return positionItemFact;
        }
        public Vector3Int Move(IMovable itemArg, Vector3Int moveParamsArg)
        {
            Vector3Int positionItem = itemArg.Position;
            Vector3Int newPosition = Vector3Int.zero;

            if (IsCanMove(positionItem, moveParamsArg))
            {
                newPosition = CalculateSum(positionItem, moveParamsArg);
                if (itemArg.IsObstruction)
                    _cObstructionReplace.Replace(positionItem, newPosition);
                
                return newPosition;
            }
            return positionItem;
        }
    }
}

