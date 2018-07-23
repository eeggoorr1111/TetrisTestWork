using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using Tetris.Interfaces;

namespace Tetris.Global
{
    /// <summary> Класс описывает логику игры на SceneGame. </summary>
    /// <see> Для получения информации по сущностям перейдите в Scripts/Global/GameConfigurators/AbstractConfigurator.cs </see>
    public class TetrisGame : MonoBehaviour
    {
        public Camera _camera;
        public Text _lblScores;

        private IMap _map;
        private IFigure _figure;
        private IPlayer _player;
        private bool _isGameOver = false;
        private int _cntScores = 0;

        private IFigureGenerator _generator;
        private IFactoryFigure _factory;

        private IMover _mover;
        private IRotator _rotator;
        private Vector3Int _vectorDrop;
        private int _delayDrop = 0;
        private int _counterFrameDrop = 0;

        private Obstructions _obstructions;
        private IStackFigures _stackFigures;

        private void Start()
        {
            AbstractConfigurator configurator = DataConfigs.LoadConfigurator();

            if (_lblScores == null)
                throw new TetrisException("Не указано поле для вывода очков!");

            _map = configurator.CreateMap();
            _generator = configurator.CreateGenerator();
            _factory = configurator.CreateFactoryFigure();
            _mover = configurator.CreateMover();
            _rotator = configurator.CreateRotator();
            _vectorDrop = configurator.CreateVectorDropInt();
            _delayDrop = configurator.DelayFrameDrop();
            _obstructions = configurator.CreateObstructions();
            _stackFigures = configurator.CreateStackFigures();

            _map.SetCamera(_camera);
            _figure = _generator.NewFigure(_factory);
        }

        private void Update()
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
                    _lblScores.text = _cntScores.ToString();
                    if (_stackFigures.MaxArea >= _map.Size.y - 1)
                    {
                        GameOver();
                        return;
                    }
                    _figure = _generator.NewFigure(_factory);
                }
            }

            if (Input.GetKeyDown(KeyCode.D))
                _figure.Move(_mover, new Vector3Int(1, 0, 0));
            else if (Input.GetKeyDown(KeyCode.A))
                _figure.Move(_mover, new Vector3Int(-1, 0, 0));
            else if (Input.GetKeyDown(KeyCode.W))
                _figure.Move(_mover, new Vector3Int(0, 0, 1));
            else if (Input.GetKeyDown(KeyCode.S))
                _figure.Move(_mover, new Vector3Int(0, 0, -1));
            else if (Input.GetKeyDown(KeyCode.Q))
                _figure.Rotate(_rotator, true);
            else if (Input.GetKeyDown(KeyCode.E))
                _figure.Rotate(_rotator, false);
        }

        private void GameOver()
        {
            SceneManager.LoadScene(0);
        }
    }
}
