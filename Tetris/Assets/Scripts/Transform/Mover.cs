using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tetris.Interfaces;
using Tetris.Global;

namespace Tetris.Transform
{
    public class Mover : IMover
    {
        private Vector3Int _mapSize;
        private ICommandObstructionIsExists _cObstructionIsExists;
        private ICommandObstructionReplace _cObstructionReplace;

        public Mover(   Vector3Int mapSizeArg, 
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

        /// <summary>
        /// Проверяет можно ли переместиться в указанную точку
        /// </summary>
        /// <param name="moveToArg"> Проверяемая точка </param>
        /// <returns> true - если можно переместиться </returns>
        /// <remarks> Не осуществляет проверку на промежуточные препятствия </remarks>
        public bool IsCanMove(Vector3Int moveToArg)
        {
            if (moveToArg.x >= _mapSize.x || moveToArg.x < 0 ||
                moveToArg.y >= _mapSize.y || moveToArg.y < 0 ||
                moveToArg.z >= _mapSize.z || moveToArg.z < 0)
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
                        break;
                    }
                }
            }

            for (int i = 1; i <= moveByAxis; i++)
            {
                if (direction)
                    infoFromMove.SetValue(moveTo, nowByAxis + i, null);
                else
                    infoFromMove.SetValue(moveTo, nowByAxis - i, null);
                if (_cObstructionIsExists.IsExists(moveTo.GetVector()))
                    return false;
            }

            if (moveTo.x >= _mapSize.x || moveTo.x < 0 ||
                moveTo.y >= _mapSize.y || moveTo.y < 0 ||
                moveTo.z >= _mapSize.z || moveTo.z < 0)
                return false;
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
                newPositionFact.x = positionItemFact.x + moveParamsFact.x;
                newPositionFact.y = positionItemFact.y + moveParamsFact.y;
                newPositionFact.z = positionItemFact.z + moveParamsFact.z;

                if (itemArg.IsObstruction)
                {
                    newPosition.x = positionItem.x + moveParams.x;
                    newPosition.y = positionItem.y + moveParams.y;
                    newPosition.z = positionItem.z + moveParams.z;
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
                newPosition.x = positionItem.x + moveParamsArg.x;
                newPosition.y = positionItem.y + moveParamsArg.y;
                newPosition.z = positionItem.z + moveParamsArg.z;

                if (itemArg.IsObstruction)
                    _cObstructionReplace.Replace(positionItem, newPosition);

                return newPosition;
            }
            return positionItem;
        }
    }
}
