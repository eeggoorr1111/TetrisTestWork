using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using System;


namespace Tetris.View
{
    /// <summary>
    /// View в паттерне MVC
    /// </summary>
    [RequireComponent(typeof(UI))]
    public sealed class TetrisView : MonoBehaviour
    {
        [Inject]
        private void Constructor(   FigureBlockedWall figureArg,
                                    FigureThroughtWall figureThroughtWallArg,
                                    ILevelsParams lvlsParamsArg,
                                    Block.Pool poolBlockArg,
                                    MapData mapArg)
        {
            _figureBlockedWall = figureArg;
            _figureThroughtWall = figureThroughtWallArg;
            _lvlsParams = lvlsParamsArg;
            _poolBlocks = poolBlockArg;
            _map = mapArg;
        }

        public bool IsExistsMonoB => this != null;


        [SerializeField] private Material _blockMaterial;
        [SerializeField] private HeapBlocks _heap;
        private event Action _onRotate;
        private event Action<bool> _onMove;
        private event Action<bool> _onBoostFall;
        private event Action _onGoToMenu;
        private event Action<int> _onStartGame;
        private FigureThroughtWall _figureThroughtWall;
        private FigureBlockedWall _figureBlockedWall;
        private IFigure _figure;
        private ILevelsParams _lvlsParams;
        private UI _ui;
        private Block.Pool _poolBlocks;
        private MapData _map;
        private Transform _transf;


        public void StartCustom()
        {
            _ui = GetComponent<UI>();
            _ui.StartCustom(_map);

            _ui.BtnLvl1.onClick.AddListener(() => StartGame(0));
            _ui.BtnLvl2.onClick.AddListener(() => StartGame(1));
            _ui.BtnGoToMenu.onClick.AddListener(GoToMenu);

            _figureBlockedWall.StartCustom();
            _figureThroughtWall.StartCustom();

            _heap.StartCustom(_map, _poolBlocks);

            _transf = GetComponent<Transform>();
        }
        public void NewFigure(int idxTemplateArg, Vector3 posArg)
        {
            if (_lvlsParams.Current.CanMoveThroughtWall)
                _figure = _figureThroughtWall;
            else
                _figure = _figureBlockedWall;

            if (_figure.Blocks.WithItems())
                _heap.Add(_figure.Blocks);

            _figure.NewFigrue(posArg, _lvlsParams.Current.FiguresTemplates[idxTemplateArg]);
        }
        public void Exit()
        {
            Application.Quit();
        }
        public void StartGame(int levelArg)
        {
            int level = levelArg;

            if (level < 0 || level > _lvlsParams.MaxLevel)
            {
                Debug.LogError($"Try play level {level}. Min level 0, max level {_lvlsParams.MaxLevel}. Play level 0", this);
                level = 0;
            }

            if (_onStartGame != null)
                _onStartGame.Invoke(level);

            Color boundsMap = new Color(_map.MinPoint.x, _map.MinPoint.y, _map.MaxPoint.x, _map.MaxPoint.y);
            _blockMaterial.SetColor("_BoundsMap", boundsMap);

            _ui.StartGame();
        }
        public void Subscribe(Action onRotateArg, Action<bool> onMoveArg, Action<bool> onBoostFallArg, Action onGoToMenuArg, Action<int> onStartGameArg)
        {
            _onRotate -= onRotateArg;
            _onRotate += onRotateArg;

            _onMove -= onMoveArg;
            _onMove += onMoveArg;

            _onBoostFall -= onBoostFallArg;
            _onBoostFall += onBoostFallArg;

            _onGoToMenu -= onGoToMenuArg;
            _onGoToMenu += onGoToMenuArg;

            _onStartGame -= onStartGameArg;
            _onStartGame += onStartGameArg;
        }
        public void Unsubscribe(Action onRotateArg, Action<bool> onMoveArg, Action<bool> onBoostFallArg, Action onGoToMenuArg, Action<int> onStartGameArg)
        {
            _onRotate -= onRotateArg;
            _onMove -= onMoveArg;
            _onBoostFall -= onBoostFallArg;
            _onGoToMenu -= onGoToMenuArg;
            _onStartGame -= onStartGameArg;
        }
        public void GoToMenu()
        {
            EndGame();

            if (_onGoToMenu != null)
                _onGoToMenu.Invoke();
        }
        public void EndGame()
        {
            _ui.EndGame();
            _heap.EndGame();
            _figureBlockedWall.EndGame();
            _figureThroughtWall.EndGame();
        }
        public void NextFrame(int scoresArg, Vector3 newPosFigureArg, Quaternion rotateArg)
        {
            _ui.SetScores(scoresArg);
            _figure.Transform(newPosFigureArg, rotateArg);

            InputCheck();
        }
        public void Delete(IReadOnlyList<int> rangesArg)
        {
            _heap.Delete(rangesArg);
        }
        


        private void OnValidate()
        {
            if (_blockMaterial == null)
                Debug.LogError("For block not setted material", this);

            if (_heap == null)
                Debug.LogError("Not setted heap object", this);
        }
        private void InputCheck()
        {
            if (_onRotate == null || _onMove == null || _onBoostFall == null)
                return;

            if (Input.GetKeyDown(KeyCode.D))
                _onMove.Invoke(true);

            else if (Input.GetKeyDown(KeyCode.A))
                _onMove.Invoke(false);

            else if (Input.GetKeyDown(KeyCode.W))
                _onRotate.Invoke();

            else if (Input.GetKeyDown(KeyCode.S))
                _onBoostFall.Invoke(true);

            /// a separate if in case of simultaneously down, for example D key and up S key
            if (Input.GetKeyUp(KeyCode.S))
                _onBoostFall.Invoke(false);
        }
        private void OnDestroy()
        {
            _onRotate = null;
            _onMove = null;
            _onGoToMenu = null;
            _onStartGame = null;
        }
    }
}

