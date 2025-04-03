using UnityEngine;

public class TriggerLoss : MonoBehaviour
{
    private float _timer = 0f;
    private bool _isColliding = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            _isColliding = true;
            _timer = 0f;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (_isColliding && collision.gameObject.layer == 6)
        {
            _timer += Time.deltaTime;
            if (_timer >= GameManager.instance.TimeTillGameOver)
            {
                GameManager.instance.GameOver();
                _isColliding = false; 
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            _isColliding = false;
            _timer = 0f;
        }
    }
}
