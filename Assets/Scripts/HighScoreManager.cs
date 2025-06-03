using Configs;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class HighScoreManager : MonoBehaviour
{
    [SerializeField]
    private HighscorePrefConfig HighscorePrefConfig;

    public int Highscore => HighscorePrefConfig.HighScore;

    public UnityEvent<int> UpdateHighScoreEvent => HighscorePrefConfig.UpdateHighScoreEvent;

    [SerializeField]
    private TextMeshProUGUI _highScoreText;

    [SerializeField]
    private TextMeshProUGUI _newHighScoreText;

    public bool IsNewHighScore { get; protected set; } = false;

    void Start()
    {
        UpdateHighScoreEvent.AddListener(UpdateHighScoreText);

        _newHighScoreText.gameObject.SetActive(false);

        if (Highscore == 0)
        {
            _highScoreText.text = string.Empty;
        }
        else
        {
            _highScoreText.text = $"{Highscore:0}";
        }
    }

    public void UpdateHighScoreText(int value)
    {
        if (value > Highscore)
        {
            _highScoreText.text = $"{value:0}";
            HighscorePrefConfig.SetPref(value);

            IsNewHighScore = true;
            _newHighScoreText.gameObject.SetActive(true);
        }
    }
}
