using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Tetris.View;

namespace Tetris
{
    /// <summary>
    /// Он же Controller в рамках паттерна MVC
    /// </summary>
    public class Main : MonoBehaviour
    {
        [Inject]
        protected void Constructor(TetrisView viewArg, TetrisModel modelArg)
        {
            _model = modelArg;
            _view = viewArg;
            _gameStatus = GameStatusKey.Menu;
        }


        protected TetrisModel _model;
        protected TetrisView _view;
        protected Vector3 _posFigure;
        private GameStatusKey _gameStatus;
        protected bool _isBoostingFall;


        protected void OnApplicationFocus(bool focusArg)
        {
            if (!focusArg && _gameStatus == GameStatusKey.Game)
            {
                _isBoostingFall = false;
                _gameStatus = GameStatusKey.Pause;
            }
            else if (focusArg && _gameStatus == GameStatusKey.Pause)
                _gameStatus = GameStatusKey.Game;
        }
        protected void Start()
        {
            _view.StartCustom();
        }
        protected void Update()
        {
            if (_gameStatus == GameStatusKey.Game)
            {
                FigureModel newFigure = null;
                IReadOnlyList<int> ranges = null;
                bool isGameOver;

                _model.ContinueFallFigure(_isBoostingFall, ref newFigure, ref ranges, out isGameOver);
                
                if (isGameOver)
                    GameOver();
                else
                {
                    if (newFigure != null)
                        _view.NewFigure(newFigure.IdxTemplate, newFigure.Pivot);
                    _view.NextFrame(_model.Scores, _model.Figure.Pivot, _model.Figure.Rotate);
                }

                if (ranges.WithItems())
                    _view.Delete(ranges);
            }
        }
        protected void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            if (_model != null)
            {
                if (_model.Figure != null && !_model.Figure.Blocks.IsEmpty())
                {
                    Gizmos.DrawCube(_model.Figure.Bounds.center, _model.Figure.Bounds.size);
                    foreach (var block in _model.Figure.Blocks)
                        Gizmos.DrawCube(block.center, block.size);
                }
                    

                Gizmos.DrawSphere(_model.Map.MinPoint, 0.1f);
                Gizmos.DrawSphere(_model.Map.MaxPoint, 0.1f);

                if (_model.HeapFigures != null)
                    Gizmos.DrawCube(_model.HeapFigures.Bounds.center, _model.HeapFigures.Bounds.size);
            }
        }
        protected void OnEnable()
        {
            _view.Subscribe(_model.Rotate, _model.MoveFigure, OnBoostFall, GameOver, StartGame);
        }
        protected void OnDisable()
        {
            if (_view != null && _view.IsExistsMonoB)
                _view.Unsubscribe(_model.Rotate, _model.MoveFigure, OnBoostFall, GameOver, StartGame);
        }
        protected void OnBoostFall(bool statusArg)
        {
            _isBoostingFall = statusArg;
        }
        protected void GameOver()
        {
            _gameStatus = GameStatusKey.Menu;
        }
        protected void StartGame(int indexGameModeArg)
        {
            FigureModel figure = null;

            _isBoostingFall = false;
            _gameStatus = GameStatusKey.Game;
            _model.StartGame(indexGameModeArg, ref figure);
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

