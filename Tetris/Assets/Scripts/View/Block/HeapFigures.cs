using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tetris.View
{
    public sealed class HeapFigures : MonoBehaviour
    {
        private Dictionary<Vector2Int, Block> _heap;
        private int _lastRange;
        private Block.Pool _poolBlocks;
        private Transform _transf;
        private MapData _map;


        public void StartCustom(MapData mapArg, Block.Pool poolBlocksArg)
        {
            _heap = new Dictionary<Vector2Int, Block>();
            _lastRange = -1;
            _poolBlocks = poolBlocksArg;
            _transf = GetComponent<Transform>();
            _map = mapArg;
        }
        public void Add(IReadOnlyList<Block> blocksArg)
        {
            foreach (var block in blocksArg)
            {
                int blockX = Mathf.RoundToInt(block.Transf.position.x);
                int blockY = Mathf.RoundToInt(block.Transf.position.y);
                Vector2Int pos = new Vector2Int(blockX, blockY);

                if (blockX > _map.MaxCell.x || blockX < _map.MinCell.x)
                {
                    _poolBlocks.Despawn(block);
                    continue;
                }

                if (blockY > _lastRange)
                    _lastRange = blockY;

                _heap[pos] = block;
                block.Transf.SetParent(_transf);
            }
        }
        public void EndGame()
        {
            _lastRange = -1;
            foreach (var pair in _heap)
                _poolBlocks.Despawn(pair.Value);
            _heap.Clear();
        }
        public void Delete(IReadOnlyList<int> rangesArg)
        {
            // TODO: slow fall figures and remove duplicate code
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
        
    }
}

