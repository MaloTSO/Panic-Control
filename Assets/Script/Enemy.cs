using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] int maxHealth = 100;
    int currentHealth;
    public float moveSpeed = 2f;
    private Transform player;


    void Start()
    {
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (player != null)
        {
            // Se déplacer vers le joueur
            Vector2 direction = (player.position - transform.position).normalized;
            transform.Translate(direction * moveSpeed * Time.deltaTime);

            // Regarder le joueur
            var playerToTheRight = player.position.x > transform.position.x;
            transform.localScale = new Vector2(playerToTheRight ? 0.3f : -0.3f, 0.3f);
            
        }
    }

    // Pour quand l'ennemi est touché
    public void Hit(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
