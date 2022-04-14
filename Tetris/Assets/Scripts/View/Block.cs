using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Tetris
{
    public class Block : MonoBehaviour
    {
        public Transform Transf => _transf;
        public Vector2Int PosInFigure => _posInFigure;


        protected Transform _transf;
        protected Vector2Int _posInFigure;


        protected void StartCustom()
        {
            _transf = GetComponent<Transform>();
        }
        protected void SetToFigure(Vector2Int posInFigureArg)
        {
            _posInFigure = posInFigureArg;
        }


        public class Pool : MonoMemoryPool<Vector2Int, Block>
        {
            protected override void OnCreated(Block itemArg)
            {
                itemArg.StartCustom();
                base.OnCreated(itemArg);
            }
            protected override void Reinitialize(Vector2Int posInFigureArg, Block itemArg)
            {
                itemArg.SetToFigure(posInFigureArg);
                base.Reinitialize(posInFigureArg, itemArg);
            }
        }
    }
}

