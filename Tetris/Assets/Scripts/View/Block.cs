using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Tetris
{
    public class Block : MonoBehaviour
    {
        public Transform Transf => _transf;
        public Vector3 PosInFigure => new Vector3(_posInFigure.x, _posInFigure.y, 0);
        public Vector2Int PosInFigureInt
        {
            get { return _posInFigure; }
            set { _posInFigure = value; }
        }


        protected Transform _transf;
        [SerializeField] protected Vector2Int _posInFigure;


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

