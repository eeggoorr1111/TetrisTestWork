using TMPro;
using UnityEngine;
using Zenject;
using UnityEngine.UI;

namespace Tetris.View
{
    public sealed class UI : MonoBehaviour
    {
        public Button BtnLvl1 => _btnLvl1;
        public Button BtnLvl2 => _btnLvl2;
        public Button BtnGoToMenu => _btnToMenu;


        [SerializeField] private TextMeshProUGUI _lblScores;
        [SerializeField] private Canvas _gameUi;
        [SerializeField] private Canvas _menu;
        [SerializeField] private Camera _camera;
        [SerializeField] private MeshRenderer _borderSample;
        [SerializeField] private Button _btnLvl1;
        [SerializeField] private Button _btnLvl2;
        [SerializeField] private Button _btnToMenu;
        [SerializeField] private Image _gamePanel;


        private Transform _cameraTransf;
        private Transform _borderSampleTransf;
        private MapData _map;
        private Transform _leftBorder;
        private Transform _rightBorder;
        private Transform _topBorder;
        private Transform _bottomBorder;
        private float _heightMenuCanvas;


        public void StartCustom(MapData mapDataArg)
        {
            _map = mapDataArg;

            _borderSampleTransf = _borderSample.transform;
            _cameraTransf = _camera.transform;

            _leftBorder = GameObject.Instantiate(_borderSample, Vector3.zero, Quaternion.identity).transform;
            _rightBorder = GameObject.Instantiate(_borderSample, Vector3.zero, Quaternion.identity).transform;
            _topBorder = GameObject.Instantiate(_borderSample, Vector3.zero, Quaternion.AngleAxis(90, Vector3.forward)).transform;
            _bottomBorder = GameObject.Instantiate(_borderSample, Vector3.zero, Quaternion.AngleAxis(90, Vector3.forward)).transform;

            _leftBorder.gameObject.SetActive(false);
            _rightBorder.gameObject.SetActive(false);
            _topBorder.gameObject.SetActive(false);
            _bottomBorder.gameObject.SetActive(false);

            _heightMenuCanvas = _gamePanel.GetComponent<RectTransform>().rect.height * _gameUi.transform.localScale.y;
        }
        public void StartGame()
        {
            _menu.enabled = false;
            _gameUi.enabled = true;

            _leftBorder.gameObject.SetActive(true);
            _rightBorder.gameObject.SetActive(true);
            _topBorder.gameObject.SetActive(true);
            _bottomBorder.gameObject.SetActive(true);

            SetCameraAndBorders();
        }
        public void EndGame()
        {
            _menu.enabled = true;
            _gameUi.enabled = false;

            _leftBorder.gameObject.SetActive(false);
            _rightBorder.gameObject.SetActive(false);
            _topBorder.gameObject.SetActive(false);
            _bottomBorder.gameObject.SetActive(false);

            SetScores(0);
        }
        public void SetScores(int scoresArg)
        {
            _lblScores.text = scoresArg.ToString();
        }


        private void SetCameraAndBorders()
        {
            Vector3 sizeBorder = _borderSampleTransf.localScale;
            Vector3 marginBorderH = new Vector3(sizeBorder.x / 2, 0, 0);
            Vector3 marginBorderV = new Vector3(0, sizeBorder.x / 2, 0);
            float widthWith2Border = _map.SizeX + sizeBorder.x * 2;
            float heightWith2Border = _map.SizeY + sizeBorder.x * 2;

            _leftBorder.position = _map.CenterLeft - marginBorderH;
            _rightBorder.position = _map.CenterRight + marginBorderH;
            _topBorder.position = _map.CenterTop + marginBorderV;
            _bottomBorder.position = _map.CenterBottom - marginBorderV;

            _leftBorder.localScale = sizeBorder.WithY(_map.SizeY);
            _rightBorder.localScale = sizeBorder.WithY(_map.SizeY);
            _topBorder.localScale = sizeBorder.WithY(widthWith2Border);
            _bottomBorder.localScale = sizeBorder.WithY(widthWith2Border);

            SetCameraToSeeMap(widthWith2Border, heightWith2Border, sizeBorder.x);
        }
        private float GetZPosCameraForViewArea(float widthAreaArg, float heightAreaArg)
        {
            float scalerForFillGameMapByHorizontal = widthAreaArg / heightAreaArg / _camera.aspect;
            float frustumHeight = heightAreaArg;

            if (scalerForFillGameMapByHorizontal > 1f)
                frustumHeight *= scalerForFillGameMapByHorizontal;

            return -(frustumHeight * 0.5f / Mathf.Tan(_camera.fieldOfView * 0.5f * Mathf.Deg2Rad));
        }
        private void SetCameraToSeeMap(float widthMapArg, float heightMapArg, float widthBorderArg)
        {
            float zPos = GetZPosCameraForViewArea(widthMapArg, heightMapArg);

            _cameraTransf.position = new Vector3(_map.MaxCell.x / 2f, _map.MaxCell.y / 2f, zPos);

            Vector3 pointEndMenu = _camera.ScreenToWorldPoint(new Vector3(0, _heightMenuCanvas, Mathf.Abs(zPos)));
            Vector3 pointStartMenu = _camera.ScreenToWorldPoint(new Vector3(0, 0, Mathf.Abs(zPos)));

            float heightMenuWorld = Mathf.Abs(pointEndMenu.y - pointStartMenu.y);
            float finalPosZ = GetZPosCameraForViewArea(widthMapArg, heightMapArg + heightMenuWorld);
            float offsetY = heightMenuWorld / 2 + 2 * widthBorderArg;
            Vector3 camPos = new Vector3(_map.MaxCell.x / 2f, _map.MaxCell.y / 2f - offsetY, finalPosZ);

            _cameraTransf.position = camPos;
        }
        private void OnValidate()
        {
            if (_lblScores == null)
                Debug.LogError("Label scores is null", this);

            if (_camera == null)
                Debug.LogError("Camera is null", this);

            if (_gameUi == null)
                Debug.LogError("Game UI is null", this);

            if (_menu == null)
                Debug.LogError("Menu is null", this);

            if (_borderSample == null)
                Debug.LogError("Border is null", this);

            if (_btnLvl1 == null)
                Debug.LogError("Button level 1 is null", this);

            if (_btnLvl2 == null)
                Debug.LogError("Button level 2 is null", this);

            if (_btnToMenu == null)
                Debug.LogError("Button to menu is null", this);

            if (_gamePanel == null)
                Debug.LogError("Game panel is null", this);
        }
    }
}

