using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Tetris
{
    public sealed class DrawerGizmos : MonoBehaviour
    {
        public static DrawerGizmos Instance => _instance;


        private event Action _onDrawGizmos;
        private static DrawerGizmos _instance;


        public void SubscribeOnDrawGizmos(Action callbackArg)
        {
            _onDrawGizmos -= callbackArg;
            _onDrawGizmos += callbackArg;
        }
        public void UnsubscribeOnDrawGizmos(Action callbackArg)
        {
            _onDrawGizmos -= callbackArg;
        }


        private void OnEnable()
        {
            if (_instance != null)
                Debug.LogError($"На сцене присутствует более одного { GetType().Name }", this);
            _instance = this;
        }
        private void OnDisable()
        {
            _instance = null;
            _onDrawGizmos = null;
        }
        private void OnDrawGizmos()
        {
            if (_onDrawGizmos != null)
                _onDrawGizmos.Invoke();
        }
    }
}

