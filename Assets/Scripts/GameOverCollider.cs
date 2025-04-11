using UnityEngine;

public class GameOverCollider : MonoBehaviour
{
    private bool m_isColliding = false;

    private float m_timer = 0f;

    [Range(0.5f, 2f)]
    [SerializeField]
    private float TimeTillGameOver = 0.5f; //

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (HasRigidBody2D(collision))
        {
            m_isColliding = true;
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (m_isColliding)
        {
            m_timer += Time.deltaTime;
        }

        if (m_timer >= TimeTillGameOver)
        {
            if (HasRigidBody2D(collision))
            {
                GameManager.instance.GameOver();
                m_isColliding = false; // reset the collision state
                m_timer = 0f; // reset the timer
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // So far, not sure how to check there is NO collision with all objects
        m_isColliding = false;
        m_timer = 0f;
    }

    bool HasRigidBody2D(Collider2D collider2D)
    {
        Rigidbody2D rb = collider2D.attachedRigidbody;
        if (rb == null)
        {
            collider2D.gameObject.TryGetComponent<Rigidbody2D>(out rb);
        }

        return rb != null;
    }
}
