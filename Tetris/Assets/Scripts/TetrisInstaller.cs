using UnityEngine;
using Zenject;
using System.Collections.Generic;
using TMPro;
using Tetris.View;
using Tetris.Model;


namespace Tetris
{
    public class TetrisInstaller : MonoInstaller
    {
        private static readonly Vector2Int MinSizeMap;


        [Header("COMMON")]
        [SerializeField] protected CalculateParams _calculateParams;
        [SerializeField] protected LevelParams _level1;
        [SerializeField] protected LevelParams _level2;

        [Header("VIEW")]
        [SerializeField] protected TetrisView _view;
        [SerializeField] protected BlocksOfFigure _blockFigure;
        [SerializeField] protected Block _block;
        [SerializeField] protected Camera _camera;
        [SerializeField] protected BlockParams _blockParams;

        [Header("UI")]
        [SerializeField] protected TextMeshProUGUI _lblScores;
        [SerializeField] protected GameUi _gameUi;
        [SerializeField] protected Canvas _menu;
        [SerializeField] protected MeshRenderer _border;
        


        public override void InstallBindings()
        {
            if (!IsValidInputData())
                return;

            LevelsParams lvlsParams = CreateLevelsParams();

            Container.Bind<LevelsParams>().FromInstance(lvlsParams).AsSingle();
            Container.Bind<ILevelsParams>().FromInstance(lvlsParams).AsSingle();

            Container.Bind<CalculateParams>().FromInstance(_calculateParams).AsSingle();
            Container.Bind<MapData>().FromNew().AsSingle();

            InstallModel();
            InstallView();
        }


        protected void InstallModel()
        {
            Container.Bind<TetrisModel>().AsSingle();
            Container.Bind<HeapFigures>().AsSingle();
            Container.Bind<FigureGenerator>().AsSingle();
            Container.Bind<CheckCollisionHeap>().AsSingle();

            Container.Bind<TransfBlockedWall>().AsSingle();
            Container.Bind<TransformatorThroughtWall>().AsSingle();
        }
        protected void InstallView()
        {
            int countBlocksMode1 = Mathf.RoundToInt(_level1.CountCellsAll * 0.5f);
            int countBlocksMode2 = Mathf.RoundToInt(_level2.CountCellsAll * 0.5f);

            Container.Bind<TetrisView>().FromInstance(_view).AsSingle();
            Container.Bind<Camera>().FromInstance(_camera).AsSingle();
            Container.Bind<BlocksOfFigure>().WithId("BlocksFigure1").FromComponentInNewPrefab(_blockFigure).AsCached();
            Container.Bind<BlocksOfFigure>().WithId("BlocksFigure2").FromComponentInNewPrefab(_blockFigure).AsCached();
            Container.BindMemoryPool<Block, Block.Pool>().WithInitialSize(Mathf.Max(countBlocksMode1, countBlocksMode2)).FromComponentInNewPrefab(_block);
            Container.Bind<BlockParams>().FromInstance(_blockParams).AsTransient();

            Container.Bind<FigureBlockedWall>().AsSingle();
            Container.Bind<FigureThroughtWall>().AsSingle();

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
        private LevelsParams CreateLevelsParams()
        {
            LevelParams[] lvlsParams = new LevelParams[]
            {
                _level1,
                _level2
            };

            return new LevelsParams(lvlsParams, 0);
        }
        protected bool IsValidInputData()
        {
            bool valid = true;

            if (!CheckFigureTemplates(_level1))
                valid = false;

            if (!CheckFigureTemplates(_level2))
                valid = false;

            if (_view == null)
            {
                Debug.LogError("view is null", this);
                valid = false;
            }

            if (_blockFigure == null)
            {
                Debug.LogError("figure view is null", this);
                valid = false;
            }

            if (_block == null)
            {
                Debug.LogError("block is null", this);
                valid = false;
            }

            if (_level1.CountCells.x < MinSizeMap.x || _level1.CountCells.y < MinSizeMap.y ||
                _level2.CountCells.x < MinSizeMap.x || _level2.CountCells.y < MinSizeMap.y)
            {
                Debug.LogError($"Map size must be >= {MinSizeMap.x} in x and >= {MinSizeMap.y} in y", this);
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

            if (_calculateParams.SizeBoundsBlock.x < _calculateParams.AllowedError || 
                _calculateParams.SizeBoundsBlock.y < _calculateParams.AllowedError || 
                _calculateParams.SizeBoundsBlock.z < _calculateParams.AllowedError)
            {
                Debug.LogError($"Each of the block bounds size axes must be > {_calculateParams.AllowedError}", this);
                valid = false;
            }

            if (_level1.SpeedFalling < _calculateParams.AllowedError ||
                _level2.SpeedFalling < _calculateParams.AllowedError)
            {
                Debug.LogError($"Speed falling must be >{_calculateParams.AllowedError}", this);
                valid = false;
            }

            if (_level1.SpeedFallingBoosted < _calculateParams.AllowedError ||
                _level2.SpeedFallingBoosted < _calculateParams.AllowedError)
            {
                Debug.LogError($"Speed falling boost must be > {_calculateParams.AllowedError}", this);
                valid = false;
            }

            if (_level1.TimeMoveToSide < _calculateParams.AllowedError ||
                _level2.TimeMoveToSide < _calculateParams.AllowedError)
            {
                Debug.LogError($"Speed move figure to side must be > {_calculateParams.AllowedError}", this);
                valid = false;
            }

            return valid;
        }

        private bool CheckFigureTemplates(LevelParams paramsArg)
        {
            bool valid = true;
            IReadOnlyList<FigureTemplate> templates = paramsArg.FiguresTemplates;

            if (templates.IsEmpty())
            {
                Debug.LogError("Count templates figures is 0", this);
                return false;
            }
            else
            {
                for (int i = 0; i < templates.Count; i++)
                    if (templates[i].WeightGenerate < float.Epsilon)
                    {
                        Debug.LogError($"Weight figure with index {i} < 0", this);
                        valid = false;
                    }


                if (templates.GetSumWeights() < float.Epsilon)
                {
                    Debug.LogError("Sum weights of templates figures must be > 0", this);
                    valid = false;
                }
            }

            return valid;
        }
    }
}
