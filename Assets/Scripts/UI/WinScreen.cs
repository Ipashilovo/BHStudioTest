using TMPro;
using UnityEngine;

namespace UI
{
    public class WinScreen : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        
        public void Init(string id)
        {
            _text.text = id;
            gameObject.SetActive(true);
        }
    }
}