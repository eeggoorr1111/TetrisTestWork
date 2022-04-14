using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using System;
using TMPro;


namespace Tetris
{
    /// <summary>
    /// View в паттерне MVC
    /// </summary>
    public class View : MonoBehaviour
    {
        [Inject]
        protected void Constructor(IReadOnlyList<FigureTemplate> templatesFigureArg, FigureView.Pool figuresPoolArg)
        {
            _figureTemplates = templatesFigureArg;
            _figuresPool = figuresPoolArg;

            if (_lblScores == null)
                Debug.LogError("Label scores is null", this);

            if (_camera == null)
                Debug.LogError("Camera is null", this);

            if (_gameUi == null)
                Debug.LogError("Game UI is null", this);

            if (_menu == null)
                Debug.LogError("Menu is null", this);
        }

        public bool IsExistsMonoB => this != null;


        [SerializeField] protected TextMeshProUGUI _lblScores;
        [SerializeField] protected Camera _camera;
        [SerializeField] protected Canvas _gameUi;
        [SerializeField] protected Canvas _menu;



        protected event Action<UserAction> _onUserAction;
        protected event Action _onGoToMenu;
        protected event Action<int> _onStartGame;
        protected IReadOnlyList<FigureTemplate> _figureTemplates;
        protected FigureView.Pool _figuresPool;
        protected FigureView _curFigure;


        public void NewFigure(int idxTemplateArg, Vector3 posArg)
        {
            if (_figureTemplates.IsValidIndex(idxTemplateArg))
            {
                _curFigure = _figuresPool.Spawn(posArg, _figureTemplates[idxTemplateArg]);
                _curFigure.SubscribeOnRemovedAllBlocks(RemovedAllBlocks);
            }
            else
                Debug.LogError($"Index template {idxTemplateArg} not valid. Min index 0. Max index {_figureTemplates.Count}", this);
        }
        public void MoveFigure(Vector3 newPosArg)
        {
            if (_curFigure == null)
            {
                Debug.LogError("Try move not setted figure", this);
                return;
            }

            _curFigure.Transf.position = newPosArg;
        }
        public void Exit()
        {
            Application.Quit();
        }
        public void StartGame(int indexModeArg)
        {
            _menu.enabled = false;
            _gameUi.enabled = true;

            if (_onStartGame != null)
                _onStartGame.Invoke(indexModeArg);
        }
        public void SetScores(int scoresArg)
        {
            if (_lblScores != null)
                _lblScores.text = scoresArg.ToString();
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
            _menu.enabled = true;
            _gameUi.enabled = false;

            if (_onGoToMenu != null)
                _onGoToMenu.Invoke();
        }
        public void UpdateCustom()
        {
            if (_onUserAction == null)
                return;

            KeyCode keyDown = KeyCode.D;
            switch (keyDown)
            {
                case KeyCode.D:
                    _onUserAction.Invoke(UserAction.ToMove(new Vector3Int(1, 0, 0)));
                    break;

                case KeyCode.A:
                    _onUserAction.Invoke(UserAction.ToMove(new Vector3Int(-1, 0, 0)));
                    break;

                case KeyCode.Q:
                    _onUserAction.Invoke(UserAction.ToRotate(RotateKey.CtrClockwise));
                    break;

                case KeyCode.E:
                    _onUserAction.Invoke(UserAction.ToRotate(RotateKey.Clockwise));
                    break;
            }
        }


        protected void OnEnable()
        {
            if (_curFigure != null)
                _curFigure.SubscribeOnRemovedAllBlocks(RemovedAllBlocks);
        }
        protected void OnDisable()
        {
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

