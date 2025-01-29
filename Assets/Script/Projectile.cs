using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 18f;

    private void FixedUpdate()
    {
        // Ajuster la direction en fonction de l'orientation du gun
        Vector2 direction = transform.right;

        // Si la direction de l'arme est inversée, ajuster la direction du projectile
        if (transform.localScale.x < 0)  // Vérifie si l'arme est inversée
        {
            direction = -direction; 
        }

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = direction * speed;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var enemy = collision.gameObject.GetComponent<Enemy>();

        if (enemy != null)
        {
            enemy.Hit(25);
            Destroy(gameObject);
        }
    }
}
