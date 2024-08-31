using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace UnityUtils
{
    public class UnityMainThreadDispatcher : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Initialize()
        {
            Instantiate(Resources.Load("UnityMainThreadDispatcher"));
        }

        private static readonly Queue<Action> _executionQueue = new Queue<Action>();


        private static UnityMainThreadDispatcher instance;
        public static UnityMainThreadDispatcher Instance { get { return instance; } }
        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }
            else
            {
                instance = this;
            }
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            lock (_executionQueue)
            {
                while (_executionQueue.Count > 0)
                {
                    Action action = _executionQueue.Dequeue();
                    action.Invoke();
                }
            }
        }

        public void Enqueue(Action action)
        {
            lock (_executionQueue)
            {
                _executionQueue.Enqueue(action);
            }
        }
    }
}
