using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SwitchButton : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI _textMeshProUGUI;
    [SerializeField] Image _image;
    public string positiveText = "Positive";
    public string negativeText = "Negative";

    public Sprite positiveSprite;
    public Sprite negativeSprite;

    public bool isPositive { get; private set; } = true;

    public void Switch()
    {
        isPositive = !isPositive;
        if (isPositive)
        {
            _textMeshProUGUI.text = positiveText;
            _image.sprite = positiveSprite;
        }
        else
        {
            _textMeshProUGUI.text = negativeText;
            _image.sprite = negativeSprite;
        }
    }

    void Awake()
    {
        if (_textMeshProUGUI == null)
        {
            _textMeshProUGUI = GetComponentInChildren<TextMeshProUGUI>();
        }
        _textMeshProUGUI.text = positiveText;

        if (_image == null)
        {
            _image = GetComponent<Image>();
        }

        if (positiveSprite == null)
        {
            positiveSprite = _image.sprite;
            negativeSprite = _image.sprite;
        }
        else
        {
            _image.sprite = positiveSprite;
        }
    }



}
