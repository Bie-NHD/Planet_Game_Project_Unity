using UnityEngine;

public class PlanetInfo : MonoBehaviour
{
    [Range(0, 10)]
    public int PlanetIndex = 0;
    public int PointsWhenAnnihilated = 1;

    [Range(1f, 1.5f)]
    public float PlanetMass = 1;

    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();

        _rb.mass = PlanetMass;
    }
}
