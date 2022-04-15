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
                                    Camera cameraArg,
                                    UI uiArg,
                                    [Inject(Id = "maxIdxGameMod")] int maxIdxGameMod)
        {
            _figureTemplates = templatesFigureArg;
            _figuresPool = figuresPoolArg;
            _maxIdxGameMods = maxIdxGameMod;
            _camera = cameraArg;
            _ui = uiArg;
        }

        public bool IsExistsMonoB => this != null;


        protected event Action<UserAction> _onUserAction;
        protected event Action _onGoToMenu;
        protected event Action<int> _onStartGame;
        protected IReadOnlyList<FigureTemplate> _figureTemplates;
        protected FigureView.Pool _figuresPool;
        protected FigureView _curFigure;
        protected int _maxIdxGameMods;
        protected Camera _camera;
        protected UI _ui;



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
        public void Subscribe(Action<UserAction> onUserActionArg, Action onGoToMenuArg, Action<int> onStartGameArg)
        {
            if (onUserActionArg != null)
            {
                _onUserAction -= onUserActionArg;
                _onUserAction += onUserActionArg;
            }
           
            if (onGoToMenuArg != null)
            {
                _onGoToMenu -= onGoToMenuArg;
                _onGoToMenu += onGoToMenuArg;
            }
            
            if (onStartGameArg != null)
            {
                _onStartGame -= onStartGameArg;
                _onStartGame += onStartGameArg;
            }
        }
        public void Unsubscribe(Action<UserAction> onUserActionArg, Action onGoToMenuArg, Action<int> onStartGameArg)
        {
            if (onUserActionArg != null)
                _onUserAction -= onUserActionArg;

            if (onGoToMenuArg != null)
                _onGoToMenu -= onGoToMenuArg;

            if (onStartGameArg != null)
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
            if (_onUserAction == null)
                return;

            if (Input.GetKeyDown(KeyCode.D))
                _onUserAction.Invoke(UserAction.ToMove(new Vector3Int(1, 0, 0)));

            else if (Input.GetKeyDown(KeyCode.A))
                _onUserAction.Invoke(UserAction.ToMove(new Vector3Int(-1, 0, 0)));

            else if(Input.GetKeyDown(KeyCode.Q))
                _onUserAction.Invoke(UserAction.ToRotate(RotateKey.CtrClockwise));

            else if (Input.GetKeyDown(KeyCode.E))
                _onUserAction.Invoke(UserAction.ToRotate(RotateKey.Clockwise));
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
            _onUserAction = null;
            _onGoToMenu = null;
            _onStartGame = null;
        }
        protected void RemovedAllBlocks(FigureView ofFigureArg)
        {
            _figuresPool.Despawn(ofFigureArg);
        }
    }
}

