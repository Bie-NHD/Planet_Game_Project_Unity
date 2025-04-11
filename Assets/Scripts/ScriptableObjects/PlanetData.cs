using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public struct PlanetDataStruct
{
    [Range(0, 10)]
    public int Index;
    public int Points;

    [Range(1f, 1.5f)]
    public float Mass;

    [Range(0.3f, 1.3f)]
    public float Scale;

    public PlanetDataStruct(int index, int points, float mass, float scale)
    {
        Index = index;
        Points = points;
        Mass = mass;
        Scale = scale;
    }

    public override string ToString()
    {
        return $"PlanetDataStruct: Index = {Index},\tPoints = {Points},\tMass = {Mass},\tScale = {Scale}";
    }
}

public class SerializedPlanetDataDictionary
    : UnityEngine.Rendering.SerializedDictionary<int, PlanetDataStruct> { }

[CreateAssetMenu(fileName = "PlanetData", menuName = "Scriptable Objects/PlanetData")]
public class PlanetData : ScriptableObject
{
    public static readonly int LayerIndex = 8;
    public static readonly String LayerName = "Planet";

    public GameObject planetPrefab;

    public List<Sprite> planetSprites;

    [SerializeField]
    public List<PlanetDataStruct> planetDataList = new List<PlanetDataStruct>()
    {
        new(0, 1, 1f, 0.3f),
        new(1, 2, 1.05f, 0.4f),
        new(2, 4, 1.1f, 0.5f),
        new(3, 8, 1.15f, 0.6f),
        new(4, 16, 1.2f, 0.7f),
        new(5, 32, 1.25f, 0.8f),
        new(6, 64, 1.3f, 0.9f),
        new(7, 128, 1.35f, 1.0f),
        new(8, 256, 1.4f, 1.1f),
        new(9, 512, 1.45f, 1.2f),
        new(10, 1024, 1.5f, 1.3f),
    };
}
