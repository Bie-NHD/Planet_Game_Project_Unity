using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SwitchButton : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI _textMeshProUGUI;
    public string positiveText = "Positive";
    public string negativeText = "Negative";

    public bool isPositive { get; private set; } = true;

    public void Switch()
    {
        isPositive = !isPositive;
        if (isPositive)
        {
            _textMeshProUGUI.text = positiveText;
        }
        else
        {
            _textMeshProUGUI.text = negativeText;
        }
    }

    void Awake()
    {
        if (_textMeshProUGUI == null)
        {
            _textMeshProUGUI = GetComponentInChildren<TextMeshProUGUI>();
        }

        _textMeshProUGUI.text = positiveText;
    }



}
