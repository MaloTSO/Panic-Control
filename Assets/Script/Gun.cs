using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] GameObject muzzle;
    [SerializeField] Transform muzzlePosition;
    [SerializeField] GameObject projectilePrefab;

    [Header("Config")]

    [SerializeField] float fireDistance = 10;
    [SerializeField] float fireRate = 0.5f;


    [Header("Others")]
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

        foreach (Enemy enemy in enemies)
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

            if (angle > 90 || angle < -90)
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

        Debug.Log("transform.localScale: " + transform.localScale);

        Debug.Log("Transform.rotation.x: " + transform.rotation.x);
        Debug.Log("Transform.rotation.y: " + transform.rotation.y);
        Debug.Log("Transform.rotation.z: " + transform.rotation.z);

        Vector3 direction = closestEnemy.position - transform.position;
        direction.Normalize();

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        Quaternion rotation = Quaternion.Euler(0, 0, angle);

        var projectileGo = Instantiate(projectilePrefab, muzzlePosition.position, rotation);
        Destroy(projectileGo, 3f);

    }



}
