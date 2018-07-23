using System;
using System.Collections.Generic;
using UnityEngine;
using Tetris.Global;
using Tetris.Interfaces;
using Tetris.GameObjects;

namespace Tetris.Generator
{
    public class TemplateGroupFigures
    {
        private int _id;
        private Vector3Int _size;
        private byte _chance;
        private List<Vector3Int> _template;

        public int ID { get { return _id; } }
        public Vector3Int Size { get { return _size; } }
        public byte Chance { get { return _chance; } }
        public List<Vector3Int> Template
        {
            get
            {
                List<Vector3Int> copy = new List<Vector3Int>();
                foreach (var item in _template)
                    copy.Add(item);
                return copy;
            }
        }
        public int Version { get { return _template.Count; } }

        /// <summary> Класс описывает шаблон по которому создаются фигуры (объекты GroupFigures) </summary>
        /// <param name="idArg"> ID шаблона фигуры. Уникальность не гарантируется. Служит чисто для отладки. </param>
        /// <param name="sizeArg"> Размер шаблона в 3 измерениях </param>
        /// <param name="chanceArg"> Шанс выпадения фигуры по данному шаблону в процентах от 1 до 100</param>
        public TemplateGroupFigures(int idArg, Vector3Int sizeArg, byte chanceArg)
        {
            if (idArg < 1)
                throw new TetrisException("ID шаблона не может быть меньше 1!");
            if (chanceArg <= 0 || chanceArg > 100)
                throw new TetrisException("Шанс выпадения фигуры по шаблону может быть > 0 и <= 100!");
            if (sizeArg.x < 1 || sizeArg.y < 1 || sizeArg.z < 1)
                throw new TetrisException("Шаблон должен быть задан в 3-ех плоскостях!");
            _id = idArg;
            _chance = chanceArg;
            _size = sizeArg;
            _template = new List<Vector3Int>();
        }

        /// <summary> Указывает какой элемент в шаблоне должен быть заполнен </summary>
        /// <param name="posItemArg"> Позиция элемента который должен быть заполнен </param>
        /// <remarks> Фактически класс описывает булевую матрицу. AddItem в какой точке матрицы true </remarks>
        public void AddItem(Vector3Int posItemArg)
        {
            if (posItemArg.x >= 0 && posItemArg.x < _size.x &&
                posItemArg.y >= 0 && posItemArg.y < _size.y &&
                posItemArg.z >= 0 && posItemArg.z < _size.z)
            {
                _template.Add(posItemArg);
                return;
            }
            throw new TetrisException("Попытка добавить элемент, который лежит вне границ шаблона");
        }
    }
}
