using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tetris.Interfaces;

namespace Tetris.Global
{
    public abstract class AbstractConfigurator
    {
        protected string _name = "MODE NO NAME";

        public string Name
        {
            get { return _name; }
        }

        #region Карта, камера

        /// <summary>
        /// IMap - работа с камерой
        /// </summary>
        public abstract IMap CreateMap();

        #endregion

        #region Генерация фигур

        /// <summary>
        /// IFactoryFigure - Задает тип фигуры в классе GroupFigures
        /// </summary>
        public abstract IFactoryFigure CreateFactoryFigure();

        /// <summary>
        /// Генератор фигур
        /// </summary>
        public abstract IFigureGenerator CreateGenerator();

        #endregion

        #region Перемещение и вращение фигур

        /// <summary>
        /// Задает вектор падения фигур. В игре а данный момент используется CreateVectorDropInt
        /// </summary>
        public abstract Vector3 CreateVectorDrop();
        public abstract Vector3Int CreateVectorDropInt();

        /// <summary>
        /// Количество кадров, спустя которое фигура совершает очередное движение
        /// </summary>
        public abstract int DelayFrameDrop();

        /// <summary>
        /// Задает класс, который занимается передвижением фигур
        /// </summary>
        public abstract IMover CreateMover();

        /// <summary>
        /// Задает класс, который занимается вращением фигур
        /// </summary>
        public abstract IRotator CreateRotator();

        #endregion

        #region Препятствия
        /// <summary>
        /// Задает класс, который описывает препятствия на сцене
        /// </summary>
        public abstract Obstructions CreateObstructions();

        /// <summary>
        /// Задает класс, который представляет кучу объектов и занимается удалением заполненных строк
        /// </summary>
        public abstract IStackFigures CreateStackFigures();
        #endregion

        #region Команды для работы с препятствиями
        /// <summary>
        /// Команды, которые используются для работы с препятствиями
        /// </summary>
        public abstract ICommandObstructionIsExists CmdObstructionIsExists();
        public abstract ICommandObstructionAdd CmdObstructionAdd();
        public abstract ICommandObstructionReplace CmdObstructionReplace();
        public abstract ICommandObstructionDelete CmdObstructionDelete();
        #endregion
    }
}
