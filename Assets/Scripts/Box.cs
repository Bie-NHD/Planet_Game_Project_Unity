using UnityEngine;

public class Box : MonoBehaviour
{
    void ResizeBoxToScreen()
    {
        
        float screenWidth = Camera.main.orthographicSize * 2f * Screen.width / Screen.height;
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            float spriteWidth = sr.bounds.size.x;
            float scaleX = screenWidth / spriteWidth;

            transform.localScale = new Vector3(scaleX, transform.localScale.y, 1f);
        }
    }
    void Start()
    {
        ResizeBoxToScreen();
    }

   
}
