using UnityEngine;

[RequireComponent(typeof(PlanetInfoHolder))]
public class PlanetCombiner : MonoBehaviour
{
    private PlanetInfoHolder _info;

    [SerializeField]
    AudioManager audioManager;

    private void Awake()
    {
        _info = GetComponent<PlanetInfoHolder>();

        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(
            $"PlanetCombiner: {gameObject.name} collided with {collision.gameObject.name}\nLayer: {collision.gameObject.layer}"
        );

        if (collision.gameObject.layer != 8 && gameObject.layer != 8)
        {
            return;
        }

        if (
            collision.gameObject.TryGetComponent<PlanetInfoHolder>(out PlanetInfoHolder otherInfo)
            == false
        )
        {
            return;
        }

        if (_info.PlanetIndex == otherInfo.PlanetIndex)
        {
            int thisID = gameObject.GetInstanceID();
            int otherID = collision.gameObject.GetInstanceID();

            if (thisID > otherID)
            {
                // TODO(Duyen): Inject dependency to GameManager or PlanetData
                if (_info.PlanetIndex != 10)
                {
                    OnMerge(other: collision.gameObject);
                }

                ReleaseToPool(collision.gameObject);
                ReleaseToPool(gameObject);
            }
        }
    }

    private GameObject SpawnCombinedPlanet(int index)
    {
        GameObject go = PlanetSelector.instance.Planets[index + 1];
        return go;
    }

    private void BounceUpEffect(GameObject gameObject, float scale = 4f)
    {
        if (gameObject.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
        {
            rb.AddForce(Vector3.up * scale, ForceMode2D.Impulse);
        }
    }

    private void OnMerge(GameObject other)
    {
        Vector3 middlePosition = (transform.position + other.transform.position) / 2;

        GameManager.instance.GetPlanetFromPool(_info.PlanetIndex + 1, middlePosition);
        // Bounce up effect
        BounceUpEffect(gameObject, 4f / Mathf.Max(2, _info.PlanetIndex));

        // (Duyen): Merge effect
        GameManager.instance.RunMergeEffect(middlePosition);

        GameManager.instance.AddScore(_info.Points);
        audioManager.PlaySFX(audioManager.merge);
    }

    private void ReleaseToPool(GameObject gameObject)
    {
        gameObject.TryGetComponent<PlanetInfoHolder>(out PlanetInfoHolder planetInfoHolder);
        if (planetInfoHolder != null)
        {
            // planetInfoHolder.ObjectPool.Release(planetInfoHolder);
            GameManager.instance.ReturnPlanetToPool(planetInfoHolder);
        }
        else
        {
            Debug.LogError("PlanetInfoHolder not found on the game object.");
        }
    }
}
