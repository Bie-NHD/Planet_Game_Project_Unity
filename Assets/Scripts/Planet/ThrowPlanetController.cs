using UnityEngine;

public class ThrowPlanetController : MonoBehaviour
{
    public static ThrowPlanetController instance;
    public GameObject CurrentPlanet { get; set; }

    [SerializeField] private Transform _planetTransform;
            [SerializeField] private Transform _parentAfterThrow;
    [SerializeField] private float _extraWidth = 0.05f;

    private PlayerController _playerController;
    private CircleCollider2D _circleCollider;
    private AudioManager audioManager;
    public Bounds Bounds { get; private set; }

    public bool CanThrow { get; set; } = true;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        audioManager = GameObject.FindGameObjectWithTag("Audio")?.GetComponent<AudioManager>();
    }

    void Start()
    {
        _playerController = GetComponent<PlayerController>();
        PlanetSelector.instance.PickNextPlanet();
        SpawnAPlanet(PlanetSelector.instance.NextPlanetTag);
        PlanetSelector.instance.PickNextPlanet();
    }
    public void SpawnAPlanet(string planetTag)
    {
        GameObject planet = PlanetObjectPool.Instance.SpawnFromPool(
            planetTag,
            _planetTransform.position,
            Quaternion.identity,
            false,
            _planetTransform
        );

        if (planet != null)
        {
            CurrentPlanet = planet;
            _circleCollider = CurrentPlanet.GetComponent<CircleCollider2D>();
            if (_circleCollider != null)
            {
                Bounds = _circleCollider.bounds;
                _playerController.ChangeBoundary(_extraWidth);
            }
        }
    }
    public void ThrowPlanetAtPosition(Vector3 throwPosition)    
    {
        if (!CanThrow || CurrentPlanet == null) return;
     
        audioManager.PlaySFX(audioManager.thow);
        
        string currentTag = CurrentPlanet.tag;
        Vector3 spawnPosition = _planetTransform.position;
        spawnPosition.x = throwPosition.x;

        GameObject physicsPlanet = PlanetObjectPool.Instance.SpawnFromPool(
            currentTag,
            spawnPosition,
            Quaternion.identity,
            true,
            _parentAfterThrow
        );

        if (physicsPlanet != null)
        {
            var rb = physicsPlanet.GetComponent<Rigidbody2D>();
            var eyeAnimator = physicsPlanet.GetComponent<EyeAnimatorController>();
            if (eyeAnimator != null)
                eyeAnimator.SetCloseEye(false);
        }

        PlanetObjectPool.Instance.ReturnToPool(CurrentPlanet);
        CurrentPlanet = null;
        CanThrow = false;
    }

    private void Update()
    {
        if (CurrentPlanet != null)
        {
            Vector3 pos = _planetTransform.position;
            float clampedX = Mathf.Clamp(pos.x, _playerController.LeftBound, _playerController.RightBound);
            pos.x = clampedX;
            CurrentPlanet.transform.position = pos;
        }
    }

    
}
