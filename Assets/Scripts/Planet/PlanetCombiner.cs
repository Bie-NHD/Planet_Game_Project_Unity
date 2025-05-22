using System.Collections;
using UnityEngine;

public class PlanetCombiner : MonoBehaviour
{
    private int _layerIndex;
    private PlanetInfo _info;   
    private AudioManager audioManager;

 private bool _isProcessingMerge = false;
    private void OnEnable()
    {
        _info = GetComponent<PlanetInfo>();
        _layerIndex = gameObject.layer;

 _isProcessingMerge = false; 
        audioManager = GameObject.FindGameObjectWithTag("Audio")?.GetComponent<AudioManager>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var eyeAnimator = GetComponent<EyeAnimatorController>();
        if (eyeAnimator != null)
            eyeAnimator.SetCloseEye(true);
    StartCoroutine(OpenEyeAfterDelay(0.2f));
    
        if (_isProcessingMerge) return;
        if (collision.gameObject.layer == _layerIndex)
        {
            PlanetInfo otherInfo = collision.gameObject.GetComponent<PlanetInfo>();
            if (otherInfo != null && _info != null && _info.PlanetIndex == otherInfo.PlanetIndex)
            {
                      PlanetCombiner otherCombiner = collision.gameObject.GetComponent<PlanetCombiner>();
                if (otherCombiner != null && otherCombiner._isProcessingMerge) return;

                int thisID = gameObject.GetInstanceID();
                int otherID = collision.gameObject.GetInstanceID();

                if (thisID > otherID)
                {
                       _isProcessingMerge = true;
                    otherCombiner._isProcessingMerge = true;
                    HandlePlanetMerge(collision.gameObject, otherInfo);
                }
            }
        }
    }
private IEnumerator OpenEyeAfterDelay(float delay)
{
    yield return new WaitForSeconds(delay);
    var eyeAnimator = GetComponent<EyeAnimatorController>();
    if (eyeAnimator != null && !_isProcessingMerge) // Chỉ mở mắt nếu không đang trong quá trình merge
        eyeAnimator.SetCloseEye(false);
}
    private void HandlePlanetMerge(GameObject otherPlanet, PlanetInfo otherInfo)
    {
        if (_info == null)
            return;
        // Debug.Log($"Starting merge of planets with index {_info.PlanetIndex}");

        // GameManager.instance.AddScore(_info.PointsWhenAnnihilated);
        GameManager.UpdateScoreEvent.Invoke(_info.PointsWhenAnnihilated);

        Vector3 middlePosition = (transform.position + otherPlanet.transform.position) / 2;
        audioManager.PlaySFX(audioManager.merge);

        if (GameManager.instance.mergeEffectPrefab != null)
        {
            GameObject effect = Instantiate(
                GameManager.instance.mergeEffectPrefab,
                middlePosition,
                Quaternion.identity,
                GameManager.instance.MergeEffectLayer.transform
            );
            Destroy(effect, 2f);
        }

        if (_info.PlanetIndex >= PlanetSelector.instance.MaxMergeIndex)
        {
            Debug.Log(
                $"Reached maximum merge level ({PlanetSelector.instance.MaxMergeIndex}), destroying planets"
            );

            PlanetObjectPool.Instance.ReturnToPool(otherPlanet);
            PlanetObjectPool.Instance.ReturnToPool(gameObject);
        }
        else
        {
            // Debug.Log($"Merging planets to create index {_info.PlanetIndex + 1}");

            SpawnMergedPlanet(middlePosition, _info.PlanetIndex + 1);

            PlanetObjectPool.Instance.ReturnToPool(otherPlanet);
            PlanetObjectPool.Instance.ReturnToPool(gameObject);
        }
    }

    private void SpawnMergedPlanet(Vector3 position, int newIndex)
    {
        string newTag = $"animal_{newIndex}";
        // Debug.Log($"Attempting to spawn merged planet with tag {newTag}");
        GameObject newPlanet = PlanetObjectPool.Instance.SpawnFromPool(
            newTag,
            position,
            Quaternion.identity,
            true,
            GameManager.instance.AnimalHolderLayer.transform
        );

        if (newPlanet != null)
        {
            newPlanet.SetActive(true);

            var rb = newPlanet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
                rb.AddForce(
                    Vector3.up * (4f / Mathf.Max(2, _info.PlanetIndex)),
                    ForceMode2D.Impulse
                );
            }

            var informer = newPlanet.GetComponent<ColliderInformer>();
            if (informer != null)
            {
                informer.ResetCollisionState();
                informer.WasCombinedIn = true;
            }

            var planetInfo = newPlanet.GetComponent<PlanetInfo>();
            if (planetInfo != null)
            {
                planetInfo.PlanetIndex = newIndex;
            }
        }
        else
        {
            Debug.LogError($"Failed to spawn merged planet with tag: {newTag}");
        }
    }
}
