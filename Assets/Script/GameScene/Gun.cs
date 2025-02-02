using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] GameObject muzzle;
    [SerializeField] Transform muzzlePosition;
    [SerializeField] GameObject projectilePrefab;

    [Header("Config")]

    [SerializeField] float fireDistance = 10;
    [SerializeField] float fireRate = 0.1f;

    /*********************************************************************************************/

    Transform player;
    Vector2 offset = new Vector2(1, 0f);

    private float timeSinceLastShot = 0f;
    private float timeSinceLastEnemyChange = 0f;
    private float timeSinceLastDirectionChange = 0f;


    Transform closestEnemy;
    //Animator anim;

    private void Start()
    {
        timeSinceLastShot = fireRate;
        player = GameObject.Find("Player").transform;
        SetOffset(offset);
    }

    void Update()
    {
        transform.position = (Vector2)player.position + offset;


        FindClosestEnemy();
        AimAtEnemy();
        Shooting();
    }

    private void FindClosestEnemy() 
    {
        timeSinceLastEnemyChange += Time.deltaTime;
        if (timeSinceLastEnemyChange < 1f) return;
        closestEnemy = null;
        float shortestDistance = Mathf.Infinity;

        Enemy[] enemies = FindObjectsOfType<Enemy>();

        foreach (Enemy enemy in enemies) // Trouver l'ennemi le plus proche
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);

            if (distance <= fireDistance && distance <= shortestDistance)
            {
                shortestDistance = distance;
                closestEnemy = enemy.transform;
            }

        }
        if (closestEnemy != null)
        {
            timeSinceLastEnemyChange = 0f; // Réinitialiser le délai après avoir changé la cible
        }

    }

    void AimAtEnemy() 
    {
        if (closestEnemy != null)
        {

            timeSinceLastDirectionChange += Time.deltaTime;

            Vector3 direction = closestEnemy.position - transform.position;
            direction.Normalize();

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            if (angle > 90 || angle < -90) //toute cette verif permet de mettre l'arme a gauche ou a droite du player en fonction de la position de l'ennemi
            {

                transform.rotation = Quaternion.Euler(0, 0, angle + 180f);
                transform.localScale = new Vector3(1f, 1f, 1f);


                if (timeSinceLastDirectionChange < 1f)
                    return;

                SetOffset(new Vector2(-1, 0f));

                timeSinceLastDirectionChange = 0f;

            }
            else
            {

                transform.rotation = Quaternion.Euler(0, 0, angle);
                transform.localScale = new Vector3(-1f, 1f, 1f);


                if (timeSinceLastDirectionChange < 1f)
                    return;



                SetOffset(new Vector2(1, 0f));

                timeSinceLastDirectionChange = 0f;

            }

            transform.position = (Vector2)player.position + offset;

        }
    }

    public void SetOffset(Vector2 o)
    {
        offset = o;
    }


    private void Shooting()
    {
        if (closestEnemy == null) return;

        timeSinceLastShot += Time.deltaTime;
        if (timeSinceLastShot >= fireRate)
        {
            Shoot();
            timeSinceLastShot = 0;
        }

    }

    public void Shoot()
    {
        var muzzleGo = Instantiate(muzzle, muzzlePosition.position, transform.rotation);
        muzzleGo.transform.SetParent(transform);
        Destroy(muzzleGo, 0.05f);


        Vector3 direction = closestEnemy.position - transform.position;
        direction.Normalize();

        // permet de regler le probleme de rotation de l'arme, on ajouter 180 degrés si l'angle est superieur a 90 ou inferieur a -90 dans la fonction AimAtEnemy
        // ici on retire 90 degrés pour que le projectile parte dans la direction de l'ennemi
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0, 0, angle);

        var projectileGo = Instantiate(projectilePrefab, muzzlePosition.position, rotation);
        Destroy(projectileGo, 3f);

    }



}
