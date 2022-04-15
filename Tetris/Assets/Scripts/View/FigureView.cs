using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using System;

namespace Tetris
{

    public class FigureView : MonoBehaviour
    {
        [Inject]
        protected void Constructor(Block.Pool poolBlocksArg)
        {
            _poolBlocks = poolBlocksArg;
        }


        public Transform Transf => _transf;
        public bool IsExistsMonoB => this != null;


        protected Transform _transf;
        protected Block.Pool _poolBlocks;
        protected List<Block> _blocks;
        protected event Action<FigureView> _onRemovedAllBlocks;

        
        public void SubscribeOnRemovedAllBlocks(Action<FigureView> callbackArg)
        {
            _onRemovedAllBlocks -= callbackArg;
            _onRemovedAllBlocks += callbackArg;
        }
        public void UnsubscribeOnRemovedAllBlocks(Action<FigureView> callbackArg)
        {
            _onRemovedAllBlocks -= callbackArg;
        }
        public void RemoveBlock(Vector2Int positionArg)
        {
            for (int i = _blocks.Count - 1; i >= 0; i--)
                if (_blocks[i].PosInFigure == positionArg)
                {
                    _poolBlocks.Despawn(_blocks[i]);
                    _blocks.RemoveAt(i);

                    break;
                }

            if (_blocks.Count == 0 && _onRemovedAllBlocks != null)
                _onRemovedAllBlocks.Invoke(this);
        }


        
        protected void StartCustom()
        {
            _transf = GetComponent<Transform>();
            _blocks = new List<Block>();
        }
        protected void NewFigrue(Vector3 posArg, FigureTemplate templateArg)
        {
            _transf.position = posArg;
            ClearBlocks();

            foreach (Vector2Int blockPos in templateArg.Blocks)
            {
                Block block = _poolBlocks.Spawn(blockPos);

                block.Transf.SetParent(_transf);
                block.Transf.localPosition = new Vector3Int(blockPos.x, blockPos.y, 0);
            }
        }
        protected void ClearBlocks()
        {
            if (_blocks.Count > 0)
            {
                foreach (var block in _blocks)
                    _poolBlocks.Despawn(block);
                _blocks.Clear();
            }
        }
        protected void OnDestroy()
        {
            _onRemovedAllBlocks = null;
        }


        public class Pool : MonoMemoryPool<Vector3, FigureTemplate, FigureView>
        {
            protected override void OnCreated(FigureView item)
            {
                base.OnCreated(item);
                item.StartCustom();
            }
            protected override void Reinitialize(Vector3 posArg, FigureTemplate templateArg, FigureView figureArg)
            {
                figureArg.NewFigrue(posArg, templateArg);
            }
            protected override void OnDespawned(FigureView itemArg)
            {
                itemArg.ClearBlocks();
                base.OnDespawned(itemArg);
            }
        }
    }

}
