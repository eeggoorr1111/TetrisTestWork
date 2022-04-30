using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Tetris.View
{

    public class BlocksOfFigure : MonoBehaviour
    {
        [Inject]
        protected void Constructor(Block.Pool poolBlocksArg)
        {
            _poolBlocks = poolBlocksArg;
        }


        public Transform Transf => _transf;
        public IReadOnlyList<Block> Blocks => _blocks;


        protected Transform _transf;
        protected Block.Pool _poolBlocks;
        protected List<Block> _blocks;


        public void StartCustom()
        {
            _transf = GetComponent<Transform>();
            _blocks = new List<Block>();
        }
        public void NewFigrue(Vector3 posArg, FigureTemplate templateArg)
        {
            _transf.position = posArg;
            _blocks.Clear();

            foreach (Vector2Int blockPos in templateArg.Blocks)
            {
                Block block = _poolBlocks.Spawn(blockPos);

                block.Transf.SetParent(_transf);
                block.Transf.localPosition = new Vector3Int(blockPos.x, blockPos.y, 0);
                block.Transf.rotation = Quaternion.Euler(0f, 0f, 0f);

                _blocks.Add(block);
            }
        }
    }
}
