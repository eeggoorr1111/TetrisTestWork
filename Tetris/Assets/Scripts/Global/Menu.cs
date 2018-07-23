using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Tetris.Interfaces;
using Tetris.Generator;
using Tetris.GameObjects;
using Tetris.Transform;
using Tetris.Commands;

namespace Tetris.Global
{
    public class Menu : MonoBehaviour
    {
        private List<AbstractConfigurator> _configs;
        private AbstractConfigurator _configMenu;
        public GameObject _prefabButton;
        public Camera _prefabCamera;
        private List<Button> _buttons; 

        private IMap _map;
        public GameObject _prefabCube;
        
        private IFigureGenerator _generator;
        private IFactoryFigure _factory;
        private int _delayGenerate = 30;
        private int _counterFrameGenerate = 0;
        
        private IMover _mover;
        private Vector3 _vectorDrop;
        private Vector3 _vectorDropFrame;
        private float _deltaTime;
        private int _delayDrop = 0;
        private int _counterFrameDrop = 0;
        
        private List<IFigure> _figures;
        private List<IFigure> _figuresDelete;

        /// <summary>
        /// Класс описывает логику сцены меню
        /// </summary>
        public Menu()
        {
            _configs = new List<AbstractConfigurator>();
            _buttons = new List<Button>();
            _figures = new List<IFigure>();
            _figuresDelete = new List<IFigure>();
        }

        private void Start()
        {
            Button newButton;

            _configMenu = new ConfigurationMenu("МЕНЮ", _prefabCamera, _prefabCube);
            _configs.Add(new ConfigurationMode1("ПЕРВЫЙ РЕЖИМ", _prefabCamera, _prefabCube));
            _configs.Add(new ConfigurationMode2("ВТОРОЙ РЕЖИМ", _prefabCamera, _prefabCube));

            _map = _configMenu.CreateMap();
            _generator = _configMenu.CreateGenerator();
            _factory = _configMenu.CreateFactoryFigure();
            _mover = _configMenu.CreateMover();
            _vectorDrop = _configMenu.CreateVectorDrop();
            _delayDrop = _configMenu.DelayFrameDrop();

            _map.SetCamera(_prefabCamera);

            foreach (var config in _configs)
            {
                newButton = Instantiate(_prefabButton, this.transform).GetComponent<Button>();
                newButton.GetComponentInChildren<Text>().text = config.Name;
                newButton.onClick.AddListener(delegate { NewGame(config); });
                _buttons.Add(newButton);
            }

            newButton = Instantiate(_prefabButton, this.transform).GetComponent<Button>();
            newButton.GetComponentInChildren<Text>().text = "ВЫХОД";
            newButton.onClick.AddListener(ExitGame);
            _buttons.Add(newButton);
        }
        void Update()
        {
            bool isMove;
            _deltaTime = Time.deltaTime;
            _vectorDropFrame = new Vector3( _vectorDrop.x * _deltaTime,
                                            _vectorDrop.y * _deltaTime,
                                            _vectorDrop.z * _deltaTime);
            
            _counterFrameGenerate++;
            if (_counterFrameGenerate == _delayGenerate)
            {
                _counterFrameGenerate = 0;
                _figures.Add(_generator.NewFigure(_factory));
            }

            _counterFrameDrop++;
            if (_counterFrameDrop == _delayDrop)
            {
                _counterFrameDrop = 0;
                foreach (var figure in _figures)
                {
                    isMove = figure.Move(_mover, _vectorDropFrame);
                    if (!isMove)
                        _figuresDelete.Add(figure);
                }
                foreach (var figure in _figuresDelete)
                {
                    figure.Dispose();
                    _figures.Remove(figure);
                }
                _figuresDelete.Clear();
            }
        }

        public void NewGame(AbstractConfigurator config)
        {
            DataConfigs.SaveConfigurator(config);
            SceneManager.LoadScene(1);
        }
        public void ExitGame()
        {
            Application.Quit();
        }
    }
}