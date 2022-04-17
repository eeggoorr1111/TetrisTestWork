using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using System;


namespace Tetris
{
    /// <summary>
    /// View в паттерне MVC
    /// </summary>
    public class View : MonoBehaviour
    {
        [Inject]
        protected void Constructor( IReadOnlyList<FigureTemplate> templatesFigureArg, 
                                    FigureView.Pool figuresPoolArg,
                                    UI uiArg,
                                    GameUi gameUiArg,
                                    [Inject(Id = "maxIdxGameMod")] int maxIdxGameModArg)
        {
            _figureTemplates = templatesFigureArg;
            _figuresPool = figuresPoolArg;
            _maxIdxGameMods = maxIdxGameModArg;
            _ui = uiArg;
            _gameUi = gameUiArg;
        }

        public bool IsExistsMonoB => this != null;


        protected event Action<bool> _onRotate;
        protected event Action<bool> _onMove;
        protected event Action _onGoToMenu;
        protected event Action<int> _onStartGame;
        protected IReadOnlyList<FigureTemplate> _figureTemplates;
        protected FigureView.Pool _figuresPool;
        protected FigureView _curFigure;
        protected int _maxIdxGameMods;
        protected UI _ui;
        protected GameUi _gameUi;



        public void StartCustom()
        {
            _gameUi.StartCustom();
            _ui.SetCameraAndBorders();
        }
        public void NewFigure(int idxTemplateArg, Vector3 posArg)
        {
            _curFigure = _figuresPool.Spawn(posArg, _figureTemplates[idxTemplateArg]);
            _curFigure.SubscribeOnRemovedAllBlocks(RemovedAllBlocks);
        }
        public void Exit()
        {
            Application.Quit();
        }
        public void StartGame(int idxModeArg)
        {
            int idxMode = idxModeArg;
            if (idxMode < 0 || idxMode > _maxIdxGameMods)
            {
                Debug.LogError($"Try play to game mode with index {idxMode}. Min index 0, max index {_maxIdxGameMods}. Play mode with index 0", this);
                idxMode = 0;
            }

            _ui.StartGame();
            if (_onStartGame != null)
                _onStartGame.Invoke(idxMode);
        }
        public void Subscribe(Action<bool> onRotateArg, Action<bool> onMoveArg, Action onGoToMenuArg, Action<int> onStartGameArg)
        {
            _onRotate -= onRotateArg;
            _onRotate += onRotateArg;

            _onMove -= onMoveArg;
            _onMove += onMoveArg;

            _onGoToMenu -= onGoToMenuArg;
            _onGoToMenu += onGoToMenuArg;

            _onStartGame -= onStartGameArg;
            _onStartGame += onStartGameArg;
        }
        public void Unsubscribe(Action<bool> onUserActionArg, Action<bool> onMoveArg, Action onGoToMenuArg, Action<int> onStartGameArg)
        {
            _onRotate -= onUserActionArg;
            _onMove -= onMoveArg;
            _onGoToMenu -= onGoToMenuArg;
            _onStartGame -= onStartGameArg;
        }
        public void GoToMenu()
        {
            _ui.EndGame();

            if (_onGoToMenu != null)
                _onGoToMenu.Invoke();
        }
        public void NextFrame(int scoresArg, Vector3 newPosFigureArg)
        {
            _ui.SetScores(scoresArg);
            _curFigure.Transf.position = newPosFigureArg;

            InputCheck();
        }


        protected void InputCheck()
        {
            if (_onRotate == null || _onMove == null)
                return;

            if (Input.GetKeyDown(KeyCode.D))
                _onMove.Invoke(true);

            else if (Input.GetKeyDown(KeyCode.A))
                _onMove.Invoke(false);

            else if(Input.GetKeyDown(KeyCode.Q))
                _onRotate.Invoke(false);

            else if (Input.GetKeyDown(KeyCode.E))
                _onRotate.Invoke(true);
        }
        protected void OnEnable()
        {
            if (_curFigure != null)
                _curFigure.SubscribeOnRemovedAllBlocks(RemovedAllBlocks);
        }
        protected void OnDisable()
        {
            if (_curFigure != null)
                _curFigure.UnsubscribeOnRemovedAllBlocks(RemovedAllBlocks);
        }
        protected void OnDestroy()
        {
            _onRotate = null;
            _onMove = null;
            _onGoToMenu = null;
            _onStartGame = null;
        }
        protected void RemovedAllBlocks(FigureView ofFigureArg)
        {
            _figuresPool.Despawn(ofFigureArg);
        }
    }
}

