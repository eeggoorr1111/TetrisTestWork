using System.Collections;
using System.Collections.Generic;
using Tetris.Interfaces;
using Tetris.GameObjects;
using Tetris.Generator;
using Tetris.Transform;
using Tetris.Commands;
using UnityEngine;

namespace Tetris.Global
{
    public class ConfigurationMode1 : AbstractConfigurator
    {
        private GameObject _modelCube;
        private IMap _map;
        private Obstructions _obstructions;

        private IMover _mover;
        private IRotator _rotator;

        private ICommandObstructionIsExists _cmdObstructionIsExists;
        private ICommandObstructionAdd _cmdObstructionAdd;
        private ICommandObstructionReplace _cmdObstructionReplace;
        private ICommandObstructionDelete _cmdObstructionDelete;

        public ConfigurationMode1(string nameArg, Camera cameraArg, GameObject modelCubeArg)
        {
            _name = nameArg;
            _map = new MapGame(new Vector3Int(10, 20, 1));
            _modelCube = modelCubeArg;
            _obstructions = new Obstructions();

            _cmdObstructionIsExists = new CommandObstructionIsExists(_obstructions);
            _cmdObstructionAdd = new CommandObstructionAdd(_obstructions);
            _cmdObstructionReplace = new CommandObstructionReplace(_obstructions);
            _cmdObstructionDelete = new CommandObstructionDelete(_obstructions);

            _mover = new Mover(_map.Size, _cmdObstructionIsExists, _cmdObstructionReplace);
            _rotator = new Rotator(_mover, _map.Size);
        }

        public override IMap CreateMap()
        {
            return _map;
        }

        public override IFactoryFigure CreateFactoryFigure()
        {
            return new CubeFactory(_modelCube);
        }
        public override IFigureGenerator CreateGenerator()
        {
            TemplateFigureManager manager = new TemplateFigureManager();

            TemplateGroupFigures figure1 = new TemplateGroupFigures(1, new Vector3Int(2, 2, 1), 10);
            figure1.AddItem(new Vector3Int(0, 0, 0));
            figure1.AddItem(new Vector3Int(0, 1, 0));
            figure1.AddItem(new Vector3Int(1, 0, 0));
            figure1.AddItem(new Vector3Int(1, 1, 0));

            TemplateGroupFigures figure2 = new TemplateGroupFigures(2, new Vector3Int(3, 2, 1), 15);
            figure2.AddItem(new Vector3Int(1, 0, 0));
            figure2.AddItem(new Vector3Int(2, 0, 0));
            figure2.AddItem(new Vector3Int(0, 1, 0));
            figure2.AddItem(new Vector3Int(1, 1, 0));

            TemplateGroupFigures figure3 = new TemplateGroupFigures(3, new Vector3Int(3, 2, 1), 15);
            figure3.AddItem(new Vector3Int(0, 0, 0));
            figure3.AddItem(new Vector3Int(1, 0, 0));
            figure3.AddItem(new Vector3Int(1, 1, 0));
            figure3.AddItem(new Vector3Int(2, 1, 0));

            TemplateGroupFigures figure4 = new TemplateGroupFigures(4, new Vector3Int(3, 2, 1), 15);
            figure4.AddItem(new Vector3Int(0, 0, 0));
            figure4.AddItem(new Vector3Int(1, 0, 0));
            figure4.AddItem(new Vector3Int(2, 0, 0));
            figure4.AddItem(new Vector3Int(2, 1, 0));

            TemplateGroupFigures figure5 = new TemplateGroupFigures(5, new Vector3Int(3, 2, 1), 15);
            figure5.AddItem(new Vector3Int(0, 0, 0));
            figure5.AddItem(new Vector3Int(1, 0, 0));
            figure5.AddItem(new Vector3Int(2, 0, 0));
            figure5.AddItem(new Vector3Int(0, 1, 0));

            TemplateGroupFigures figure6 = new TemplateGroupFigures(6, new Vector3Int(4, 1, 1), 10);
            figure6.AddItem(new Vector3Int(0, 0, 0));
            figure6.AddItem(new Vector3Int(1, 0, 0));
            figure6.AddItem(new Vector3Int(2, 0, 0));
            figure6.AddItem(new Vector3Int(3, 0, 0));

            TemplateGroupFigures figure7 = new TemplateGroupFigures(7, new Vector3Int(3, 2, 1), 20);
            figure7.AddItem(new Vector3Int(0, 0, 0));
            figure7.AddItem(new Vector3Int(1, 0, 0));
            figure7.AddItem(new Vector3Int(2, 0, 0));
            figure7.AddItem(new Vector3Int(1, 1, 0));

            manager.AddTemplate(figure1).AddTemplate(figure2).AddTemplate(figure3).AddTemplate(figure4)
                .AddTemplate(figure5).AddTemplate(figure6).AddTemplate(figure7);

            return new Tetris.Generator.Generator(_map.Size, manager);
        }

        public override Vector3 CreateVectorDrop()
        {
            return new Vector3(0, -1.0f, 0);
        }
        public override Vector3Int CreateVectorDropInt()
        {
            return Vector3Int.down;
        }
        public override int DelayFrameDrop()
        {
            return 15;
        }
        public override IMover CreateMover()
        {
            return _mover;
        }
        public override IRotator CreateRotator()
        {
            return _rotator;
        }

        public override Obstructions CreateObstructions()
        {
            return _obstructions;
        }
        public override IStackFigures CreateStackFigures()
        {
            return new StackFigures(_map.Size, 1, _mover, _cmdObstructionDelete);
        }

        public override ICommandObstructionIsExists CmdObstructionIsExists()
        {
            return _cmdObstructionIsExists;
        }
        public override ICommandObstructionAdd CmdObstructionAdd()
        {
            return _cmdObstructionAdd;
        }
        public override ICommandObstructionReplace CmdObstructionReplace()
        {
            return _cmdObstructionReplace;
        }
        public override ICommandObstructionDelete CmdObstructionDelete()
        {
            return _cmdObstructionDelete;
        }

    }
}
