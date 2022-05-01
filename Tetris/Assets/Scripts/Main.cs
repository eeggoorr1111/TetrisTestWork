using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Tetris.View;
using Tetris.Model;

namespace Tetris
{
    /// <summary>
    /// Он же Controller в рамках паттерна MVC
    /// </summary>
    public sealed class Main : MonoBehaviour
    {
        [Inject]
        private void Constructor(TetrisView viewArg, TetrisModel modelArg, LevelsParams lvlsParamsArg)
        {
            _model = modelArg;
            _view = viewArg;
            _lvlsParams = lvlsParamsArg;
            _gameStatus = GameStatusKey.Menu;
        }


        private TetrisModel _model;
        private TetrisView _view;
        private GameStatusKey _gameStatus;
        private LevelsParams _lvlsParams;
        private bool _isBoostingFall;


        private void OnApplicationFocus(bool focusArg)
        {
            if (!focusArg && _gameStatus == GameStatusKey.Game)
            {
                _isBoostingFall = false;
                _gameStatus = GameStatusKey.Pause;
            }
            else if (focusArg && _gameStatus == GameStatusKey.Pause)
                _gameStatus = GameStatusKey.Game;
        }
        private void Start()
        {
            _view.StartCustom();
        }
        private void Update()
        {
            if (_gameStatus == GameStatusKey.Game)
            {
                FigureModel newFigure = null;
                IReadOnlyList<int> deleteRanges = null;
                bool isGameOver;

                _model.ContinueFallFigure(_isBoostingFall, ref newFigure, ref deleteRanges, out isGameOver);
                
                if (isGameOver)
                    EndGame();
                else
                {
                    if (newFigure != null)
                        _view.NewFigure(newFigure.IdxTemplate, newFigure.Pivot);
                    _view.NextFrame(_model.Scores, _model.Figure.Pivot, _model.Figure.Rotate);
                }

                if (deleteRanges.WithItems())
                    _view.Delete(deleteRanges);
            }
        }
        private void OnEnable()
        {
            _view.Subscribe(_model.Rotate, _model.MoveFigure, OnBoostFall, GoToMenu, StartGame);
        }
        private void OnDisable()
        {
            if (_view != null && _view.IsExistsMonoB)
                _view.Unsubscribe(_model.Rotate, _model.MoveFigure, OnBoostFall, GoToMenu, StartGame);
        }
        private void OnBoostFall(bool statusArg)
        {
            _isBoostingFall = statusArg;
        }
        private void GoToMenu()
        {
            _gameStatus = GameStatusKey.Menu;
            _model.EndGame();
        }
        private void EndGame()
        {
            _gameStatus = GameStatusKey.Menu;
            _view.EndGame();
        }
        private void StartGame(int levelArg)
        {
            FigureModel figure = null;

            _lvlsParams.CurrentLevel = levelArg;
            _isBoostingFall = false;
            _gameStatus = GameStatusKey.Game;
            _model.StartGame(ref figure);
            _view.NewFigure(figure.IdxTemplate, figure.Pivot);
        }


        private enum GameStatusKey
        {
            Game,
            Pause,
            Menu
        }
    }
}

