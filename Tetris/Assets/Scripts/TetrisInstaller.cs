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
        [SerializeField] protected CalculateParams _calculateParams;
        [SerializeField] protected Difficulty _difficulty;

        [Header("VIEW")]
        [SerializeField] protected View _view;
        [SerializeField] protected FigureView _figureView;
        [SerializeField] protected Block _block;
        [SerializeField] protected Camera _camera;
        [SerializeField] protected Vector3 _sizeBlock = new Vector3(1f, 1f, 1f);

        [Header("UI")]
        [SerializeField] protected TextMeshProUGUI _lblScores;
        [SerializeField] protected GameUi _gameUi;
        [SerializeField] protected Canvas _menu;
        [SerializeField] protected MeshRenderer _border;
        


        public override void InstallBindings()
        {
            if (!IsValidInputData())
                return;

            Container.Bind<IReadOnlyList<FigureTemplate>>().FromInstance(_templatesFigures).AsSingle();
            Container.Bind<Difficulty>().FromInstance(_difficulty).AsSingle();
            Container.Bind<CalculateParams>().FromInstance(_calculateParams).AsSingle();
            Container.Bind<MapData>().FromNew().AsSingle();

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
            int countBlocks = Mathf.RoundToInt(_difficulty.SizeMap.x * _difficulty.SizeMap.y * 0.5f);

            Container.Bind<View>().FromInstance(_view).AsSingle();
            Container.Bind<Camera>().FromInstance(_camera).AsSingle();
            Container.Bind<FigureView>().FromNewComponentOnNewPrefab(_figureView).AsSingle();
            Container.BindMemoryPool<Block, Block.Pool>().WithInitialSize(countBlocks).FromComponentInNewPrefab(_block);

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

            if (_difficulty.SizeMap.x < 5 || _difficulty.SizeMap.y < 10)
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

            if (_calculateParams == null || _calculateParams.AllowedError < float.Epsilon)
            {
                Debug.LogError("Not setted AllowedError in Calculated params", this);
                valid = false;
            }

            if (_sizeBlock.x < _calculateParams.AllowedError || 
                _sizeBlock.y < _calculateParams.AllowedError || 
                _sizeBlock.z < _calculateParams.AllowedError)
            {
                Debug.LogError($"Each of the block size axes must be >  {_calculateParams.AllowedError}", this);
                valid = false;
            }

            if (_calculateParams.SizeBoundsBlock.x < _calculateParams.AllowedError || 
                _calculateParams.SizeBoundsBlock.y < _calculateParams.AllowedError || 
                _calculateParams.SizeBoundsBlock.z < _calculateParams.AllowedError)
            {
                Debug.LogError($"Each of the block bounds size axes must be > {_calculateParams.AllowedError}", this);
                valid = false;
            }

            if (_difficulty.SpeedFalling < _calculateParams.AllowedError)
            {
                Debug.LogError($"Speed falling must be >{_calculateParams.AllowedError}", this);
                valid = false;
            }

            if (_difficulty.SpeedFallingBoosted < _calculateParams.AllowedError)
            {
                Debug.LogError($"Speed falling boost must be > {_calculateParams.AllowedError}", this);
                valid = false;
            }

            if (_difficulty.TimeMoveToSide < _calculateParams.AllowedError)
            {
                Debug.LogError($"Speed move figure to side must be > {_calculateParams.AllowedError}", this);
                valid = false;
            }

            return valid;
        }
    }
}
