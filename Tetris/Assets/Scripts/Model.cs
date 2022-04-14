using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

namespace Tetris
{
    /// <summary>
    /// Model в рамках паттерна MVC
    /// </summary>
    public class Model
    {
        public Model(Rotator rotatorArg, IReadOnlyList<IMover> moversArg)
        {
            //_map = configurator.CreateMap();
            //_generator = configurator.CreateGenerator();
            //_factory = configurator.CreateFactoryFigure();
            _movers = moversArg;
            _rotator = rotatorArg;
            _vectorDrop = Vector3.down;
            //_delayDrop = configurator.DelayFrameDrop();
            //_obstructions = configurator.CreateObstructions();
            //_stackFigures = configurator.CreateStackFigures();


            //_figure = _generator.NewFigure(_factory);
        }


        public int Scores => _scores;
        public bool IsGameOver => _isGameOver;


        //private IMap _map;
        //private IFigure _figure;
        private bool _isGameOver = false;
        private int _scores = 0;

        //private IFigureGenerator _generator;
        //private IFactoryFigure _factory;

        private IReadOnlyList<IMover> _movers;
        private Rotator _rotator;
        private Vector3 _vectorDrop;
        private int _delayDrop = 0;
        private int _counterFrameDrop = 0;

        //private Obstructions _obstructions;
        //private IStackFigures _stackFigures;

        public void GameOver()
        {
            //_figure.Dispose();
            //_stackFigures.Dispose();
        }


        

        public void NewFrame()
        { 
            /*bool isMoved = false;
            
            if (_isGameOver)
                return;

            _counterFrameDrop++;
            if (_counterFrameDrop == _delayDrop)
            {
                _counterFrameDrop = 0;
                isMoved = _figure.Move(_movers, _vectorDrop);
                if (!isMoved)
                {
                    _obstructions.Add(_figure.InnerPositions.ToArray());
                    _scores += _stackFigures.Add(_figure.InnerFigures.ToArray());

                    if (_stackFigures.MaxArea >= _map.Size.y - 1)
                    {
                        GameOver();
                        return;
                    }
                    _figure = _generator.NewFigure(_factory);
                }
            }*/
        }
        public void Move(Vector3Int moveParamsArg)
        {
            //_figure.Move(_movers, moveParamsArg);
        }
        public void Rotate(bool directionArg)
        {
            //_figure.Rotate(_rotator, directionArg);
        }
        public void SetCamera(Camera cameraArg)
        {
            //_map.SetCamera(cameraArg);
        }
    }
}
