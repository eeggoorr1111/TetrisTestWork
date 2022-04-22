using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Tetris
{
    public sealed class FigureModel
    {
        public FigureModel( Rotator rotatorArg, 
                            int idxTemplateArg, 
                            ColliderFigure colliderArg,
                            Vector3 deltaPivotArg)
        {
            _rotator = rotatorArg;
            _idxTemplate = idxTemplateArg;
            _collider = colliderArg;
            _deltaPivot = deltaPivotArg;
        }


        public Vector3 Pivot => _collider.Bounds.center + _deltaPivot;
        public IReadOnlyList<Bounds> BoundsBlocks => _collider.Blocks;
        public int IdxTemplate => _idxTemplate;
        public Bounds Bounds => _collider.Bounds;
        public IReadOnlyList<Bounds> Blocks => _collider.Blocks;
        public Quaternion Rotate => _collider.Rotate;


        
        private readonly int _idxTemplate;
        private readonly Vector3 _deltaPivot;
        private readonly Rotator _rotator;
        private readonly ColliderFigure _collider;


        public void ToMoveToSide(Mover moverArg, bool toRightArg)
        {
            moverArg.MoveToSide(toRightArg, _collider);
        }
        public bool ToFall(Mover moverArg, bool boostedFallArg)
        {
            return moverArg.ToFall(boostedFallArg, _collider);
        }
        public void ToRotate()
        {
            _rotator.Rotate(_collider);
        }
    }
}

