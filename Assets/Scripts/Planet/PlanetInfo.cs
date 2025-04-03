using UnityEngine;

public class PlanetInfo : MonoBehaviour
{
    public int PlanetIndex = 0;
    public int PointsWhenAnnihilated = 1;
    public float PlanetMass = 1;

    private Rigidbody2D _rb;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        
        _rb.mass = PlanetMass;
    }
}
