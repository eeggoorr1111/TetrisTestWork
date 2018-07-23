using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tetris.GameObjects;
using Tetris.Global;
using Tetris.Interfaces;

namespace Tetris.Generator
{
 public class Generator : IFigureGenerator
    {
        private Vector3Int _sizeMap;
        private TemplateFigureManager _managerTemplates;


        /// <summary> Класс генерирующий фигуры </summary>
        /// <param name="sizeMapArg"> Размер игрового поля </param>
        /// <param name="templatesFiguresArg"> Менеджер шаблонов фигур </param>
        /// <param name="moverArg"> Класс занмиающийся передвижением объектов </param>
        public Generator(   Vector3Int sizeMapArg,
                            TemplateFigureManager managerTemplatesArg)
        {
            if (sizeMapArg.x < 5 || sizeMapArg.y < 5 || sizeMapArg.z < 1)
                throw new TetrisException("Слишком маленький размер карты");

            if(managerTemplatesArg == null || !managerTemplatesArg.IsFill)
                throw new TetrisException("Попытка передать нулевой или пустой менеджер шаблонов в генератор!");

            _sizeMap = sizeMapArg;
            _managerTemplates = managerTemplatesArg;
        }

        /// <summary> Генерация новой фигуры </summary>
        /// <returns> Возвращает новую фигуру</returns>
        public IFigure NewFigure(IFactoryFigure factoryArg)
        {
            byte randomTemplate = (byte)Random.Range(1, 101);
            TemplateGroupFigures template = _managerTemplates.GetTemplateByRandom(randomTemplate);
            Vector3Int sizeTemplate = template.Size;
            int posX = Random.Range(0, _sizeMap.x - sizeTemplate.x);
            int posY = _sizeMap.y - sizeTemplate.y;
            int posZ = Random.Range(0, _sizeMap.z - sizeTemplate.z);
            IFigure figure = new GroupFigures(template, new Vector3Int(posX, posY, posZ), factoryArg); 

            if (sizeTemplate.x >= _sizeMap.x ||
                sizeTemplate.y >= _sizeMap.y ||
                sizeTemplate.z > _sizeMap.z)
                throw new TetrisException("Попытка сгенерировать фигуру, которая больше игрового поля (карты)!");

            return figure;
        }
    }
}
   