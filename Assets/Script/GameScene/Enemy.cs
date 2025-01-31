using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] int maxHealth = 100;

    [SerializeField] float moveSpeed = 2f;

    [Header("Charger")]
    [SerializeField] bool isCharger;
    [SerializeField] float distanceToCharge = 5f;
    [SerializeField] float chargeSpeed = 10f;
    [SerializeField] float prepareTime = 2f;

    bool isCharging = false;
    bool isPreparingCharge = false;

    
    int currentHealth;
    private Transform player;
    
    void Start()
    {
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        EnemyManager.instance.IncreaseZombieCount();
    }

    void Update()
    {
        // if(!WaveManager.instance.WaveRunning()) return; // Si la vague n'est pas en cours, on ne fait rien

        if(isPreparingCharge) return;
        
        if (player != null)
        {
            // Se déplacer vers le joueur
            Vector2 direction = (player.position - transform.position).normalized;
            transform.Translate(direction * moveSpeed * Time.deltaTime);

            // Regarder le joueur
            var playerToTheRight = player.position.x > transform.position.x;
            transform.localScale = new Vector2(playerToTheRight ? 0.3f : -0.3f, 0.3f);
        
            if(isCharger && !isCharging && Vector2.Distance(transform.position, player.position) < distanceToCharge)
            {
                isPreparingCharge = true;
                Invoke("StartCharging", prepareTime); //Invoke appelle la méthode StartCharging après prepareTime secondes
            }
        }
    }

    void StartCharging()
    {
        isPreparingCharge = false;
        isCharging = true;
        moveSpeed = chargeSpeed;
    }

    // Pour quand l'ennemi est touché
    public void Hit(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            EnemyManager.instance.DecreaseZombieCount();
            Destroy(gameObject);
        }
    }
}
