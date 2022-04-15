using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Tetris
{
    public class HeapFigures
    {
        public HeapFigures(Map mapArg)
        {
            _map = mapArg;
            _boundsTop = new List<Bounds>();
        }


        public int BottomByY => _map.Bounds.min.y;
        public float TopByY => _topByY;
        public Bounds Bounds => _bounds;
        public IReadOnlyList<Bounds> BoundsOfTop => _boundsTop;
        public int Ranges => 0;


        protected List<Bounds> _boundsTop;
        protected Bounds _bounds;
        protected Map _map;
        protected float _topByY;


        public int Add(FigureModel figureArg)
        {
            return 0;
        }
    }
}

