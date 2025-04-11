using UnityEngine;
using UnityEngine.UI;

public class PlanetSelector : MonoBehaviour
{
    public static PlanetSelector instance;
    public int HighestStartingIndex = 3;
public int MaxMergeIndex = 11;   
    [SerializeField] private Image _nextPlanetImage;
    [SerializeField] private Sprite[] _planetSprites;


    public string NextPlanetTag { get; private set; }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
 
    public string PickRandomPlanetForThrow()
    {
        int randomIndex = Random.Range(0, HighestStartingIndex + 1);
        return "animal_" + randomIndex;
    }

    public void PickNextPlanet()
    {
        int randomIndex = Random.Range(0, HighestStartingIndex + 1);
        NextPlanetTag = "animal_" + randomIndex;

        if (randomIndex < _planetSprites.Length)
        {
            _nextPlanetImage.sprite = _planetSprites[randomIndex];
        }
    }
}
