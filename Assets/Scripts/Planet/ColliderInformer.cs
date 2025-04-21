using UnityEngine;

public class ColliderInformer : MonoBehaviour
{
    public bool WasCombinedIn { get; set; }
    private bool _hasCollided;
 private float _collisionCooldown = 0.1f; // Thêm cooldown time
    private float _lastCollisionTime;
    private void OnEnable()
    {
        ResetCollisionState();
    }

    public void ResetCollisionState()
    {
        _hasCollided = false;
        WasCombinedIn = false;
        _lastCollisionTime = 0f;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
         if (Time.time - _lastCollisionTime < _collisionCooldown) return;
        if (!_hasCollided && !WasCombinedIn)
        {
            _hasCollided = true;
             _lastCollisionTime = Time.time;
            if (ThrowPlanetController.instance != null)
            {
                ThrowPlanetController.instance.CanThrow = true;
                ThrowPlanetController.instance.SpawnAPlanet(PlanetSelector.instance.NextPlanetTag);
                PlanetSelector.instance.PickNextPlanet();
            }
        }
    }
}
