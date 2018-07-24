using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tetris.Global;
using Tetris.Interfaces;
using System.Linq;

namespace Tetris.GameObjects
{
    public class StackFigures : IStackFigures
    {
        private int _sizeFillArea;
        private IMover _mover;
        private List<IFigure> _figures;
        private ICommandObstructionDelete _cmdObstructionDelete;
        private int _cntAreaClear;
        private int _maxArea;

        public int MaxArea { get { return _maxArea; } }

        /// <summary>
        /// Если ряд заполнен, он опускается вниз в зависимости от количества очищенных рядов
        /// </summary>
        /// <param name="cntClear"> Количество удаленных рядов </param>
        /// <param name="areaNum"> Номер области которую нужно перетащить </param>
        private void DropArea(int cntClear, int areaNum)
        {
            if (cntClear > 0)
            {
                var moveFigures = _figures.Where(figure => figure.Position.y == areaNum);
                foreach (var figure in moveFigures)
                    figure.Move(_mover, new Vector3Int(0, -cntClear, 0));
            }
        }

        /// <summary> Класс представляет кучу фигур, в которой они скапливаются </summary>
        /// <param name="cntClearAreaArg"> Минимальное количество областей которое будет удаляться за раз </param>
        public StackFigures(Vector3Int sizeMapArg, int cntAreaClearArg, IMover moverArg, ICommandObstructionDelete cmdObstructionDeleteArg)
        {
            _figures = new List<IFigure>();
            _cntAreaClear = cntAreaClearArg;
            _sizeFillArea = sizeMapArg.x * sizeMapArg.z;
            _mover = moverArg;
            _cmdObstructionDelete = cmdObstructionDeleteArg;
        }

        /// <summary> Добавление фигур в кучу фигур и очистка заполненных областей, если такие есть </summary>
        /// <param name="figure"> Фигура, которую необходимо добавить в кучу </param>
        /// <returns> Количество удаленных областей </returns>
        public int Add(IFigure[] figuresArg)
        {
            List<int> areasCleal = new List<int>();
            List<IFigure> deleteFigures = new List<IFigure>();
            int lastArea = 0;
            int cntClear = 0;

            if (figuresArg == null)
                throw new TetrisException("Попытка добавить в кучу фигур пустую фигуру!");

            foreach (var figure in figuresArg)
            {
                ((IMovable)figure).IsObstruction = true;
                _figures.Add(figure);
            }
               
            var areas = _figures.OrderBy(figure => figure.Position.y).GroupBy(
                (figure) => new
                {
                    figure.Position.y
                },
                (key, group) => new
                {
                    Key = key.y,
                    Count = group.Count()
                }
            );

            foreach (var area in areas)
            {
                if (area.Count == _sizeFillArea)
                    areasCleal.Add(area.Key);
                else if (areasCleal.Count >= _cntAreaClear)
                {
                    foreach (int areaClear in areasCleal)
                    {
                        var deleteFiguresQuery = _figures.Where(figure => figure.Position.y == areaClear);
                        foreach (var deleteFigure in deleteFiguresQuery)
                            deleteFigures.Add(deleteFigure);

                        foreach (var deleteFigure in deleteFigures)
                        {
                            _cmdObstructionDelete.Delete(deleteFigure.Position);
                            deleteFigure.Dispose();
                            _figures.Remove(deleteFigure);
                        }
                        deleteFigures.Clear();
                    }
                    cntClear += areasCleal.Count();
                    areasCleal.Clear();
                }
                else
                    areasCleal.Clear();
                DropArea(cntClear, area.Key);
                lastArea++;
            }
            _maxArea = lastArea - cntClear;
            return cntClear;
        }

        public void Dispose()
        {
            foreach (var figure in _figures)
                figure.Dispose();
        }
    }
}
    
