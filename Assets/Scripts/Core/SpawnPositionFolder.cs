using UnityEngine;

namespace Core
{
    public class SpawnPositionFolder : MonoBehaviour
    {
        [SerializeField] private Transform[] _spawnPoints;
        
        public Vector3 GetRandomPosition()
        {
            return _spawnPoints.GetRandom().position;
        }
    }
}