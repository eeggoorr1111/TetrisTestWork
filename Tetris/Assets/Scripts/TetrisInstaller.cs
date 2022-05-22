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

        [Header("MODEL")]
        [SerializeField] protected CalculateParams _calculateParams;

        [Header("VIEW")]
        [SerializeField] protected TetrisView _view;
        [SerializeField] protected BlocksOfFigure _blockFigure;
        [SerializeField] protected Block _block;

        [Header("LEVELS")]
        [SerializeField] protected LevelParams _level1;
        [SerializeField] protected LevelParams _level2;


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
            Container.Bind<Model.HeapBlocks>().AsSingle();
            Container.Bind<FigureGenerator>().AsSingle();
            Container.Bind<CheckerOnCollisionWithHeap>().AsSingle();

            Container.Bind<TransfBlockedWall>().AsSingle();
            Container.Bind<TransfThroughtWall>().AsSingle();
        }
        private void InstallView()
        {
            int countBlocksMode1 = Mathf.RoundToInt(_level1.CountCellsAll * 0.5f);
            int countBlocksMode2 = Mathf.RoundToInt(_level2.CountCellsAll * 0.5f);
            int initCountBlocks = Mathf.Min(countBlocksMode1, countBlocksMode2);
            Transform transfFigures = new GameObject("Figures").GetComponent<Transform>();
            Transform transfBlocks = new GameObject("Blocks").GetComponent<Transform>();
            Transform transfView = _view.transform;

            transfFigures.SetParent(transfView);
            transfBlocks.SetParent(transfView);

            Container.Bind<TetrisView>().FromInstance(_view).AsSingle();
            Container.Bind<BlocksOfFigure>().WithId("BlocksFigure1").FromComponentInNewPrefab(_blockFigure).UnderTransform(transfFigures).AsCached();
            Container.Bind<BlocksOfFigure>().WithId("BlocksFigure2").FromComponentInNewPrefab(_blockFigure).UnderTransform(transfFigures).AsCached();
            Container.BindMemoryPool<Block, Block.Pool>().WithInitialSize(initCountBlocks).FromComponentInNewPrefab(_block).UnderTransform(transfBlocks); 

            Container.Bind<FigureBlockedWall>().AsSingle();
            Container.Bind<FigureThroughtWall>().AsSingle();
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
        private bool IsValidInputData()
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
