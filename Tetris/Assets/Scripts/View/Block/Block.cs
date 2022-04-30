using UnityEngine;
using Zenject;

namespace Tetris.View
{
    public sealed class Block : MonoBehaviour
    {
        public Transform Transf => _transf;


        private Transform _transf;


        private void StartCustom()
        {
            _transf = GetComponent<Transform>();
        }


        public class Pool : MonoMemoryPool<Vector2Int, Block>
        {
            protected override void OnCreated(Block itemArg)
            {
                itemArg.StartCustom();
                base.OnCreated(itemArg);
            }
        }
    }
}

