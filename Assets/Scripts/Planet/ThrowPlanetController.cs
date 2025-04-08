using UnityEngine;

public class ThrowPlanetController : MonoBehaviour
{
    public static ThrowPlanetController instance;

    private ThrowPlanetController() { }

    public GameObject CurrentPlanet { get; set; }

    [SerializeField] private Transform _planetTransform;
    [SerializeField] private Transform _parentAfterThrow;
    [SerializeField] private PlanetSelector _selector;

    private PlayerController _playerController;

    private Rigidbody2D _rb;
    private CircleCollider2D _circleCollider;

    AudioManager audioManager;

    public Bounds Bounds { get; private set; }

    private const float EXTRA_WIDTH = 0.05f;

    public bool CanThrow { get; set; } = true;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }


    void Start()
    {

        _playerController = GetComponent<PlayerController>();
        PlanetSelector.instance.PickNextPlanet();
        SpawnAPlanet(PlanetSelector.instance.NextPlanet);
        PlanetSelector.instance.PickNextPlanet();
    }
    public void ThrowPlanetAtPosition(Vector3 throwPosition)
    {
        if (!CanThrow || CurrentPlanet == null) return;

        audioManager.PlaySFX(audioManager.thow);
        SpriteIndex index = CurrentPlanet.GetComponent<SpriteIndex>();
        Quaternion rot = CurrentPlanet.transform.rotation;

        Vector3 spawnPosition = _planetTransform.position;

        // Chỉ sử dụng tọa độ X từ vị trí tap, giữ nguyên Y từ vị trí spawn ban đầu
        spawnPosition.x = throwPosition.x;

        GameObject go = Instantiate(
            PlanetSelector.instance.Planets[index.Index],
            spawnPosition, // Sử dụng vị trí spawn đã điều chỉnh
            rot
        );

        go.transform.SetParent(_parentAfterThrow);
        Destroy(CurrentPlanet);
        CanThrow = false;

       
    }
    private void Update()
    {
        if (CurrentPlanet != null)
        {
            Vector3 pos = _planetTransform.position;

            // Clamp theo left/right bounds
            float halfPlanetSize = Bounds.size.x / 2;
            float clampedX = Mathf.Clamp(pos.x, _playerController.LeftBound, _playerController.RightBound);

            pos.x = clampedX;
            CurrentPlanet.transform.position = pos;
        }
        //if (UserInput.IsThrowPressed && CanThrow)
        //{
        //    audioManager.PlaySFX(audioManager.thow);
        //    SpriteIndex index = CurrentPlanet.GetComponent<SpriteIndex>();
        //    Quaternion rot = CurrentPlanet.transform.rotation;

        //    GameObject go = Instantiate(PlanetSelector.instance.Planets[index.Index], CurrentPlanet.transform.position, rot);
        //    go.transform.SetParent(_parentAfterThrow);
        //    Destroy(CurrentPlanet);
        //    CanThrow = false;
        //}
    }
    public void SpawnAPlanet(GameObject Planet)
    {
        GameObject go = Instantiate(Planet, _planetTransform);
        CurrentPlanet = go;
        _circleCollider = CurrentPlanet.GetComponent<CircleCollider2D>();
        Bounds = _circleCollider.bounds;

        _playerController.ChangeBoundary(EXTRA_WIDTH);
    }

}
