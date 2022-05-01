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
        public TetrisModel( TransformatorThroughtWall transfThroughtWallArg,
                            TransfBlockedWall transfBlockedWallArg,
                            HeapFigures heapFiguresArg, 
                            MapData mapArg,
                            FigureGenerator generatorArg,
                            ILevelsParams lvlsParamsArg)
        {
            _transfThroughtWall = transfThroughtWallArg;
            _transfBlockedWall = transfBlockedWallArg;
            _map = mapArg;
            _heapFigures = heapFiguresArg;
            _generator = generatorArg;
            _lvlsParams = lvlsParamsArg;
        }


        public FigureModel Figure { get; private set; }
        public int Scores { get; private set; }


        private readonly TransformatorThroughtWall _transfThroughtWall;
        private readonly TransfBlockedWall _transfBlockedWall;
        private readonly HeapFigures _heapFigures;
        private readonly FigureGenerator _generator;
        private readonly MapData _map;
        private readonly ILevelsParams _lvlsParams;
        private Transformator _transformator;


        public void StartGame(ref FigureModel newFigure)
        {
            if (_lvlsParams.Current.CanMoveThroughtWall)
                _transformator = _transfThroughtWall;
            else
                _transformator = _transfBlockedWall;

            Figure = _generator.NewFigure();

            newFigure = Figure;
        }
        public void ContinueFallFigure(bool boostedFallArg, ref FigureModel newFigure, ref IReadOnlyList<int> deleteRanges, out bool isGameOver)
        {
            isGameOver = false;
            if (!Figure.ToFall(_transformator, boostedFallArg))
            {
                deleteRanges = _heapFigures.Add(Figure);

                Scores += deleteRanges.Count;
                if (_heapFigures.TopByY > _map.TopByY)
                {
                    isGameOver = true;
                    EndGame();
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
            Figure.ToMoveToSide(_transformator, toRightArg);
        }
        public void Rotate()
        {
            Figure.ToRotate(_transformator);
        }
        public void EndGame()
        {
            _heapFigures.Clear();
            Figure = null;
            Scores = 0;
        }
    }
}
