using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tetris.Global;

public class Obstructions
{
    private Dictionary<string, Vector3Int> _obstructions;
    
    /// <summary>
    /// Класс описывает препятствия и их местонахождение на сцене
    /// </summary>
    public Obstructions()
    {
        _obstructions = new Dictionary<string, Vector3Int>();
    }

    /// <summary>
    /// Добавить препятствия
    /// </summary>
    /// <param name="pointsArg">Координаты препятсвий</param>
    public void Add(Vector3Int[] pointsArg)
    {
        string key;
        if (pointsArg == null)
            throw new TetrisException("Попытка передать пустую коллекцию точек!");

        foreach (var point in pointsArg)
        {
            key = String.Concat(point.x, ".", point.y, ".", point.z);
            _obstructions.Add(key, point);
        }
            
    }

    /// <summary>
    /// Переместить препятствия
    /// </summary>
    /// <param name="fromArg">Точка откуда переместить</param>
    /// <param name="toArg">Точка куда переместить</param>
    public void Replace(Vector3Int fromArg, Vector3Int toArg)
    {
        string oldKey = String.Concat(fromArg.x, ".", fromArg.y, ".", fromArg.z);
        string newKey = String.Concat(toArg.x, ".", toArg.y, ".", toArg.z);

        if (oldKey != newKey && _obstructions.ContainsKey(oldKey))
        {
            _obstructions.Remove(oldKey);
            _obstructions.Add(newKey, toArg);
        }
    }

    /// <summary>
    /// Проверка существования препятствия
    /// </summary>
    /// <param name="pointArg">Проверяемая точка</param>
    /// <returns>true - в данной точке есть препятствие </returns>
    public bool IsExists(Vector3Int pointArg)
    {
        string key = String.Concat(pointArg.x, ".", pointArg.y, ".", pointArg.z);
        return _obstructions.ContainsKey(key);
    }

    /// <summary>
    /// Удаление препятствия
    /// </summary>
    /// <param name="pointArg"> Координата препятствия на удаление </param>
    /// <returns> true - если удаление прошло успешно</returns>
    public bool Delete(Vector3Int pointArg)
    {
        string key = String.Concat(pointArg.x, ".", pointArg.y, ".", pointArg.z);
        if (_obstructions.ContainsKey(key))
        {
             _obstructions.Remove(key);
            return true;
        }  
        return false;
    }
}
