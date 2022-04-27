﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Tetris.View
{
    public sealed class Figure : IFigure
    {
        public Figure([Inject(Id = "BlocksFigure1")] BlocksOfFigure blocksArg)
        {
            _blocks = blocksArg;
        }


        public IReadOnlyList<Block> Blocks => _blocks.Blocks;


        private BlocksOfFigure _blocks;


        public void StartCustom()
        {
            _blocks.StartCustom();
        }
        public void Transform(Vector3 posArg, Quaternion rotateArg)
        {
            _blocks.Transf.position = posArg;
            _blocks.Transf.rotation = rotateArg;
        }
        public void NewFigrue(Vector3 posArg, FigureTemplate templateArg)
        {
            _blocks.NewFigrue(posArg, templateArg);
        }
    }
}

