using UnityEngine;
using Zenject;
using System.Collections.Generic;
using TMPro;

namespace Tetris
{
    public class TetrisInstaller : MonoInstaller
    {
        [Header("COMMON")]
        [SerializeField] protected List<FigureTemplate> _templatesFigures;
        [SerializeField] protected Vector2Int _minMap;
        [SerializeField] protected Vector2Int _maxMap;

        [Header("VIEW")]
        [SerializeField] protected View _view;
        [SerializeField] protected FigureView _figureView;
        [SerializeField] protected Block _block;
        [SerializeField] protected Camera _camera;

        [Header("UI")]
        [SerializeField] protected TextMeshProUGUI _lblScores;
        [SerializeField] protected Canvas _gameUi;
        [SerializeField] protected Canvas _menu;


        [Header("DIFFICULTY")]
        [SerializeField] protected Difficulty _difficulty;


        public override void InstallBindings()
        {
            if (!IsValidInputData())
                return;

            Container.Bind<IReadOnlyList<FigureTemplate>>().FromInstance(_templatesFigures).AsSingle();
            Container.Bind<Difficulty>().FromInstance(_difficulty).AsSingle();
            Container.Bind<Map>().FromInstance(GetMap()).AsSingle();

            InstallModel();
            InstallView();
        }


        protected void InstallModel()
        {
            Container.Bind<Model>().FromNew().AsSingle();
            Container.Bind<Rotator>().FromNew().AsSingle();
            Container.Bind<HeapFigures>().FromNew().AsSingle();
            Container.Bind<FigureGenerator>().FromNew().AsSingle();

            IReadOnlyList<Mover> movers = CreateMovers();
            Container.Bind<IReadOnlyList<Mover>>().FromInstance(movers).AsSingle();
            Container.Bind<int>().WithId("maxIdxGameMod").FromInstance(movers.Count - 1).AsSingle();
        }
        protected void InstallView()
        {
            Container.Bind<View>().FromInstance(_view).AsSingle();
            Container.Bind<Camera>().FromInstance(_camera).AsSingle();
            Container.BindMemoryPool<FigureView, FigureView.Pool>().WithInitialSize(10).FromComponentInNewPrefab(_figureView);
            Container.BindMemoryPool<Block, Block.Pool>().WithInitialSize(40).FromComponentInNewPrefab(_block);

            InstallUI();
        }
        protected void InstallUI()
        {
            Container.Bind<TextMeshProUGUI>().WithId("lblScores").FromInstance(_lblScores).AsSingle();
            Container.Bind<Canvas>().WithId("gameUI").FromInstance(_gameUi).AsTransient();
            Container.Bind<Canvas>().WithId("menu").FromInstance(_menu).AsTransient();
            
            Container.Bind<UI>().FromNew().AsSingle();
        }
        protected IReadOnlyList<Mover> CreateMovers()
        {
            List<Mover> movers = new List<Mover>();

            movers.Add(Container.Instantiate<MoverBlockedWall>());
            movers.Add(Container.Instantiate<MoverThroughtWall>());

            return movers;
        }
        protected Map GetMap()
        {
            BoundsInt bounds = new BoundsInt();
            bounds.SetMinMax(new Vector3Int(_minMap.x, _minMap.y, 0), 
                             new Vector3Int(_maxMap.x, _maxMap.y, 0));

            return new Map(bounds);
        }
        protected bool IsValidInputData()
        {
            bool valid = true;

            if (_templatesFigures.IsEmpty())
            {
                Debug.LogError("Count templates figures is 0", this);
                valid = false;
            }
            else
            {
                float sumWeights = 0f;
                for (int i = 0; i < _templatesFigures.Count; i++)
                {
                    sumWeights += _templatesFigures[i].WeightGenerate;
                    if (_templatesFigures[i].WeightGenerate < 0)
                    {
                        Debug.LogError($"Weight figure with index {i} < 0", this);
                        valid = false;
                    }
                }

                if (sumWeights < float.Epsilon)
                {
                    Debug.LogError("Sum weights of templates figures must be > 0", this);
                    valid = false;
                }
            }

            if (_view == null)
            {
                Debug.LogError("view is null", this);
                valid = false;
            }

            if (_figureView == null)
            {
                Debug.LogError("figure view is null", this);
                valid = false;
            }

            if (_block == null)
            {
                Debug.LogError("block is null", this);
                valid = false;
            }

            if (_difficulty.SpeedFalling < float.Epsilon)
            {
                Debug.LogError("Speed falling <= 0", this);
                valid = false;
            }

            if (_difficulty.SpeedFallingBoosted < float.Epsilon)
            {
                Debug.LogError("Speed falling boost <= 0", this);
                valid = false;
            }

            if (_difficulty.SpeedMoveToSide < float.Epsilon)
            {
                Debug.LogError("Speed move figure to side <= 0", this);
                valid = false;
            }

            Vector2Int sizeMap = _maxMap - _minMap;
            if (sizeMap.x < 4 || sizeMap.y < 10)
            {
                Debug.LogError("Map size must be >= 4 in x and >= 10 in y", this);
                valid = false;
            }

            if (_lblScores == null)
            {
                Debug.LogError("Label scores is null", this);
                valid = false;
            }

            if (_camera == null)
            {
                Debug.LogError("Camera is null", this);
                valid = false;
            }
                
            if (_gameUi == null)
            {
                Debug.LogError("Game UI is null", this);
                valid = false;
            }

            if (_menu == null)
            {
                Debug.LogError("Menu is null", this);
                valid = false;
            }
                

            return valid;
        }
    }
}
