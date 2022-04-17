using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Tetris
{
    /// <summary>
    /// Он же Controller в рамках паттерна MVC
    /// </summary>
    public class Main : MonoBehaviour
    {
        [Inject]
        protected void Constructor(View viewArg, Model modelArg)
        {
            _model = modelArg;
            _view = viewArg;
            _isGame = false;
        }


        protected Model _model;
        protected View _view;
        protected Vector3 _posFigure;
        protected bool _isGame;


        protected void Start()
        {
            _view.StartCustom();
        }
        protected void Update()
        {
            if (_isGame)
            {
                FigureModel newFigure = null;
                bool isGameOver;

                _model.ContinueFallFigure(false, ref newFigure, out isGameOver);
                if (isGameOver)
                    GameOver();
                else
                {
                    if (newFigure != null)
                        _view.NewFigure(newFigure.IdxTemplate, newFigure.Pivot);
                    _view.NextFrame(_model.Scores, _model.Figure.Pivot);
                }
            }
        }
        protected void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            if (_model != null)
            {
                if (_model.Figure != null && !_model.Figure.BoundsBlocks.IsEmpty())
                    foreach (var block in _model.Figure.BoundsBlocks)
                        Gizmos.DrawCube(block.center, block.size);

                Gizmos.DrawSphere(_model.Map.MinPoint, 0.1f);
                Gizmos.DrawSphere(_model.Map.MaxPoint, 0.1f);

                if (_model.HeapFigures != null)
                    Gizmos.DrawCube(_model.HeapFigures.Bounds.center, _model.HeapFigures.Bounds.size);
            }
        }
        protected void OnEnable()
        {
            _view.Subscribe(OnRotate, _model.MoveFigure, GameOver, StartGame);
        }
        protected void OnDisable()
        {
            if (_view != null && _view.IsExistsMonoB)
                _view.Unsubscribe(OnRotate, _model.MoveFigure, GameOver, StartGame);
        }

        protected void OnRotate(bool byClockwiseArg)
        {

        }
        protected void GameOver()
        {
            _isGame = false;
        }
        protected void StartGame(int indexGameModeArg)
        {
            FigureModel figure = null;

            _isGame = true;
            _model.StartGame(indexGameModeArg, ref figure);
            _view.NewFigure(figure.IdxTemplate, figure.Pivot);
        }
    }
}

