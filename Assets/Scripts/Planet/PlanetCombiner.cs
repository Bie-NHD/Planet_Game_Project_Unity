using UnityEngine;

public class PlanetCombiner : MonoBehaviour
{
    private int _layerIndex;
    private PlanetInfo _info;
    AudioManager audioManager;
   

    private void Awake()
    {
        _info = GetComponent<PlanetInfo>();
        _layerIndex = gameObject.layer; 
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == _layerIndex)
        {
            PlanetInfo info = collision.gameObject.GetComponent<PlanetInfo>();
           if(info != null)
            {
                if (_info.PlanetIndex == info.PlanetIndex)
                {
                    int thisID = gameObject.GetInstanceID();
                    int otherID = collision.gameObject.GetInstanceID();

                    if (thisID > otherID)
                    {
                        GameManager.instance.AddScore(_info.PointsWhenAnnihilated);
                        if (_info.PlanetIndex == PlanetSelector.instance.Planets.Length - 1)
                        {
                            Destroy(collision.gameObject);
                            Destroy(gameObject);

                        }
                        else
                        {
                            audioManager.PlaySFX(audioManager.merge);

                         
                            Vector3 middlePosition = (transform.position + collision.transform.position) / 2;
                           
                            GameObject  go = Instantiate(SpawnCombinedPlanet(_info.PlanetIndex), GameManager.instance.transform);
                            go.transform.position = middlePosition;
                            ColliderInformer informer = go.GetComponent<ColliderInformer>();
                            if(informer != null)
                            {
                                informer.WasCombinedIn = true;
                            }
                            Destroy(collision.gameObject);
                            Destroy(gameObject);
                        }
                    }
                
                }
            }
        }
    }
    private GameObject SpawnCombinedPlanet(int index)
    {
        GameObject go = PlanetSelector.instance.Planets[index + 1];
        return go;
    }

}
