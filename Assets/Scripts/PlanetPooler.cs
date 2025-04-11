using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PlanetPooler : MonoBehaviour
{
    [SerializeField]
    private GameObject _planetPrefab;

    [SerializeField]
    private PlanetData _planetData;

    public readonly List<Stack<GameObject>> PlanetStacks = new(20);

    public readonly List<GameObject> m_noPhysPlanets = new(20);

    public List<GameObject> NoPhysPlanets => m_noPhysPlanets;

    [SerializeField]
    private GameManager _gameManager;

    [SerializeField]
    private Transform _activePlanetHolder;

    [SerializeField]
    private Transform _throwingPlanetHolder;

    [SerializeField]
    private PlanetSelector _planetSelector;

    public int IndexWithLargestStack = 0;

    private void Awake()
    {
        if (_gameManager == null)
        {
            _gameManager = GetComponentInParent<GameManager>();
        }
        if (_planetData == null)
        {
            _planetData = _gameManager.GameObjectIndex.PlanetData;
        }

        if (_activePlanetHolder == null)
        {
            _activePlanetHolder = _gameManager.GameObjectIndex.ActivePlanetHolder.transform;
        }

        if (_planetSelector == null)
        {
            _planetSelector = GetComponent<PlanetSelector>();
        }

        SetUpPools();
    }

    private GameObject CreateNewNoPhysPlanet(int index)
    {
        if (index < 0 || index > 10)
        {
            index = Mathf.Clamp(index, 0, 10);
        }

        GameObject newGameObj = Instantiate(_planetPrefab);
        PlanetInfoHolder planet = newGameObj.GetComponent<PlanetInfoHolder>();

        PlanetDataStruct planetData = _planetData.planetDataList[index];

        Debug.Log($"CreateNewNoPhysPlanet {planetData.ToString()}");

        planet.SetUp(planetData);
        // planet.ObjectPool = m_noPhysPlanetPool;

        newGameObj.TryGetComponent<SpriteRenderer>(out SpriteRenderer spriteRenderer);
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = _planetData.planetSprites[index];
        }

        return newGameObj;
    }

    private GameObject SetUpNoPhysPlanet(int i)
    {
        if (i > 5)
            return null;

        GameObject gameObject = CreateNewNoPhysPlanet(i);
        ToggleComponents(gameObject, false);
        gameObject.transform.SetParent(_throwingPlanetHolder, false);
        m_noPhysPlanets.Add(gameObject);
        gameObject.layer = 0;

        return gameObject;
    }

    private GameObject SetupPhysPlanet(int index)
    {
        Debug.Log($"SetupPhysPlanet {index}");

        GameObject gameObject = CreateNewNoPhysPlanet(index);
        gameObject.transform.SetParent(_activePlanetHolder, false);

        gameObject.TryGetComponent<PlanetInfoHolder>(out PlanetInfoHolder planetInfoHolder);
        planetInfoHolder.SetUpRigidbody(gameObject.AddComponent<Rigidbody2D>());

        ToggleComponents(gameObject, false);

        gameObject.layer = 8;

        return gameObject;
    }

    public GameObject GetNoPhysPlanet(int index, Vector3 position)
    {
        NoPhysPlanets.ForEach(planet => planet.SetActive(false));
        NoPhysPlanets[index].transform.position = position;
        NoPhysPlanets[index].SetActive(true);

        return NoPhysPlanets[index];
    }

    private void ToggleComponents(GameObject gameObject, bool enable)
    {
        gameObject.GetComponent<SpawningDelayer>().enabled = enable;
        gameObject.GetComponent<PlanetCombiner>().enabled = enable;
        gameObject.SetActive(enable);
    }

    private void SetUpPools()
    {
        for (int index = 0; index <= 10; index++)
        {
            SetUpNoPhysPlanet(index);

            PlanetStacks.Add(new(20));
        }
    }

    public void AddPlanetToStack(int index, GameObject planet)
    {
        if (index < 0 || index > 10)
        {
            index = Mathf.Clamp(index, 0, 10);
        }

        PlanetStacks[index].Push(planet);
        ToggleComponents(planet, false);

        if (PlanetStacks[index].Count > PlanetStacks[IndexWithLargestStack].Count)
        {
            IndexWithLargestStack = index;
        }
    }

    public GameObject GetPlanetFromStack(int index)
    {
        if (index < 0 || index > 10)
        {
            index = Mathf.Clamp(index, 0, 10);
        }

        if (PlanetStacks[index].Count == 0)
        {
            // PlanetStacks[index].Push(CreateNewPlanet(index).gameObject);
            PlanetStacks[index].Push(SetupPhysPlanet(index));
        }

        GameObject gameObject = PlanetStacks[index].Pop();

        ToggleComponents(gameObject, true);

        return gameObject;
    }
}
