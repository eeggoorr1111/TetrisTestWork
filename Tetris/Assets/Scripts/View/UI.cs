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
                    [Inject(Id = "menu")] Canvas menuArg,
                    [Inject(Id = "border")] MeshRenderer borderArg,
                    [Inject(Id = "sizeBlock")] Vector3 sizeBlockArg,
                    Map mapArg)
        {
            _lblScores = lblScoresArg;
            _gameUi = gameUiArg;
            _menu = menuArg;
            _map = mapArg;

            Vector3 sizeBorder = borderArg.transform.localScale;
            Vector3 marginBorderH = new Vector3(sizeBorder.x / 2, 0, 0);
            Vector3 marginBorderV = new Vector3(0, sizeBorder.x / 2, 0);

            _leftBorder = GameObject.Instantiate(borderArg, _map.CenterLeft - marginBorderH, Quaternion.identity);
            _rightBorder = GameObject.Instantiate(borderArg, _map.CenterRight + marginBorderH, Quaternion.identity);
            _topBorder = GameObject.Instantiate(borderArg, _map.CenterTop + marginBorderV, Quaternion.AngleAxis(90, Vector3.forward));
            _bottomBorder = GameObject.Instantiate(borderArg, _map.CenterBottom - marginBorderV, Quaternion.AngleAxis(90, Vector3.forward));

            _leftBorder.transform.localScale = sizeBorder.WithY(_map.SizeY + sizeBorder.x * 2 + sizeBlockArg.y);
            _rightBorder.transform.localScale = sizeBorder.WithY(_map.SizeY + sizeBorder.x * 2 + sizeBlockArg.y);
            _topBorder.transform.localScale = sizeBorder.WithY(_map.SizeX + sizeBorder.x * 2 + sizeBlockArg.x);
            _bottomBorder.transform.localScale = sizeBorder.WithY(_map.SizeX + sizeBorder.x * 2 + sizeBlockArg.x);
        }


        protected TextMeshProUGUI _lblScores;
        protected Canvas _gameUi;
        protected Canvas _menu;
        protected Map _map;
        protected MeshRenderer _leftBorder;
        protected MeshRenderer _rightBorder;
        protected MeshRenderer _topBorder;
        protected MeshRenderer _bottomBorder;


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

