using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tetris.Global
{
    public class DataConfigs
    {
        
        private static AbstractConfigurator _configurator;

        /// <summary>
        /// Класс переносит выбранную пользователем конфигурацию с одной сцены на другую
        /// </summary>
        /// <param name="configuratorArg"> Сохраняет конфигурацию, которую нужно будет загрузить на игровой сцене </param>
        public static void SaveConfigurator(AbstractConfigurator configuratorArg)
        {
            _configurator = configuratorArg;
        }

        /// <summary> Вызывается с игровой сцены для загрузки конфигурации </summary>
        /// <returns> Возвращает сохраненную конфигурацию игры </returns>
        public static AbstractConfigurator LoadConfigurator()
        {
            if (_configurator == null)
                throw new TetrisException("Не выбран режим игры!");
            return _configurator;
        }
    }
}
    
