using UnityEngine;

public class GameOverCollider : MonoBehaviour
{
    private bool m_isColliding = false;

    private float m_timer = 0f;

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

        if (m_timer >= GameManager.instance.TimeTillGameOver)
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
        Rigidbody2D rb = other.attachedRigidbody;
        if (rb != null)
        {
            if (rb.linearVelocityY < 0)
            {
                m_isColliding = false; // reset the collision state
                m_timer = 0f; // reset the timer
            }
        }
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
