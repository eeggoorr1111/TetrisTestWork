﻿using System.Collections;
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
        private void Constructor(   FigureBlockedWall figureArg,
                                    FigureThroughtWall figureThroughtWallArg,
                                    ILevelsParams lvlsParamsArg,
                                    Block.Pool poolBlockArg,
                                    MapData mapArg,
                                    UI uiArg,
                                    BlockParams blockParamsArg)
        {
            _figureBlockedWall = figureArg;
            _figureThroughtWall = figureThroughtWallArg;
            _lvlsParams = lvlsParamsArg;
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
        private FigureThroughtWall _figureThroughtWall;
        private FigureBlockedWall _figureBlockedWall;
        private Dictionary<Vector2Int, Block> _heap;
        private IFigure _figure;
        private ILevelsParams _lvlsParams;
        private UI _ui;
        private Block.Pool _poolBlocks;
        private int _lastRange;
        private MapData _map;
        private Transform _transf;
        private BlockParams _blockParams;



        public void StartCustom()
        {
            _figureBlockedWall.StartCustom();
            _figureThroughtWall.StartCustom();
            _ui.StartCustom();

            _transf = GetComponent<Transform>();
        }
        public void NewFigure(int idxTemplateArg, Vector3 posArg)
        {
            if (_lvlsParams.Current.CanMoveThroughtWall)
                _figure = _figureThroughtWall;
            else
                _figure = _figureBlockedWall;

            if (_figure.Blocks.WithItems())
                AddToHeap(_figure.Blocks);

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
            _blockParams.Material.SetColor("_BoundsMap", boundsMap);

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

