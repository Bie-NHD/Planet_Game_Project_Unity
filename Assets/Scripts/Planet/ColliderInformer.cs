using UnityEngine;

public class ColliderInformer : MonoBehaviour
{
    public bool WasCombinedIn { get; set; }
    private bool _hasCollided;

    private void OnEnable()
    {
        ResetCollisionState();
    }

    public void ResetCollisionState()
    {
        _hasCollided = false;
        WasCombinedIn = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!_hasCollided && !WasCombinedIn)
        {
            _hasCollided = true;
            if (ThrowPlanetController.instance != null)
            {
                ThrowPlanetController.instance.CanThrow = true;
                ThrowPlanetController.instance.SpawnAPlanet(PlanetSelector.instance.NextPlanetTag);
                PlanetSelector.instance.PickNextPlanet();
            }
        }
    }
}
