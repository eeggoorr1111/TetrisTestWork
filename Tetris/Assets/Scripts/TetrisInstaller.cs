using UnityEngine;
using Zenject;
using System.Collections.Generic;

namespace Tetris
{
    public class TetrisInstaller : MonoInstaller
    {
        [Header("COMMON")]
        [SerializeField] protected List<FigureTemplate> _templatesFigures;

        [Header("VIEW")]
        [SerializeField] protected View _view;
        [SerializeField] protected FigureView _figureView;
        [SerializeField] protected Block _block;


        public override void InstallBindings()
        {
            Container.Bind<IReadOnlyList<FigureTemplate>>().FromInstance(_templatesFigures).AsSingle();

            InstallModel();
            InstallView();
        }


        protected void InstallModel()
        {
            Container.Bind<Model>().FromNew().AsSingle();
            Container.Bind<Rotator>().FromNew().AsSingle();
            Container.Bind<IReadOnlyList<IMover>>().FromMethod(CreateMovers).AsSingle();
        }
        protected void InstallView()
        {
            Container.Bind<View>().FromInstance(_view).AsSingle();
            Container.BindMemoryPool<FigureView, FigureView.Pool>().WithInitialSize(10).FromComponentInNewPrefab(_figureView);
            Container.BindMemoryPool<Block, Block.Pool>().WithInitialSize(40).FromComponentInNewPrefab(_block);
        }
        protected IReadOnlyList<IMover> CreateMovers()
        {
            List<IMover> movers = new List<IMover>();

            movers.Add(Container.Instantiate<Mover>());
            movers.Add(Container.Instantiate<MoverBehindWall>());

            return movers;
        }
    }
}
