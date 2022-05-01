using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Tetris.View
{
    public class FigureThroughtWall : IFigure
    {
        public FigureThroughtWall(  [Inject(Id = "BlocksFigure1")] BlocksOfFigure blocks1Arg,
                                    [Inject(Id = "BlocksFigure2")] BlocksOfFigure blocks2Arg,
                                    MapData mapArg)
        {
            _blocks1 = blocks1Arg;
            _blocks2 = blocks2Arg;
            _map = mapArg;
            _blocksList = new List<Block>();
        }


        public IReadOnlyList<Block> Blocks => _blocksList;


        private BlocksOfFigure _blocks1;
        private BlocksOfFigure _blocks2;
        private List<Block> _blocksList;
        private MapData _map;


        public void StartCustom()
        {
            _blocks1.StartCustom();
            _blocks2.StartCustom();
        }
        public void Transform(Vector3 posArg, Quaternion rotateArg)
        {
            Vector3 pos1, pos2;
            GetPosBlocks(posArg, out pos1, out pos2);

            _blocks1.Transf.position = pos1;
            _blocks2.Transf.position = pos2;

            _blocks1.Transf.rotation = rotateArg;
            _blocks2.Transf.rotation = rotateArg;
        }
        public void NewFigrue(Vector3 posArg, FigureTemplate templateArg)
        {
            Vector3 pos1, pos2;
            GetPosBlocks(posArg, out pos1, out pos2);

            _blocks1.NewFigrue(pos1, templateArg);
            _blocks2.NewFigrue(pos2, templateArg);

            _blocksList.Clear();
            _blocksList.AddRange(_blocks1.Blocks);
            _blocksList.AddRange(_blocks2.Blocks);
        }
        public void EndGame()
        {
            _blocks1.ClearBlocks();
            _blocks2.ClearBlocks();
        }


        protected void GetPosBlocks(Vector3 posFigureArg, out Vector3 pos1, out Vector3 pos2)
        {
            Vector3 offset = new Vector3(_map.SizeX, 0, 0);

            pos1 = posFigureArg;
            pos2 = posFigureArg.x > _map.Center.x ? pos1 - offset : pos1 + offset;
        }
    }
}

