using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SwitchButton : MonoBehaviour
{
    [Header("-----------UI Elements----------")]
    [SerializeField]
    TextMeshProUGUI _textMeshProUGUI;

    [Tooltip(
        "Image rendered on the button attached to this GameObjecct. Can be fetched in Awake()."
    )]
    [SerializeField]
    Image _image;

    [Header("-----------Audio Type----------")]
    [Tooltip("Select the type of audio to toggle")]
    [SerializeField]
    AudioType audioType = AudioType.Sound;

    public string positiveText = "Positive";
    public string negativeText = "Negative";

    public Sprite positiveSprite;
    public Sprite negativeSprite;

    public bool isPositive { get; private set; } = true;

    private AudioManager _audioManager;

    public void Switch()
    {
        isPositive = !isPositive;
        _audioManager.ToggleAudio(audioType, isPositive);
        UpdateUI();
    }

    void Awake()
    {
        // 1. Get UI elements
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
        // 2. Check if the audio type is enabled or disabled

        _audioManager = GameManager.instance.AudioManager;

        switch (audioType)
        {
            case AudioType.Music:
                isPositive = _audioManager.IsEnabled(audioType);
                break;
            case AudioType.Sound:
                isPositive = _audioManager.IsEnabled(audioType);
                break;
        }
        // 3. Update UI based on the current state
        UpdateUI();
    }

    private void UpdateUI()
    {
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
}
