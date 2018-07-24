using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tetris.Global
{
    public class Controller : MonoBehaviour
    {
        public Camera _camera;
        public Text _lblScores;
        public Button _btnExit;
        private TetrisGame _model;

        void Start()
        {
            if (_camera == null)
                throw new TetrisException("Во ViewController не передана камера");

            if (_lblScores == null)
                throw new TetrisException("Во ViewController не передано поле для вывода очков");

            if (_btnExit == null)
                throw new TetrisException("Во ViewController не передана кнопка выхода из игры");

            _model = new TetrisGame();
            _model.SetCamera(_camera);
            _model.ChangeScoresEvent += UpdateScores;
            _btnExit.onClick.AddListener(delegate { _model.GameOver(); });
        }

        void Update()
        {
            _model.NewFrame();
            if (Input.GetKeyDown(KeyCode.D))
                _model.Move(new Vector3Int(1, 0, 0));
            else if (Input.GetKeyDown(KeyCode.A))
                _model.Move(new Vector3Int(-1, 0, 0));
            else if (Input.GetKeyDown(KeyCode.W))
                _model.Move(new Vector3Int(0, 0, 1));
            else if (Input.GetKeyDown(KeyCode.S))
                _model.Move(new Vector3Int(0, 0, -1));
            else if (Input.GetKeyDown(KeyCode.Q))
                _model.Rotate(true);
            else if (Input.GetKeyDown(KeyCode.E))
                _model.Rotate(false);
        }

        void UpdateScores(int scoresArg)
        {
            _lblScores.text = scoresArg.ToString();
        }
    }
}
