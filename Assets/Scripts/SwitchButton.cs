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

    // [Header("-----------Audio Type----------")]
    // [Tooltip("Select the type of audio to toggle")]
    // [SerializeField]
    // AudioType audioType = AudioType.Sound;

    public string positiveText = "Positive";
    public string negativeText = "Negative";

    public Sprite positiveSprite;
    public Sprite negativeSprite;

    public bool IsPositive => AudioPrefConfig.GetPref();

    // private AudioManager _audioManager;

    public AudioPrefConfig AudioPrefConfig;

    private Button _button;

    public void Switch()
    {
        AudioPrefConfig.TogglePref();
        UpdateUI();
    }

    // void OnEnable()
    // {
    //     _button.onClick.AddListener(Switch);
    // }

    // void OnDisable()
    // {
    //     _button.onClick.RemoveListener(Switch);
    // }

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

        TryGetComponent<Button>(out _button);

        // 2. Check if the audio type is enabled or disabled

        // _audioManager = GameManager.instance.AudioManager;

        // switch (audioType)
        // {
        //     case AudioType.Music:
        //         isPositive = _audioManager.IsEnabled(audioType);
        //         break;
        //     case AudioType.Sound:
        //         isPositive = _audioManager.IsEnabled(audioType);
        //         break;
        // }
        // 3. Update UI based on the current state
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (IsPositive)
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
