using UnityEngine;

namespace Balance.Data
{
    [CreateAssetMenu(fileName = "EndGameConfig", menuName = "ScriptableObject/EndGameConfig", order = 0)]
    public class EndGameConfig : ScriptableObject
    {
        [SerializeField] private int _maxScore;
        [SerializeField] private float _delayTime;

        public int MaxScore => _maxScore;

        public float DelayTime => _delayTime;
    }
}