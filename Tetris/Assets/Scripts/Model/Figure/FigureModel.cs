using System.Collections.Generic;
using UnityEngine;

namespace Tetris.Model
{
    public sealed class FigureModel
    {
        public FigureModel( int idxTemplateArg, 
                            ColliderFigure colliderArg)
        {
            IdxTemplate = idxTemplateArg;

            _collider = colliderArg;
        }

        public int IdxTemplate { get; }
        public Vector3 Pivot => _collider.Pivot;
        public IReadOnlyList<Bounds> Blocks => _collider.Blocks;
        public Bounds Bounds => _collider.Bounds;
        public Quaternion Rotate => _collider.Rotate;


        private readonly ColliderFigure _collider;


        public void ToMoveToSide(Transformator transfArg, bool toRightArg)
        {
            transfArg.MoveToSide(toRightArg, _collider);
        }
        public bool ToFall(Transformator transfArg, bool boostedFallArg)
        {
            return transfArg.ToFall(boostedFallArg, _collider);
        }
        public void ToRotate(Transformator transfArg)
        {
            transfArg.ToRotate(_collider);
        }
    }
}

