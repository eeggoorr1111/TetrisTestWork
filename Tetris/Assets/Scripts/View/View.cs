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
                                    FigureView figureArg,
                                    Block.Pool poolBlockArg,
                                    Map mapArg,
                                    UI uiArg,
                                    GameUi gameUiArg,
                                    [Inject(Id = "maxIdxGameMod")] int maxIdxGameModArg)
        {
            _figureTemplates = templatesFigureArg;
            _maxIdxGameMods = maxIdxGameModArg;
            _ui = uiArg;
            _gameUi = gameUiArg;
            _heap = new Dictionary<Vector2Int, Block>();
            _figure = figureArg;
            _poolBlocks = poolBlockArg;
            _map = mapArg;
            _lastRange = 0;
        }

        public bool IsExistsMonoB => this != null;


        protected event Action<bool> _onRotate;
        protected event Action<bool> _onMove;
        protected event Action<bool> _onBoostFall;
        protected event Action _onGoToMenu;
        protected event Action<int> _onStartGame;
        protected IReadOnlyList<FigureTemplate> _figureTemplates;
        protected Dictionary<Vector2Int, Block> _heap;
        protected FigureView _figure;
        protected int _maxIdxGameMods;
        protected UI _ui;
        protected GameUi _gameUi;
        protected Block.Pool _poolBlocks;
        protected int _lastRange;
        protected Map _map;
        protected Transform _transf;



        public void StartCustom()
        {
            _figure.StartCustom();
            _gameUi.StartCustom();
            _ui.SetCameraAndBorders();

            _transf = GetComponent<Transform>();
        }
        public void NewFigure(int idxTemplateArg, Vector3 posArg)
        {
            if (_figure.Blocks.WithItems())
                AddToHeap(_figure.Blocks);

            _figure.NewFigrue(posArg, _figureTemplates[idxTemplateArg]);
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
        public void Subscribe(Action<bool> onRotateArg, Action<bool> onMoveArg, Action<bool> onBoostFallArg, Action onGoToMenuArg, Action<int> onStartGameArg)
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
        public void Unsubscribe(Action<bool> onUserActionArg, Action<bool> onMoveArg, Action<bool> onBoostFallArg, Action onGoToMenuArg, Action<int> onStartGameArg)
        {
            _onRotate -= onUserActionArg;
            _onMove -= onMoveArg;
            _onBoostFall -= onBoostFallArg;
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
            _figure.Transf.position = newPosFigureArg;

            InputCheck();
        }
        public void Delete(IReadOnlyList<int> rangesArg)
        {
            int putDown = 0;
            for (int y = rangesArg[0]; y <= _lastRange; y++)
            {
                bool remove = rangesArg.Contains(y);
                if (remove)
                    putDown++;

                for (int x = 0; x < _map.SizeMap.x; x++)
                {
                    Vector2Int cell = new Vector2Int(x, y);

                    if (_heap.ContainsKey(cell))
                    {
                        if (remove)
                        {
                            _poolBlocks.Despawn(_heap[cell]);
                            _heap.Remove(cell);
                        }
                        else
                            PutDownBlock(cell, putDown);
                    }
                }
            }

            _lastRange -= rangesArg.Count;
        }


        protected void PutDownBlock(Vector2Int cellArg, int putDownArg)
        {
            Vector2Int newCell = new Vector2Int(cellArg.x, cellArg.y - putDownArg);
            Block block = _heap[cellArg];

            block.Transf.position = new Vector3(newCell.x, newCell.y, block.Transf.position.z);
            
            _heap.Remove(cellArg);
            _heap[newCell] = block;
        }
        protected void AddToHeap(IReadOnlyList<Block> blocksArg)
        {
            foreach (var block in blocksArg)
            {
                int blockX = Mathf.RoundToInt(block.Transf.position.x);
                int blockY = Mathf.RoundToInt(block.Transf.position.y);
                Vector2Int pos = new Vector2Int(blockX, blockY);

                if (blockY > _lastRange)
                    _lastRange = blockY;

                _heap[pos] = block;
                block.Transf.SetParent(_transf);
            }
        }
        protected void InputCheck()
        {
            if (_onRotate == null || _onMove == null || _onBoostFall == null)
                return;

            if (Input.GetKeyDown(KeyCode.D))
                _onMove.Invoke(true);

            else if (Input.GetKeyDown(KeyCode.A))
                _onMove.Invoke(false);

            else if (Input.GetKeyDown(KeyCode.Q))
                _onRotate.Invoke(false);

            else if (Input.GetKeyDown(KeyCode.E))
                _onRotate.Invoke(true);

            else if (Input.GetKeyDown(KeyCode.S))
                _onBoostFall.Invoke(true);

            /// a separate if in case of simultaneously down, for example D key and up S key
            if (Input.GetKeyUp(KeyCode.S))
                _onBoostFall.Invoke(false);
        }
        protected void OnDestroy()
        {
            _onRotate = null;
            _onMove = null;
            _onGoToMenu = null;
            _onStartGame = null;
        }
    }
}

