using System;
using Mirror;
using UnityEngine;

namespace Core
{
    public class DependencySingleton : MonoBehaviour
    {
        public static DependencySingleton Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }
    }
}