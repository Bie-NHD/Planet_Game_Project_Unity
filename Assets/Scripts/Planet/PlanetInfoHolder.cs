using UnityEngine;
using UnityEngine.Pool;

public class PlanetInfoHolder : MonoBehaviour
{
    public IObjectPool<PlanetInfoHolder> ObjectPool { get; set; }

    [Range(0, 10)]
    public int PlanetIndex = 0;
    public int Points = 1;

    [Range(1f, 1.5f)]
    public float Mass = 1;

    // [Range(0.3f, 1f)]
    // public float Scale;

    public PlanetInfoHolder SetUp(PlanetDataStruct planetData)
    {
        PlanetIndex = planetData.Index;
        Points = planetData.Points;
        Mass = planetData.Mass;
        // Scale = planetData.Scale;

        transform.localScale = Vector3.one * planetData.Scale;

        return this;
    }

    public void SetUpRigidbody(Rigidbody2D rb)
    {
        rb.mass = Mass;
        rb.simulated = true;
    }
}
