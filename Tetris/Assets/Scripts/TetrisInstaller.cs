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
        [SerializeField] protected Vector2Int _sizeMap;
        [SerializeField] protected CalculateParams _calculateParams;

        [Header("VIEW")]
        [SerializeField] protected View _view;
        [SerializeField] protected FigureView _figureView;
        [SerializeField] protected Block _block;
        [SerializeField] protected Camera _camera;
        [SerializeField] protected Vector3 _sizeBlock = new Vector3(1f, 1f, 1f);

        [Header("MODEL")]
        [SerializeField] protected Vector3 _sizeBoundsBlock = new Vector3(1f, 1f, 1f);

        [Header("UI")]
        [SerializeField] protected TextMeshProUGUI _lblScores;
        [SerializeField] protected GameUi _gameUi;
        [SerializeField] protected Canvas _menu;
        [SerializeField] protected MeshRenderer _border;


        [Header("DIFFICULTY")]
        [SerializeField] protected Difficulty _difficulty;


        public override void InstallBindings()
        {
            if (!IsValidInputData())
                return;

            Container.Bind<IReadOnlyList<FigureTemplate>>().FromInstance(_templatesFigures).AsSingle();
            Container.Bind<Difficulty>().FromInstance(_difficulty).AsSingle();
            Container.Bind<CalculateParams>().FromInstance(_calculateParams).AsSingle();
            Container.Bind<Map>().FromInstance(GetMap()).AsSingle();

            InstallModel();
            InstallView();
        }


        protected void InstallModel()
        {
            Container.Bind<Vector3>().WithId("sizeBoundsBlock").FromInstance(_sizeBoundsBlock).AsTransient();

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

            Container.Bind<Vector3>().WithId("sizeBlock").FromInstance(_sizeBlock).AsTransient();

            InstallUI();
        }
        protected void InstallUI()
        {
            Container.Bind<TextMeshProUGUI>().WithId("lblScores").FromInstance(_lblScores).AsSingle();
            Container.Bind<GameUi>().FromInstance(_gameUi).AsTransient();
            Container.Bind<Canvas>().WithId("menu").FromInstance(_menu).AsTransient();
            Container.Bind<MeshRenderer>().WithId("border").FromInstance(_border).AsTransient();
            
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
            bounds.SetMinMax(new Vector3Int(0, 0, 0), 
                             new Vector3Int(_sizeMap.x, _sizeMap.y, 0));

            return new Map(bounds, _sizeBoundsBlock);
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

            if (_difficulty.TimeMoveToSide < float.Epsilon)
            {
                Debug.LogError("Speed move figure to side <= 0", this);
                valid = false;
            }

            if (_sizeMap.x < 4 || _sizeMap.y < 10)
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

            if (_border == null)
            {
                Debug.LogError("Border is null", this);
                valid = false;
            }

            if (_sizeBlock.x < float.Epsilon || _sizeBlock.y < float.Epsilon || _sizeBlock.z < float.Epsilon)
            {
                Debug.LogError("Each of the block size axes must be > 0", this);
                valid = false;
            }

            if (_sizeBoundsBlock.x < float.Epsilon || _sizeBoundsBlock.y < float.Epsilon || _sizeBoundsBlock.z < float.Epsilon)
            {
                Debug.LogError("Each of the block bounds size axes must be > 0", this);
                valid = false;
            }

            if (_calculateParams == null || _calculateParams.AllowedError < float.Epsilon)
            {
                Debug.LogError("Not setted AllowedError in Calculated params", this);
                valid = false;
            }

            return valid;
        }
    }
}
