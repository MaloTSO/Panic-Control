using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] int maxHealth = 100;

    [SerializeField] float moveSpeed = 3f;

    [Header("Charger")]
    [SerializeField] bool isCharger;
    [SerializeField] int healthCharger = 100;
    [SerializeField] float distanceToCharge = 5f;
    [SerializeField] float chargeSpeed = 10f;
    [SerializeField] float prepareTime = 2f;
    bool isCharging = false;
    bool isPreparingCharge = false;

    [Header("Boss")]
    [SerializeField] bool isBoss;
    [SerializeField] int healthBoss = 1000;
    [SerializeField] float moveSpeedBoss = 2f;



    
    int currentHealth;
    private Transform player;
    
    void Start()
    {
        if(isCharger)
        {
            currentHealth = healthCharger;
        }
        else if(isBoss)
        {
            currentHealth = healthBoss;
            moveSpeed = moveSpeedBoss;
        }
        else
        {
            currentHealth = maxHealth;
        }
        player = GameObject.FindGameObjectWithTag("Player").transform;
        EnemyManager.instance.IncreaseZombieCount();
    }

    void Update()
    {

        if(isPreparingCharge) return;
        
        if (player != null)
        {
            // Se déplacer vers le joueur
            Vector2 direction = (player.position - transform.position).normalized;
            transform.Translate(direction * moveSpeed * Time.deltaTime);

            // Regarder le joueur
            var playerToTheRight = player.position.x > transform.position.x;

            if (isBoss)
            {
                transform.localScale = new Vector2(playerToTheRight ? 1f : -1f, 1f);
            }
            else{
                transform.localScale = new Vector2(playerToTheRight ? 0.3f : -0.3f, 0.3f);
            }
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
