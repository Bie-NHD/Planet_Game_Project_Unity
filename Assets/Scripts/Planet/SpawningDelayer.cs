using UnityEngine;

[RequireComponent(typeof(PlanetCombiner))]
public class SpawningDelayer : MonoBehaviour
{
    private bool hasCollidedFirstTime = false;

    private int _layer => gameObject.layer;

    private PlanetCombiner _planetCombiner;

    private void Awake()
    {
        TryGetComponent<PlanetCombiner>(out _planetCombiner);
        _planetCombiner.enabled = !hasCollidedFirstTime; // Disable the PlanetCombiner script until the first collision
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log(
            $"SpawningDelayer: {gameObject.name} collided with {other.gameObject.name}\nLayer: {other.gameObject.layer}"
        );
        // Layer 8 is for planets, and layer 7 is for the boundary
        if (other.gameObject.layer == 8 || other.gameObject.layer == 7) // Check if the collided object is not a planet
        {
            hasCollidedFirstTime = true;
            enabled = !hasCollidedFirstTime; // Disable this script to prevent further triggers
            _planetCombiner.enabled = true; // Enable the PlanetCombiner script after the first collision
            ThrowPlanetController.instance.ToggleCanThrow(canThrow: true);
        }
    }

    void OnEnable()
    {
        hasCollidedFirstTime = false; // Reset the collision state when the script is enabled
        _planetCombiner.enabled = false; // Enable the PlanetCombiner script after the first collision
    }
}
