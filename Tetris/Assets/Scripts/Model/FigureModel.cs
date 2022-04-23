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
                            ColliderFigure colliderArg)
        {
            IdxTemplate = idxTemplateArg;

            _rotator = rotatorArg;
            _collider = colliderArg;
        }

        public int IdxTemplate { get; }
        public Vector3 Pivot => _collider.Pivot;
        public IReadOnlyList<Bounds> Blocks => _collider.Blocks;
        public Bounds Bounds => _collider.Bounds;
        public Quaternion Rotate => _collider.Rotate;


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

