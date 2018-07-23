using System.Collections;
using System.Collections.Generic;
using Tetris.Global;
using UnityEngine;

namespace Tetris.Generator
{
    public class TemplateFigureManager
    {
        private List<TemplateGroupFigures> _listFigures;
        private List<byte> _rangesChance;
        private byte _chancesSum;
        

        public bool IsFill { get { return (_chancesSum == 100); } }


        /// <summary>
        /// Данный класс хранит ссылки на шаблоны и предоставляет удобный сервис для получения случайно определяемого шаблона
        /// </summary>
        public TemplateFigureManager()
        {
            _listFigures = new List<TemplateGroupFigures>();
            _rangesChance = new List<byte>() { 0 };
            _chancesSum = 0;
        }

        /// <summary> Метод добавляет новый шаблон в менеджер шаблонов </summary>
        /// <param name="templateFigureArg"> Ссылка на шаблон фигуры</param>
        public TemplateFigureManager AddTemplate(TemplateGroupFigures templateFigureArg)
        {
            if (_chancesSum + templateFigureArg.Chance > 100)
                throw new TetrisException("Сумма вероятностей выпадения фигур не может быть больше 100%");

            _chancesSum += templateFigureArg.Chance;
            _rangesChance.Add(_chancesSum);
            _listFigures.Add(templateFigureArg);

            return this;
        }

        /// <summary> У каждого шаблона есть ряд значений на которые он отзывается. 
        /// Их количество зависит от верятности выпадения шаблона</summary>
        /// <param name="randomArg"> Случайное число от 1 до 100 включительно </param>
        /// <returns> Возвращает шаблон фигуры по случайному числу от 1 до 100 </returns>
        public TemplateGroupFigures GetTemplateByRandom(byte randomArg)
        {
            if (!IsFill)
                throw new TetrisException("Попытка использовать менеджер шаблонов, который еще не заполнен!");

            if(randomArg < 1 || randomArg > 100)
                throw new TetrisException("Нет ни одного шаблона, который возвращается по числу " + randomArg + "!");

            for (int i = 1; i < _rangesChance.Count; i++)
            {
                if (randomArg > _rangesChance[i - 1] && randomArg <= _rangesChance[i])
                    return _listFigures[i - 1];
            }
            throw new TetrisException("Нет ни одного шаблона, который возвращается по числу " + randomArg + "!");
        }
    }
}
