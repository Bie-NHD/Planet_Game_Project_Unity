using UnityEngine;

public class ColliderInformer : MonoBehaviour
{
    public bool WasCombinedIn { get; set; }

    private bool _hasCollided;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!_hasCollided && !WasCombinedIn)
        {
            _hasCollided = true;
            ThrowPlanetController.instance.CanThrow = true;
            ThrowPlanetController.instance.SpawnAPlanet(PlanetSelector.instance.NextPlanet);
            PlanetSelector.instance.PickNextPlanet();

            // Destroy(this); // Minimize Destroy calls to avoid performance issues
            enabled = false; // Disable this script to prevent further collisions
        }
    }

}
