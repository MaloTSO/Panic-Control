using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{

    [SerializeField] GameObject enemyPrefab;
    [SerializeField] float timeBetweenSpawns = 0.5f;
    float currentTimeBetweenSpawns;
    Transform enemiesParent;
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
        currentTimeBetweenSpawns -= Time.deltaTime;

        if (currentTimeBetweenSpawns <= 0)
        {
            SpawnEnemy();
            currentTimeBetweenSpawns = timeBetweenSpawns;
        }
    }

    Vector2 RandomPosition()
    {
        // Largeur et hauteur de la carte
        float mapWidth = 41.99548f;
        float mapHeight = 36.12668f;

        // Affichage des dimensions de la carte pour débogage
        Debug.Log("mapWidth: " + mapWidth + " mapHeight: " + mapHeight);

        // Récupérer la position de la caméra pour ajuster le centre de la carte
        Vector2 cameraPosition = (Vector2)Camera.main.transform.position;

        // Choisir un bord aléatoire (0: Haut, 1: Bas, 2: Gauche, 3: Droit)
        int edge = Random.Range(0, 4);

        Vector2 spawnPosition = Vector2.zero;

        switch (edge)
        {
            case 0: // Bord supérieur
                    // Générer une position aléatoire entre les limites gauche et droite de la carte, mais fixer la position y au bord supérieur
                spawnPosition = new Vector2(cameraPosition.x + Random.Range(-mapWidth / 2, mapWidth / 2), cameraPosition.y + mapHeight / 2);
                break;
            case 1: // Bord inférieur
                    // Générer une position aléatoire entre les limites gauche et droite de la carte, mais fixer la position y au bord inférieur
                spawnPosition = new Vector2(cameraPosition.x + Random.Range(-mapWidth / 2, mapWidth / 2), cameraPosition.y - mapHeight / 2);
                break;
            case 2: // Bord gauche
                    // Générer une position aléatoire entre les limites du haut et du bas de la carte, mais fixer la position x au bord gauche
                spawnPosition = new Vector2(cameraPosition.x - mapWidth / 2, cameraPosition.y + Random.Range(-mapHeight / 2, mapHeight / 2));
                break;
            case 3: // Bord droit
                    // Générer une position aléatoire entre les limites du haut et du bas de la carte, mais fixer la position x au bord droit
                spawnPosition = new Vector2(cameraPosition.x + mapWidth / 2, cameraPosition.y + Random.Range(-mapHeight / 2, mapHeight / 2));
                break;
        }

        // Retourner la position de spawn générée
        return spawnPosition;
    }




    void SpawnEnemy()
    {
        var e = Instantiate(enemyPrefab, RandomPosition(), Quaternion.identity);
        e.transform.SetParent(enemiesParent);
    }

    public void DestroyAllEnemies()
    {
        foreach (Transform enemy in enemiesParent)
        {
            Destroy(enemy.gameObject);
        }
    }
}
