using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tetris
{
    public sealed class LevelsParams : ILevelsParams
    {
        public LevelsParams(LevelParams[] paramsArg, int levelArg)
        {
            _lvlsParams = paramsArg;
            CurrentLevel = levelArg;
        }


        public LevelParams Current => _lvlsParams[CurrentLevel];
        public int CurrentLevel { get; set; }
        public int MaxLevel => _lvlsParams.Length - 1;


        private readonly LevelParams[] _lvlsParams;
    }
}

