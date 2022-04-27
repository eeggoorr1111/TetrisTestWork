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
    public sealed class TetrisView : MonoBehaviour
    {
        [Inject]
        private void Constructor( IReadOnlyList<FigureTemplate> templatesFigureArg,
                                    IReadOnlyList<IFigure> figuresArg,
                                    Block.Pool poolBlockArg,
                                    MapData mapArg,
                                    UI uiArg,
                                    BlockParams blockParamsArg)
        {
            _figureTemplates = templatesFigureArg;
            _figures = figuresArg;
            _ui = uiArg;
            _heap = new Dictionary<Vector2Int, Block>();
            _poolBlocks = poolBlockArg;
            _map = mapArg;
            _blockParams = blockParamsArg;
            _lastRange = 0;
        }

        public bool IsExistsMonoB => this != null;


        private event Action _onRotate;
        private event Action<bool> _onMove;
        private event Action<bool> _onBoostFall;
        private event Action _onGoToMenu;
        private event Action<int> _onStartGame;
        private IReadOnlyList<FigureTemplate> _figureTemplates;
        private IReadOnlyList<IFigure> _figures;
        private Dictionary<Vector2Int, Block> _heap;
        private IFigure _figure;
        private UI _ui;
        private Block.Pool _poolBlocks;
        private int _lastRange;
        private MapData _map;
        private Transform _transf;
        private BlockParams _blockParams;



        public void StartCustom()
        {
            foreach (var figure in _figures)
                figure.StartCustom();

            _ui.SetCameraAndBorders();
            _transf = GetComponent<Transform>();

            Color boundsMap = new Color(_map.MinPoint.x, _map.MinPoint.y, _map.MaxPoint.x, _map.MaxPoint.y);
            _blockParams.Material.SetColor("_BoundsMap", boundsMap);
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
            int maxIdxGameMods = Math.Min(_figureTemplates.Count, _figures.Count);

            if (idxMode < 0 || idxMode > maxIdxGameMods)
            {
                Debug.LogError($"Try play to game mode with index {idxMode}. Min index 0, max index {maxIdxGameMods}. Play mode with index 0", this);
                idxMode = 0;
            }

            _figure = _figures[idxMode];
            _ui.StartGame();
            if (_onStartGame != null)
                _onStartGame.Invoke(idxMode);
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
            _ui.EndGame();

            if (_onGoToMenu != null)
                _onGoToMenu.Invoke();
        }
        public void NextFrame(int scoresArg, Vector3 newPosFigureArg, Quaternion rotateArg)
        {
            _ui.SetScores(scoresArg);
            _figure.Transform(newPosFigureArg, rotateArg);

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

                for (int x = 0; x < _map.CountCells.x; x++)
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


        private void PutDownBlock(Vector2Int cellArg, int putDownArg)
        {
            Vector2Int newCell = new Vector2Int(cellArg.x, cellArg.y - putDownArg);
            Block block = _heap[cellArg];

            block.Transf.position = new Vector3(newCell.x, newCell.y, block.Transf.position.z);
            
            _heap.Remove(cellArg);
            _heap[newCell] = block;
        }
        private void AddToHeap(IReadOnlyList<Block> blocksArg)
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

