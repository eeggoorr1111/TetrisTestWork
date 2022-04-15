﻿using System;
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
        public Model(   Rotator rotatorArg, 
                        IReadOnlyList<Mover> moversArg, 
                        HeapFigures heapFiguresArg, 
                        Map mapArg,
                        FigureGenerator generatorArg)
        {
            _movers = moversArg;
            _rotator = rotatorArg;
            _map = mapArg;
            _heapFigures = heapFiguresArg;
            _generator = generatorArg;
        }


        public FigureModel Figure => _figure;
        public int Scores => _scores;


        private int _scores = 0;
        private IReadOnlyList<Mover> _movers;
        private Rotator _rotator;
        private Mover _mover;
        private FigureModel _figure;
        private HeapFigures _heapFigures;
        private FigureGenerator _generator;
        private Map _map;

        
        public void StartGame(int indexMoverArg, ref FigureModel newFigure)
        {
            _mover = _movers[indexMoverArg];
            _figure = _generator.NewFigure();

            newFigure = _figure;
        }
        public void ContinueFallFigure(bool boostedFallArg, ref FigureModel newFigure, out bool isGameOver)
        {
            isGameOver = false;
            if (!_figure.ToFall(_mover, boostedFallArg))
            {
                int oldRangesCount = _heapFigures.Ranges;
                int newRangesCount = _heapFigures.Add(_figure);

                if (newRangesCount < oldRangesCount)
                    _scores++;
                else if (_heapFigures.TopByY > _map.Bounds.max.y)
                {
                    isGameOver = true;
                    GameOver();
                }
                else
                {
                    _figure = _generator.NewFigure();
                    newFigure = _figure;
                }
            }
        }
        public void MoveFigure(bool toRightArg)
        {
            _figure.MoveToSide(_mover);
        }


        protected void GameOver()
        {
            //_figure.Dispose();
            //_stackFigures.Dispose();
        }
    }
}
