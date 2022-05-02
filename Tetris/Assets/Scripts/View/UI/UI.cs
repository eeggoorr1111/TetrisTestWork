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
        [SerializeField] private Button _btnLvl1;
        [SerializeField] private Button _btnLvl2;
        [SerializeField] private Button _btnToMenu;
        [SerializeField] private Image _gamePanel;
        [SerializeField] private float _widthLineOfGrid;
        [SerializeField] private MeshFilter _grid;
        [SerializeField] private Transform _gridBack;


        private Transform _cameraTransf;
        private MapData _map;
        private float _heightMenuCanvas;
        private GridGenerator _gridGenerator;


        public void StartCustom(MapData mapDataArg)
        {
            _gridGenerator = new GridGenerator(_widthLineOfGrid);

            _map = mapDataArg;

            _cameraTransf = _camera.transform;

            _grid.gameObject.SetActive(false);
            _gridBack.gameObject.SetActive(false);

            _heightMenuCanvas = _gamePanel.GetComponent<RectTransform>().rect.height * _gameUi.transform.localScale.y;
        }
        public void StartGame()
        {
            _menu.enabled = false;
            _gameUi.enabled = true;

            _grid.gameObject.SetActive(true);
            _gridBack.gameObject.SetActive(true);

            SetCameraAndGrid();
        }
        public void EndGame()
        {
            _menu.enabled = true;
            _gameUi.enabled = false;

            _grid.gameObject.SetActive(false);
            _gridBack.gameObject.SetActive(false);

            SetScores(0);
        }
        public void SetScores(int scoresArg)
        {
            _lblScores.text = scoresArg.ToString();
        }


        private void SetCameraAndGrid()
        {
            float widthWith2Border = _map.SizeX + _widthLineOfGrid * 2;
            float heightWith2Border = _map.SizeY + _widthLineOfGrid * 2;

            _grid.mesh = _gridGenerator.Generate(_map.MaxCell2D);
            _gridBack.position = _map.Center.WithZ(_gridBack.position.z);
            _gridBack.localScale = new Vector3(_map.SizeX, _map.SizeY, 1);

            SetCameraToSeeMap(widthWith2Border, heightWith2Border, _widthLineOfGrid);
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
            
            if (_btnLvl1 == null)
                Debug.LogError("Button level 1 is null", this);

            if (_btnLvl2 == null)
                Debug.LogError("Button level 2 is null", this);

            if (_btnToMenu == null)
                Debug.LogError("Button to menu is null", this);

            if (_gamePanel == null)
                Debug.LogError("Game panel is null", this);

            if (_grid == null)
                Debug.LogError("Grid is null", this);

            if (_gridBack == null)
                Debug.LogError("Grid back is null", this);

        }
    }
}

