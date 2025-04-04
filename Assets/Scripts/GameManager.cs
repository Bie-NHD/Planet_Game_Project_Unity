using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int currentScore { get; set; }

    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private Image _panelGameOver;
    [SerializeField] private Image _panelGameOn;
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _menuButton;
    [SerializeField] private float _fadeTime = 2f;

    public float TimeTillGameOver = 1.5f;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += FadeGame;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= FadeGame;
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        _scoreText.text = currentScore.ToString("0");
    }
    public void AddScore(int score)
    {
        currentScore += score;
        _scoreText.text = currentScore.ToString("0");
    }

    public void GameOver()
    {
        StartCoroutine(ShowGameOverScreen());
    }

    private IEnumerator ShowGameOverScreen()
    {
        _panelGameOver.gameObject.SetActive(true);
        Color startColor = _panelGameOver.color;
        startColor.a = 0f;
        _panelGameOver.color = startColor;

        float elapsedTime = 0f;
        while (elapsedTime < _fadeTime)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(0f, 1f, elapsedTime / _fadeTime);
            startColor.a = newAlpha;
            _panelGameOver.color = startColor;
            yield return null;
        }

        // Hiện nút restart sau khi GameOver xuất hiện
        //
        //TODO(Duyen): replace _restartButton with [Transform.getChild(1).gameObject] to get the button in the panel
        //(Duyen) Since the restart button appears more than once across the game, we could refactor the button/game object to a prefab
        _restartButton.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(0.5f);
        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void FadeGame(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(FadeGameIn());
    }
    private IEnumerator FadeGameIn()
    {
        _panelGameOn.gameObject.SetActive(true);
        Color startColor = _panelGameOn.color;
        startColor.a = 1f;
        _panelGameOn.color = startColor;

        float elapsedTime = 0f;
        while (elapsedTime < _fadeTime)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(1f, 0f, elapsedTime / _fadeTime);
            startColor.a = newAlpha;
            _panelGameOn.color = startColor;
            yield return null;
        }
        _panelGameOn.gameObject.SetActive(false);
    }

    public void OpenMenu()
    {

    }
}
