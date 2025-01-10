using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI healthText;

    Rigidbody2D rb;
    [SerializeField] float moveSpeed = 6f;

    int maxHealth = 100;
    int currentHealth;

    bool isDead = false;

    float moveHorizontal;
    float moveVertical;
    Vector2 movement;
    float facingDirection = 0.15f;


    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        currentHealth = maxHealth;
        healthText.text = maxHealth.ToString();
    }

    public void Update()
    {
        if (isDead)
        {
            movement = Vector2.zero;
            return;
        }

        moveHorizontal = Input.GetAxisRaw("Horizontal");
        moveVertical = Input.GetAxisRaw("Vertical");

        movement = new Vector2(moveHorizontal, moveVertical).normalized;

        // pour regarder dans la direction du mouvement
        if (moveHorizontal < 0)
        {
            facingDirection = 0.15f;
        }
        else if (moveHorizontal > 0)
        {
            facingDirection = -0.15f;
        }
        transform.localScale = new Vector2(facingDirection, 0.15f);

    }

    private void FixedUpdate()
    {

        rb.velocity = movement * moveSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //si on touche l'ennemi, on prend des dégâts de la valeur de l'argument passé à la méthode Hit
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        Debug.Log("Collision with " + enemy);
        Debug.Log("Collision with " + collision.gameObject.name);
        if (enemy != null)
        {
            Hit(20);
        }
    }

    void Hit(int damage)
    {
        //on enlève des points de vie au joueur
        currentHealth -= damage;
        healthText.text = Mathf.Clamp(currentHealth, 0, maxHealth).ToString();
        //si le joueur n'a plus de points de vie, on appelle la méthode Die
        if(currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        // appeler GameOver plus tard
    }
}
