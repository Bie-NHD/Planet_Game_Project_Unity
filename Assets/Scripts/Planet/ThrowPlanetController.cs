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

    public Bounds Bounds { get; private set; }

    private const float EXTRA_WIDTH = 0.1f;

    public bool CanThrow { get; set; } = true;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }


    void Start()
    {

        _playerController = GetComponent<PlayerController>();
        PlanetSelector.instance.PickNextPlanet();
        //SpawnAPlanet(_selector.PickRandomPlanetForThrow());
        SpawnAPlanet(PlanetSelector.instance.NextPlanet);
        PlanetSelector.instance.PickNextPlanet();
    }
    private void Update()
    {
        if (UserInput.IsThrowPressed && CanThrow)
        {

            SpriteIndex index = CurrentPlanet.GetComponent<SpriteIndex>();
            Quaternion rot = CurrentPlanet.transform.rotation;
            GameObject go = Instantiate(PlanetSelector.instance.Planets[index.Index], CurrentPlanet.transform.position, rot);
            go.transform.SetParent(_parentAfterThrow);
            Destroy(CurrentPlanet);
            CanThrow = false;
        }
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
