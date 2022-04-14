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
        }


        protected Model _model;
        protected View _view;
        protected Vector3 _posFigure;


        [ContextMenu("Craete figure with 1 tempalte")]
        protected void Test()
        {
            _posFigure = Vector3.up * 3;
            _view.NewFigure(0, _posFigure);
        }
        protected void Update()
        {
            _model.NewFrame();
            if (_model.IsGameOver)
                GameOver();

            _view.UpdateCustom();
            _view.SetScores(_model.Scores);

            _posFigure += Vector3.down * Time.deltaTime * 2;
            _view.MoveFigure(_posFigure);

        }
        protected void OnEnable()
        {
            _view.Subscribe(NewUserAction, GameOver, StartGame);
        }
        protected void OnDisable()
        {
            if (_view != null && _view.IsExistsMonoB)
                _view.Unsubscribe(NewUserAction, GameOver, StartGame);
        }

        protected void NewUserAction(UserAction actionArg)
        {

        }
        protected void GameOver()
        {

        }
        protected void StartGame(int indexGameModeArg)
        {

        }
    }
}

