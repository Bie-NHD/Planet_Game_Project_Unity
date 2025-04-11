using UnityEngine;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class PlanetSelector : MonoBehaviour
{
    public static PlanetSelector instance;

    public GameObject[] Planets;
    public GameObject[] NoPhysicsPlanets;
    public int HighestStartingIndex = 3;

    [SerializeField]
    private Image _nextPlanetImage;

    [SerializeField]
    private Sprite[] _planetSprites;

    [SerializeField]
    private PlanetPooler _planetPooler;

    [SerializeField]
    private PlanetData _planetData;

    public GameObject NextPlanet { get; private set; }

    [Range(0, 10)]
    public static int NextPlanetIndex { get; private set; } = 0;

    [Range(0, 10)]
    public static int CurrentPlanetIndex { get; private set; } = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public GameObject PickRandomPlanetForThrow()
    {
        int randomIndex = Random.Range(0, HighestStartingIndex + 1);
        if (randomIndex < NoPhysicsPlanets.Length)
        {
            GameObject randomPlanet = NoPhysicsPlanets[randomIndex];
            return randomPlanet;
        }
        return null;
    }

    public void PickNextPlanet()
    {
        CurrentPlanetIndex = NextPlanetIndex;

        int randomIndex = Random.Range(0, HighestStartingIndex + 1);

        int indexWithLargestStack = Mathf.Clamp(
            _planetPooler.IndexWithLargestStack,
            0,
            HighestStartingIndex + 1
        );

        randomIndex = (Random.value > 0.5f) ? randomIndex : indexWithLargestStack;

        randomIndex = Mathf.Clamp(randomIndex, 0, HighestStartingIndex + 1);

        GameObject nextPlanet = NoPhysicsPlanets[randomIndex];

        NextPlanet = nextPlanet;

        _nextPlanetImage.sprite = _planetSprites[randomIndex];

        NextPlanetIndex = randomIndex;
    }
}
