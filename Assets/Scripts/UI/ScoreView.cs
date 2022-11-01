using TMPro;
using UnityEngine;

namespace UI
{
    public class ScoreView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _id;
        [SerializeField] private TMP_Text _score;
        
        public void Init(string id)
        {
            _id.text = id;
        }

        public void Bind(int value)
        {
            _score.text = value.ToString(); 
        }
    }
}