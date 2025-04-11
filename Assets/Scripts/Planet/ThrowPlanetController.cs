using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;

public class ThrowPlanetController : MonoBehaviour
{
    public static ThrowPlanetController instance;

    private ThrowPlanetController() { }

    public GameObject CurrentPlanet { get; set; }

    [SerializeField]
    private Transform _planetTransform;

    [SerializeField]
    private Transform _parentAfterThrow;

    [SerializeField]
    private PlanetSelector _selector;

    private PlayerController _playerController;

    private Rigidbody2D _rb;
    private CircleCollider2D _circleCollider;

    AudioManager audioManager;

    public Bounds Bounds { get; private set; }

    private const float EXTRA_WIDTH = 0.05f;

    public bool CanThrow { get; set; } = true;

    // private UnityEvent m_onPlanetThrowEvent;

    [SerializeField]
    PlanetPooler _planetPooler;

    [SerializeField]
    GameManager _gameManager;

    [SerializeField]
    private GameObject _planetPrefab;

    private int currentPlanetIndex => PlanetSelector.CurrentPlanetIndex;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();

        // m_onPlanetThrowEvent = new UnityEvent();
        // m_onPlanetThrowEvent.AddListener(OnPlanetThrowEvent);
    }

    void Start()
    {
        _playerController = GetComponent<PlayerController>();
        // PlanetSelector.instance.PickNextPlanet();
        // SpawnAPlanet(PlanetSelector.instance.NextPlanet);
        NewSpawnAPlanet();
        PlanetSelector.instance.PickNextPlanet();
    }

    public void ThrowPlanetAtPosition(Vector3 throwPosition)
    {
        if (!CanThrow)
            return;

        audioManager.PlaySFX(audioManager.Throw);
        // SpriteIndex index = CurrentPlanet.GetComponent<SpriteIndex>();
        // Quaternion rot = CurrentPlanet.transform.rotation;

        Vector3 spawnPosition = _planetTransform.position;

        // Chỉ sử dụng tọa độ X từ vị trí tap, giữ nguyên Y từ vị trí spawn ban đầu
        spawnPosition.x = throwPosition.x;
        spawnPosition.y = spawnPosition.y - 0.5f;

        //GameObject go = Instantiate(
        //    PlanetSelector.instance.Planets[index.Index],
        //    spawnPosition, // Sử dụng vị trí spawn đã điều chỉnh
        //    rot

        //go.transform.SetParent(_parentAfterThrow);
        //Destroy(CurrentPlanet);

        // GameObject physicsPrefab = PlanetSelector.instance.Planets[index.Index];
        // GameObject physicsPlanet = PlanetObjectPool.Instance.GetPhysicsPlanet(physicsPrefab);
        // physicsPlanet.transform.SetPositionAndRotation(spawnPosition, rot);
        // physicsPlanet.transform.SetParent(_parentAfterThrow);

        // GameObject noPhysicsPrefab = PlanetSelector.instance.NoPhysicsPlanets[index.Index];
        // PlanetObjectPool.Instance.ReturnNoPhysicsPlanet(noPhysicsPrefab, CurrentPlanet);
        // CurrentPlanet = null;

        // Duyen: Release the planet to the pool
        CanThrow = false;

        _planetPooler.NoPhysPlanets[PlanetSelector.CurrentPlanetIndex].SetActive(false);

        _planetPooler
            .GetPlanetFromStack(PlanetSelector.CurrentPlanetIndex)
            .transform.SetPositionAndRotation(spawnPosition, Quaternion.identity);

        // if (CurrentPlanet.TryGetComponent<PlanetInfoHolder>(out PlanetInfoHolder planetInfoHolder))
        // {
        //     _planetPooler.GetPlanetPool(currentPlanetIndex).Release(planetInfoHolder);
        // }
        // else
        // {
        //     Destroy(CurrentPlanet);
        //     CurrentPlanet = null;
        // }

        PlanetSelector.instance.PickNextPlanet();
    }

    private void Update()
    {
        if (CurrentPlanet != null)
        {
            Vector3 pos = _planetTransform.position;

            // Clamp theo left/right bounds

            pos.x = Mathf.Clamp(pos.x, _playerController.LeftBound, _playerController.RightBound);

            CurrentPlanet.transform.position = pos;
        }
    }

    public void SpawnAPlanet(GameObject Planet)
    {
        //GameObject go = Instantiate(Planet, _planetTransform);
        GameObject go = PlanetObjectPool.Instance.GetNoPhysicsPlanet(Planet);
        go.transform.SetParent(_planetTransform);
        go.transform.localPosition = Vector3.zero;
        CurrentPlanet = go;
        _circleCollider = CurrentPlanet.GetComponent<CircleCollider2D>();
        Bounds = _circleCollider.bounds;

        _playerController.ChangeBoundary(EXTRA_WIDTH);
    }

    // private void OnPlanetThrowEvent()
    // {
    //     if (CanThrow)
    //     {
    //         // SpawnAPlanet(_planetPooler.NoPhysPlanetPool.Get().gameObject);
    //         NewSpawnAPlanet();
    //     }
    // }

    public void ToggleCanThrow(bool canThrow = false)
    {
        CanThrow = canThrow;
        if (CanThrow)
        {
            NewSpawnAPlanet();
        }
    }

    private void NewSpawnAPlanet()
    {
        GameObject gameObject = _planetPooler.GetNoPhysPlanet(
            PlanetSelector.CurrentPlanetIndex,
            _planetTransform.position
        );
        Bounds = gameObject.GetComponent<CircleCollider2D>().bounds;
    }
}
