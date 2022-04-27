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
    public class TetrisModel
    {
        public TetrisModel(   IReadOnlyList<Transformator> moversArg, 
                        HeapFigures heapFiguresArg, 
                        MapData mapArg,
                        FigureGenerator generatorArg)
        {
            _movers = moversArg;
            _map = mapArg;
            _heapFigures = heapFiguresArg;
            _generator = generatorArg;
        }


        public FigureModel Figure => _figure;
        public HeapFigures HeapFigures => _heapFigures;
        public int Scores => _scores;
        public MapData Map => _map;


        private int _scores = 0;
        private IReadOnlyList<Transformator> _movers;
        private Transformator _mover;
        private FigureModel _figure;
        private HeapFigures _heapFigures;
        private FigureGenerator _generator;
        private MapData _map;

        
        public void StartGame(int indexMoverArg, ref FigureModel newFigure)
        {
            _mover = _movers[indexMoverArg];
            _figure = _generator.NewFigure();

            newFigure = _figure;
        }
        public void ContinueFallFigure(bool boostedFallArg, ref FigureModel newFigure, ref IReadOnlyList<int> deleteRanges, out bool isGameOver)
        {
            isGameOver = false;
            if (!_figure.ToFall(_mover, boostedFallArg))
            {
                deleteRanges = _heapFigures.Add(_figure);

                _scores += deleteRanges.Count;
                if (_heapFigures.TopByY > _map.TopByY)
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
            _figure.ToMoveToSide(_mover, toRightArg);
        }
        public void Rotate()
        {
            _figure.ToRotate(_mover);
        }


        protected void GameOver()
        {
            //_figure.Dispose();
            //_stackFigures.Dispose();
        }
    }
}
