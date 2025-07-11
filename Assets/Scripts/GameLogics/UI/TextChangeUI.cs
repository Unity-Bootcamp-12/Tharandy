using TMPro;
using UnityEngine;

public class TextChangeUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    public void SetText(string text)
    {
        _text.text = text;
    }
}
