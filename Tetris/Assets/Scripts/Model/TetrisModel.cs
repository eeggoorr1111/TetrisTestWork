using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

namespace Tetris.Model
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


        public FigureModel Figure { get; private set; }
        public int Scores { get; private set; }


        private readonly IReadOnlyList<Transformator> _movers;
        private readonly HeapFigures _heapFigures;
        private readonly FigureGenerator _generator;
        private readonly MapData _map;
        private Transformator _mover;


        public void StartGame(int indexMoverArg, ref FigureModel newFigure)
        {
            _mover = _movers[indexMoverArg];
            Figure = _generator.NewFigure();

            newFigure = Figure;
        }
        public void ContinueFallFigure(bool boostedFallArg, ref FigureModel newFigure, ref IReadOnlyList<int> deleteRanges, out bool isGameOver)
        {
            isGameOver = false;
            if (!Figure.ToFall(_mover, boostedFallArg))
            {
                deleteRanges = _heapFigures.Add(Figure);

                Scores += deleteRanges.Count;
                if (_heapFigures.TopByY > _map.TopByY)
                {
                    isGameOver = true;
                    GameOver();
                }
                else
                {
                    Figure = _generator.NewFigure();
                    newFigure = Figure;
                }
            }
        }
        public void MoveFigure(bool toRightArg)
        {
            Figure.ToMoveToSide(_mover, toRightArg);
        }
        public void Rotate()
        {
            Figure.ToRotate(_mover);
        }


        protected void GameOver()
        {
            //_figure.Dispose();
            //_stackFigures.Dispose();
        }
    }
}
