using UnityEngine;
using UnityEngine.UI;

namespace Tetris.View
{
    [RequireComponent(typeof(Canvas))]
    public class GameUi : MonoBehaviour
    {
        [SerializeField] private Image _img;


        private Canvas _canvas;


        public void StartCustom()
        {
            // TODO: Позже решить нужно это или убрать
            if (_img == null)
                Debug.LogError("Not setted image", this);

            _canvas = GetComponent<Canvas>();
        }
        public void SetEnable(bool enableArg)
        {
            _canvas.enabled = enableArg;
        }
    }
}

