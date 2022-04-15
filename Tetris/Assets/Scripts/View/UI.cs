using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Zenject;

namespace Tetris
{
    public class UI
    {
        public UI ( [Inject(Id = "lblScores")] TextMeshProUGUI lblScoresArg,
                    [Inject(Id = "gameUI")] Canvas gameUiArg,
                    [Inject(Id = "menu")] Canvas menuArg)
        {
            _lblScores = lblScoresArg;
            _gameUi = gameUiArg;
            _menu = menuArg;
        }


        protected TextMeshProUGUI _lblScores;
        protected Canvas _gameUi;
        protected Canvas _menu;


        public void StartGame()
        {
            _menu.enabled = false;
            _gameUi.enabled = true;
        }
        public void EndGame()
        {
            _menu.enabled = true;
            _gameUi.enabled = false;
        }
        public void SetScores(int scoresArg)
        {
            _lblScores.text = scoresArg.ToString();
        }
    }
}

