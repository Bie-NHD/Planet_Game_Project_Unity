using UnityEngine;
using UnityEngine.UI;

public class PlanetSelector : MonoBehaviour
{
    public static PlanetSelector instance;

    public GameObject[] Planets;
    public GameObject[] NoPhysicsPlanets;
    public int HighestStartingIndex = 3;

    [SerializeField] private Image _nextPlanetImage;
    [SerializeField] private Sprite[] _planetSprites;

    public GameObject NextPlanet { get; private set; }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    public GameObject PickRandomPlanetForThrow()
    {
        int randomIndex = Random.Range(0, HighestStartingIndex + 1);
        if(randomIndex < NoPhysicsPlanets.Length)
        {
           GameObject randomPlanet = NoPhysicsPlanets[randomIndex];
            return randomPlanet;
        }
        return null;
       
    }
    public void PickNextPlanet()
    {
        int randomIndex = Random.Range(0, HighestStartingIndex + 1);
        if (randomIndex < Planets.Length)
        {
            GameObject nextPlanet = NoPhysicsPlanets[randomIndex]; ;
          
            NextPlanet = nextPlanet;
            _nextPlanetImage.sprite = _planetSprites[randomIndex];

        }
    
    }
}
