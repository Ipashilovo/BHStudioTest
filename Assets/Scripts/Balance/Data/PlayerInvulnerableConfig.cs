using UnityEngine;

namespace Balance.Data
{
    [CreateAssetMenu(fileName = "PlayerInvulnerableConfig", menuName = "ScriptableObject/PlayerInvulnerableConfig", order = 0)]
    public class PlayerInvulnerableConfig : ScriptableObject
    {
        [SerializeField] private float _time;
        [SerializeField] private Color _color;

        public float Time => _time;
        public Color Color => _color;
    }
}