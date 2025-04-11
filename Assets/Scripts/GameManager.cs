using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int currentScore { get; set; }

    [SerializeField]
    private TextMeshProUGUI _scoreText;

    [SerializeField]
    private Image _panelGameOver;

    [SerializeField]
    private Image _panelGameOn;

    [SerializeField]
    private Button _menuButton;

    [SerializeField]
    private float _fadeTime = 2f;

    public GameObject mergeEffectPrefab;

    public GameObject AnimalHolderLayer { get; private set; }

    public GameObject MergeEffectLayer { get; private set; }

    public AudioManager AudioManager { get; private set; }

    public float TimeTillGameOver = 0.5f;

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

        AnimalHolderLayer = transform.GetChild(0).gameObject;
        MergeEffectLayer = transform.GetChild(1).gameObject;
        AudioManager = GetComponentInChildren<AudioManager>();
    }

    public void AddScore(int score)
    {
        currentScore += score;
        _scoreText.text = currentScore.ToString("0");
    }

    public void GameOver()
    {
        StartCoroutine(ShowGameOverScreen());
        GetComponentInChildren<PlayerInput>().enabled = false;

        Time.timeScale = 0f;
        StartCoroutine(PlayGameOverSound());
    }

    private IEnumerator ShakeCamera()
    {
        Transform mainCameraTransform = Camera.main.transform;
        Vector3 originalPosition = mainCameraTransform.localPosition;

        for (int i = 0; i < 4; i++)
        {
            mainCameraTransform.localPosition = new Vector3(
                UnityEngine.Random.Range(-1f, 1f),
                0,
                0
            );
            yield return null;
        }
        mainCameraTransform.localPosition = originalPosition;

        yield return null;
    }

    private IEnumerator PlayGameOverSound()
    {
        AudioManager.ToggleMusic(false);
        yield return new WaitForSecondsRealtime(0.5f);
        AudioManager.PlaySFX(AudioManager.gameOver);
        yield return new WaitForSecondsRealtime(4f); // GameOver sound duration
        AudioManager.ToggleMusic(true);
    }

    private IEnumerator ShowGameOverScreen()
    {
        // StartCoroutine(ShakeCamera());
        // Show gameOver screen
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
        _panelGameOver.transform.GetChild(1).gameObject.SetActive(true);
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

        // StartCoroutine(StartGameTextAnimation());

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

    public void OpenMenu() { }
}
