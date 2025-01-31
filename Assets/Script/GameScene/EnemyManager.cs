using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{

    [SerializeField] GameObject enemyPrefab;
    [SerializeField] GameObject chargerPrefab;
    public float timeBetweenSpawns = 1f;
    float currentTimeBetweenSpawns;
    Transform enemiesParent;
    private bool canSpawn = true;
    private int zombieCount = 0;
    public static EnemyManager instance;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        canSpawn = true;
        GameObject enemiesObject = GameObject.Find("Enemies");
        if (enemiesObject != null)
        {
            enemiesParent = enemiesObject.transform;
        }
        else
        {
            Debug.LogError("L'objet 'Enemies' n'a pas été trouvé dans la scène !");
        }
    }

    private void Update()
    {

        if(!WaveManager.instance.WaveRunning()) return;
        
        currentTimeBetweenSpawns -= Time.deltaTime;

        if (currentTimeBetweenSpawns <= 0 && canSpawn)
        {
            SpawnEnemy();
            currentTimeBetweenSpawns = timeBetweenSpawns;
        }

        
    }

    Vector2 RandomPosition()
    {
        // Récupérer le GameObject "Map"
        GameObject map = GameObject.Find("Map");

        // Vérifier que "Map" existe et a un SpriteRenderer
        if (map == null)
        {
            Debug.LogError("Le GameObject 'Map' est introuvable !");
            return Vector2.zero;
        }

        SpriteRenderer mapRenderer = map.GetComponent<SpriteRenderer>();
        if (mapRenderer == null)
        {
            Debug.LogError("Le GameObject 'Map' n'a pas de SpriteRenderer !");
            return Vector2.zero;
        }

        // Position actuelle de la caméra
        Vector2 cameraPosition = (Vector2)Camera.main.transform.position;

        // Taille visible de la caméra
        float cameraHalfHeight = Camera.main.orthographicSize;  // Hauteur visible de la caméra
        float cameraHalfWidth = cameraHalfHeight * Camera.main.aspect;  // Largeur visible de la caméra

        // Calcul des limites visibles de la caméra
        float cameraLeft = cameraPosition.x - cameraHalfWidth;
        float cameraRight = cameraPosition.x + cameraHalfWidth;
        float cameraTop = cameraPosition.y + cameraHalfHeight;
        float cameraBottom = cameraPosition.y - cameraHalfHeight;

        // Calcul des limites de la carte
        float mapLeft = mapRenderer.bounds.min.x;
        float mapRight = mapRenderer.bounds.max.x;
        float mapTop = mapRenderer.bounds.max.y;
        float mapBottom = mapRenderer.bounds.min.y;

        // Créer une liste des bords possibles autour de la caméra (0: Haut, 1: Bas, 2: Gauche, 3: Droit)
        List<int> possibleEdges = new List<int> { 0, 1, 2, 3 };

        // Exclure les bords où la caméra est proche (en tenant compte d'une marge de sécurité)
        float margin = 1.0f; 
        if (cameraPosition.y + cameraHalfHeight - margin >= cameraTop) possibleEdges.Remove(0); // Trop proche du bord supérieur
        if (cameraPosition.y - cameraHalfHeight + margin <= cameraBottom) possibleEdges.Remove(1); // Trop proche du bord inférieur
        if (cameraPosition.x - cameraHalfWidth + margin <= cameraLeft) possibleEdges.Remove(2); // Trop proche du bord gauche
        if (cameraPosition.x + cameraHalfWidth - margin >= cameraRight) possibleEdges.Remove(3); // Trop proche du bord droit

        // Si tous les bords sont exclus (ce qui est peu probable), autoriser tous les bords
        if (possibleEdges.Count == 0)
        {
            possibleEdges = new List<int> { 0, 1, 2, 3 };
        }

        // Choisir un bord aléatoire parmi les bords restants
        int edge = possibleEdges[Random.Range(0, possibleEdges.Count)];

        Vector2 spawnPosition = Vector2.zero;

        // Calculer la position de spawn en fonction du bord choisi
        switch (edge)
        {
            case 0: // Bord supérieur
                spawnPosition = new Vector2(
                    Random.Range(cameraLeft, cameraRight),  // Entre les limites gauche/droite visibles de la caméra
                    cameraTop                               // Bord supérieur visible de la caméra
                );
                break;
            case 1: // Bord inférieur
                spawnPosition = new Vector2(
                    Random.Range(cameraLeft, cameraRight),  // Entre les limites gauche/droite visibles de la caméra
                    cameraBottom                            // Bord inférieur visible de la caméra
                );
                break;
            case 2: // Bord gauche
                spawnPosition = new Vector2(
                    cameraLeft,                             // Bord gauche visible de la caméra
                    Random.Range(cameraBottom, cameraTop)  // Entre les limites haut/bas visibles de la caméra
                );
                break;
            case 3: // Bord droit
                spawnPosition = new Vector2(
                    cameraRight,                            // Bord droit visible de la caméra
                    Random.Range(cameraBottom, cameraTop)  // Entre les limites haut/bas visibles de la caméra
                );
                break;
        }

        // Si la position calculée est en dehors de la carte, la déplacer vers le bord opposé de la carte
        if (spawnPosition.x < mapLeft)
        {
            spawnPosition.x = mapRight;  // Déplacer vers le bord droit
        }
        else if (spawnPosition.x > mapRight)
        {
            spawnPosition.x = mapLeft;   // Déplacer vers le bord gauche
        }

        if (spawnPosition.y < mapBottom)
        {
            spawnPosition.y = mapTop;    // Déplacer vers le bord supérieur
        }
        else if (spawnPosition.y > mapTop)
        {
            spawnPosition.y = mapBottom; // Déplacer vers le bord inférieur
        }

        // Retourner la position de spawn générée
        return spawnPosition;
    }

    public void SpawnEnemy()
    {
        var roll = Random.Range(0, 100);
        var enemyType = roll < 90 ? enemyPrefab : chargerPrefab;


        var e = Instantiate(enemyType, RandomPosition(), Quaternion.identity);
        e.transform.SetParent(enemiesParent);
    }

    public void DestroyAllEnemies()
    {
        foreach (Transform enemy in enemiesParent)
        {
            Destroy(enemy.gameObject);
        }
    }

    public void StopSpawning(){
        canSpawn = false;
    }

    public void StartSpawning(){
        canSpawn = true;
    }

    public void IncreaseZombieCount()
    {
        zombieCount++;
    }

    public void DecreaseZombieCount()
    {
        zombieCount = Mathf.Max(0, zombieCount - 1);
    }

        public int GetZombieCount()
    {
        return zombieCount;
    }
    
}
