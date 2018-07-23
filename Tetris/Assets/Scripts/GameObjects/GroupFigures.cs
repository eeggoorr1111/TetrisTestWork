using System;
using System.Collections;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Tetris.Generator;
using Tetris.Interfaces;
using Tetris.Global;

namespace Tetris.GameObjects
{
    public class GroupFigures : IFigure
    {
        private TemplateGroupFigures _template;
        private int _templateVersion;
        private List<IFigure> _figures;
        private Vector3 _positionFact;
        private bool _isObstruction;

        public Vector3Int Position
        {
            get { return Converter.VectorMove(_positionFact); }
        }
        public Vector3 PositionFact
        {
            get { return _positionFact; }
        }
        public Vector3Int Size
        {
            get { return _template.Size; }
        }
        public TemplateGroupFigures Template
        {
            get { return _template; }
        }
        public int TemplateVersion
        {
            get { return _templateVersion; }
        }
        public bool IsObstruction
        {
            set { _isObstruction = value; }
            get { return _isObstruction; }
        }
        public List<IFigure> InnerFigures
        {
            get
            {
                List<IFigure> figures = new List<IFigure>();
                foreach (IFigure figure in _figures)
                    figures.AddRange(figure.InnerFigures);
                return figures;
            }
        }
        public List<Vector3Int> InnerPositions
        {
            get
            {
                List<Vector3Int> positions = new List<Vector3Int>();
                foreach (IFigure figure in _figures)
                    positions.AddRange(figure.InnerPositions);
                return positions;
            }
        }
        public List<Vector3> InnerPositionsFact
        {
            get
            {
                List<Vector3> positions = new List<Vector3>();
                foreach (IFigure figure in _figures)
                    positions.AddRange(figure.InnerPositionsFact);
                return positions;
            }
        }

        /// <summary> Класс представляет группу объектов (Фигуру) </summary>
        /// <param name="templateArg"> Шаблон группы объектов (фигуры) </param>
        /// <param name="positionArg"> Координата группы ( левый, нижний, ближний к нам край) </param>
        /// <remarks> 
        /// Группа объектов хранит версию шаблона по которой была создана (может быть так, что группы объектов будут созданы по 1 шаблону, но разных версий) 
        /// Несмотря на то, что можно сделать GroupFigures элементом другого GroupFigures, пока что это поддерживается лишь частично
        /// </remarks>
        public GroupFigures(TemplateGroupFigures templateArg, Vector3Int positionArg, IFactoryFigure factoryArg)
        {
            Vector3Int positionResult = Vector3Int.zero;
            Vector3Int positionGroup = Converter.VectorMove(positionArg);
            List<Vector3Int> template;
            IFigure figure;

            if (templateArg == null)
                throw new TetrisException("Попытка создать фигуру без шаблона!");

            _template = templateArg;
            _templateVersion = _template.Version;
            _positionFact = positionArg;

            _figures = new List<IFigure>();
            _isObstruction = false;
            template = _template.Template;
            foreach (var positionItem in template)
            {
                positionResult.x = positionGroup.x + positionItem.x;
                positionResult.y = positionGroup.y + positionItem.y;
                positionResult.z = positionGroup.z + positionItem.z;
                figure = factoryArg.Create(positionResult);
                if (figure.Size.x > 1 || figure.Size.y > 1 || figure.Size.z > 1)
                    throw new TetrisException("Возможность работать с группой объектов внутри другой группы объектов, реализована лишь частично!");
                _figures.Add(figure);
            }
        }

        /// <summary> Метод осуществляет перемещение группы фигур </summary>
        /// <param name="moverArg">Класс занимающийся перемещением фигур</param>
        /// <param name="moveParamArg">Вектор задающий движение фигуры</param>
        /// <returns> Возвращает true, если перемещение успешно выполнено </returns>
        public bool Move(IMover moverArg, Vector3 moveParamArg)
        {
            List<IMovable> borderPlane;
            Vector3Int moveParamInt = Converter.VectorMove(moveParamArg);

            borderPlane = GetBorderArea(moveParamInt);
            foreach (var item in borderPlane)
            {
                if (!moverArg.IsCanMove(item.Position, moveParamInt))
                    return false;
            }
            foreach (var item in _figures)
                item.Move(moverArg, moveParamArg);

            _positionFact = moverArg.Move(this, moveParamArg);
            return true;
        }
        public bool Move(IMover moverArg, Vector3Int moveParamArg)
        {
            List<IMovable> borderPlane;
            Vector3Int moveParamInt = moveParamArg;

            borderPlane = GetBorderArea(moveParamInt);
            foreach (var item in borderPlane)
            {
                if (!moverArg.IsCanMove(item.Position, moveParamInt))
                    return false;
            }
            foreach (var item in _figures)
                item.Move(moverArg, moveParamArg);

            _positionFact = moverArg.Move(this, moveParamArg);
            return true;
        }

        /// <summary> Поворот группы фигур </summary>
        /// <param name="rotatorArg"> Класс отвечающий за поворот объектов </param>
        /// <param name="isByClockArg"> true - по часовой стрелке поворот </param>
        /// <returns> true, если поворот успешно удался  </returns>
        public bool Rotate(IRotator rotatorArg, bool isByClockArg)
        {
            Vector3Int centerRotate = rotatorArg.CalculateCenter(InnerPositions.ToArray(), isByClockArg);

            IFigure figureRotate = _figures.Where(figure =>
                figure.Position.x == centerRotate.x &&
                figure.Position.y == centerRotate.y &&
                figure.Position.z == centerRotate.z).First();

            if (figureRotate == null)
                return false;

            foreach (var figure in _figures)
            {
                if (figure != figureRotate)
                {
                    if (!rotatorArg.IsCanRotate(figure.Position, centerRotate, isByClockArg))
                        return false;
                }
            }

            foreach (var figure in _figures)
            {
                if (figure != figureRotate)
                    figure.Rotate(rotatorArg, centerRotate, isByClockArg);
            }

            return true;
        }

        /// <summary> Поворот группы фигур вокруг точки </summary>
        /// <param name="rotatorArg"> Класс отвечающий за поворот объектов </param>
        /// /// <param name="centerArg"> Центр вращения </param>
        /// <param name="isByClockArg"> true - по часовой стрелке поворот </param>
        /// <returns> true, если поворот успешно удался  </returns>
        public bool Rotate(IRotator rotatorArg, Vector3 centerArg, bool isByClockArg)
        {
            Vector3Int centerInt = Converter.VectorMove(centerArg);
            foreach (var figure in _figures)
            {
                if (!rotatorArg.IsCanRotate(figure.Position, centerInt, isByClockArg))
                    return false;
            }

            foreach (var figure in _figures)
                figure.Rotate(rotatorArg, centerArg, isByClockArg);
            
            return true;
        }
        public bool Rotate(IRotator rotatorArg, Vector3Int centerArg, bool isByClockArg)
        {
            Vector3Int centerInt = centerArg;
            foreach (var figure in _figures)
            {
                if (!rotatorArg.IsCanRotate(figure.Position, centerInt, isByClockArg))
                    return false;
            }

            foreach (var figure in _figures)
                figure.Rotate(rotatorArg, centerArg, isByClockArg);

            return true;
        }

        /// <summary> 
        /// Метод возвращает поверхность данной фигуры, перпендикулярную заданной оси 
        /// </summary>
        /// <param name="axisArg">Ось, перпендикулярно которой мы ищем поверхность</param>
        /// <param name="directionArg"> true - возвращает поверхность с большим значением по оси axisArg, иначе с меньшим</param>
        /// <returns> Метод возвращает список координат фигур представляющих поверхность </returns>
        public List<IMovable> GetBorderArea(Vector3Int moveParamsArg)
        {
            List<IMovable> borderArea = new List<IMovable>();
            Axis[] allAxis = System.Enum.GetValues(typeof(Axis)) as Axis[];
            List<PropertyInfo> infoAxis = new List<PropertyInfo>();
            PropertyInfo infoAxisMove = null;
            PropertyInfo infoAxisTemp;
            bool direction = false;
            int valByAxis = 0;
            int cntAxisMove = 0;
            IFigure item;

            if (allAxis == null)
                throw new TetrisException("Не удалось получить список осей!");

            foreach (var enumAxis in allAxis)
            {
                if (enumAxis != Axis.none)
                {
                    infoAxisTemp = typeof(Vector3Int).GetProperty(enumAxis.ToString());
                    valByAxis = (int)infoAxisTemp.GetValue(moveParamsArg, null);
                    if (valByAxis != 0)
                    {
                        //if (cntAxisMove > 0)
                        //    throw new TetrisException("Нельзя задать движение по более чем 2-ум осям!");
                        cntAxisMove++;
                        infoAxisMove = infoAxisTemp;
                        direction = (valByAxis > 0);
                    }
                    else
                        infoAxis.Add(infoAxisTemp);
                }
            }

            if (infoAxisMove == null)
                throw new TetrisException("Не удалось получить экземпляр типа 'PropertyInfo' для оси axisArg!");

            var groupsFigures = _figures.GroupBy(
                (figure) => {
                    return new
                    {
                        axis1 = infoAxis[0].GetValue(figure.Position, null),
                        axis2 = infoAxis[1].GetValue(figure.Position, null)
                    };
                },
                (key, group) => new {
                    Axis1 = key.axis1,
                    Axis2 = key.axis2,
                    Group = group
                }
            );
            
            foreach (var groupFigures in groupsFigures)
            {
                if (direction)
                {
                    item = groupFigures.Group.Aggregate((figure1, figure2) =>
                    {
                        return ((int)infoAxisMove.GetValue(figure1.Position, null) > (int)infoAxisMove.GetValue(figure2.Position, null)) ? figure1 : figure2;
                    });
                }
                else
                {
                    item = groupFigures.Group.Aggregate((figure1, figure2) =>
                    {
                        return ((int)infoAxisMove.GetValue(figure1.Position, null) > (int)infoAxisMove.GetValue(figure2.Position, null)) ? figure2 : figure1;
                    });
                }
                borderArea.Add(item);
            }

            return borderArea;
        }

        /// <summary> 
        /// Удаляет модели всех внутренних фигур 
        /// </summary>
        public void Dispose()
        {
            foreach (var figure in _figures)
                figure.Dispose();
        }
    }
}

