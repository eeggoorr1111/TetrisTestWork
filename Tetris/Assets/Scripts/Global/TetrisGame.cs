using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using Tetris.Interfaces;

namespace Tetris.Global
{
    public class TetrisGame : MonoBehaviour
    {
        private IMap _map;
        private IFigure _figure;
        private IPlayer _player;
        private bool _isGameOver = false;
        private int _cntScores = 0;
        private int _cntScoresOld = 0;

        private IFigureGenerator _generator;
        private IFactoryFigure _factory;

        private IMover _mover;
        private IRotator _rotator;
        private Vector3Int _vectorDrop;
        private int _delayDrop = 0;
        private int _counterFrameDrop = 0;

        private Obstructions _obstructions;
        private IStackFigures _stackFigures;

        public void GameOver()
        {
            _figure.Dispose();
            _stackFigures.Dispose();
            SceneManager.LoadScene(0);
        }

        /// <summary> Класс описывает логику игры на SceneGame (Компонент Model). </summary>
        /// <see> Для получения информации по сущностям перейдите в Scripts/Global/GameConfigurators/AbstractConfigurator.cs </see>
        public TetrisGame()
        {
            AbstractConfigurator configurator = DataConfigs.LoadConfigurator();

            if (configurator == null)
                throw new TetrisException("Не выбран режим игры!");

            _map = configurator.CreateMap();
            _generator = configurator.CreateGenerator();
            _factory = configurator.CreateFactoryFigure();
            _mover = configurator.CreateMover();
            _rotator = configurator.CreateRotator();
            _vectorDrop = configurator.CreateVectorDropInt();
            _delayDrop = configurator.DelayFrameDrop();
            _obstructions = configurator.CreateObstructions();
            _stackFigures = configurator.CreateStackFigures();

            
            _figure = _generator.NewFigure(_factory);
        }

        public void NewFrame()
        { 
            bool isMoved = false;
            
            if (_isGameOver)
                return;

            _counterFrameDrop++;
            if (_counterFrameDrop == _delayDrop)
            {
                _counterFrameDrop = 0;
                isMoved = _figure.Move(_mover, _vectorDrop);
                if (!isMoved)
                {
                    _obstructions.Add(_figure.InnerPositions.ToArray());
                    _cntScores += _stackFigures.Add(_figure.InnerFigures.ToArray());
                    if (_cntScores != _cntScoresOld)
                    {
                        ChangeScoresEvent(_cntScores);
                        _cntScoresOld = _cntScores;
                    }
                    if (_stackFigures.MaxArea >= _map.Size.y - 1)
                    {
                        GameOver();
                        return;
                    }
                    _figure = _generator.NewFigure(_factory);
                }
            }
        }
        public void Move(Vector3Int moveParamsArg)
        {
            _figure.Move(_mover, moveParamsArg);
        }
        public void Rotate(bool directionArg)
        {
            _figure.Rotate(_rotator, directionArg);
        }
        public void SetCamera(Camera cameraArg)
        {
            _map.SetCamera(cameraArg);
        }
        public event Action<int> ChangeScoresEvent = (int scoresArg) => { };
    }
}
