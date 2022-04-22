using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Tetris
{
    public class Rotator
    {
        public Rotator(Difficulty difficultyArg)
        {
            _difficulty = difficultyArg;
        }


        protected Tween _rotate;
        protected Difficulty _difficulty;


        public void Rotate(ColliderFigure collider)
        {
            if (_rotate.IsActive())
                return;

            Matrix4x4 rotate = new Matrix4x4(   new Vector4(0f, -1f, 0f, 0f),
                                                new Vector4(1f, 0f, 0f, 0f),
                                                new Vector4(0f, 0f, 1f, 0f),
                                                new Vector4(0f, 0f, 0f, 1f));

            Matrix4x4 targetRotate = Matrix4x4.Rotate(collider.Rotate) * rotate;

            DOTween.To(() => collider.Rotate, collider.ToRotate, targetRotate.rotation.eulerAngles, _difficulty.TimeRotate).SetEase(Ease.OutSine);
        }
    }
}

