using JetBrains.Annotations;
using UnityEngine;

public class GameObjectIndex : MonoBehaviour
{
    public GameManager GameManager => m_GameManager;

    public AudioManager AudioManager => m_AudioManager;

    private GameManager m_GameManager;

    private AudioManager m_AudioManager;

    public GameObject PlayerPlanetHolder;

    public GameObject ActivePlanetHolder;

    public GameObject EffectHolder;

    public GameObject MergeObjectPrefab;

    public PlanetData PlanetData;

    void Awake()
    {
        m_GameManager = GetComponent<GameManager>();
        m_AudioManager = GetComponentInChildren<AudioManager>();
    }
}
