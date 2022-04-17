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
                    [Inject(Id = "menu")] Canvas menuArg,
                    [Inject(Id = "border")] MeshRenderer borderArg,
                    GameUi gameUiArg,
                    Map mapArg,
                    Camera cameraArg)
        {
            _lblScores = lblScoresArg;
            _gameUi = gameUiArg;
            _menu = menuArg;
            _map = mapArg;
            _borderSample = borderArg;
            _camera = cameraArg;
        }


        protected TextMeshProUGUI _lblScores;
        protected GameUi _gameUi;
        protected Canvas _menu;
        protected Map _map;
        protected Camera _camera;
        protected MeshRenderer _borderSample;
        protected MeshRenderer _leftBorder;
        protected MeshRenderer _rightBorder;
        protected MeshRenderer _topBorder;
        protected MeshRenderer _bottomBorder;


        public void SetCameraAndBorders()
        {
            Vector3 sizeBorder = _borderSample.transform.localScale;
            Vector3 marginBorderH = new Vector3(sizeBorder.x / 2, 0, 0);
            Vector3 marginBorderV = new Vector3(0, sizeBorder.x / 2, 0);
            float widthWith2Border = _map.SizeX + sizeBorder.x * 2;
            float heightWithBorder = _map.SizeY + sizeBorder.x * 2;

            _leftBorder = GameObject.Instantiate(_borderSample, _map.CenterLeft - marginBorderH, Quaternion.identity);
            _rightBorder = GameObject.Instantiate(_borderSample, _map.CenterRight + marginBorderH, Quaternion.identity);
            _topBorder = GameObject.Instantiate(_borderSample, _map.CenterTop + marginBorderV, Quaternion.AngleAxis(90, Vector3.forward));
            _bottomBorder = GameObject.Instantiate(_borderSample, _map.CenterBottom - marginBorderV, Quaternion.AngleAxis(90, Vector3.forward));

            _leftBorder.transform.localScale = sizeBorder.WithY(_map.SizeY);
            _rightBorder.transform.localScale = sizeBorder.WithY(_map.SizeY);
            _topBorder.transform.localScale = sizeBorder.WithY(widthWith2Border);
            _bottomBorder.transform.localScale = sizeBorder.WithY(widthWith2Border);

            _camera.transform.position = GetCameraPos(widthWith2Border, heightWithBorder);
        }
        public void StartGame()
        {
            _menu.enabled = false;
            _gameUi.SetEnable(true);
        }
        public void EndGame()
        {
            _menu.enabled = true;
            _gameUi.SetEnable(false);
        }
        public void SetScores(int scoresArg)
        {
            _lblScores.text = scoresArg.ToString();
        }


        protected Vector3 GetCameraPos(float widthWithBorderArg, float heightWithBorderArg)
        {
            float scalerFromWidth = widthWithBorderArg / heightWithBorderArg / _camera.aspect;
            float frustumHeight = heightWithBorderArg;

            if (scalerFromWidth > 1f)
                frustumHeight *= scalerFromWidth;

            float distance = frustumHeight * 0.5f / Mathf.Tan(_camera.fieldOfView * 0.5f * Mathf.Deg2Rad);

            return new Vector3(_map.MaxCell.x / 2f, _map.MaxCell.y / 2f, -distance);
        }
    }
}

