using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Tetris.View
{

    public sealed class BlocksOfFigure : MonoBehaviour
    {
        [Inject]
        private void Constructor(Block.Pool poolBlocksArg)
        {
            _poolBlocks = poolBlocksArg;
        }


        public Transform Transf { get; private set; }
        public IReadOnlyList<Block> Blocks => _blocks;


        private Block.Pool _poolBlocks;
        private List<Block> _blocks;


        public void StartCustom()
        {
            _blocks = new List<Block>();
            Transf = GetComponent<Transform>();
        }
        public void NewFigrue(Vector3 posArg, FigureTemplate templateArg)
        {
            Transf.position = posArg;

            _blocks.Clear();
            foreach (Vector2Int blockPos in templateArg.Blocks)
            {
                Block block = _poolBlocks.Spawn(blockPos);

                block.Transf.SetParent(Transf);
                block.Transf.localPosition = new Vector3Int(blockPos.x, blockPos.y, 0);
                block.Transf.rotation = Quaternion.Euler(0f, 0f, 0f);

                _blocks.Add(block);
            }
        }
    }
}
