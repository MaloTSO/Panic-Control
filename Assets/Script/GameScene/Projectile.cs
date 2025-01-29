using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    float speed = 12f;

    private void FixedUpdate()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //pour tuer les ennemis
        var enemy = collision.gameObject.GetComponent<Enemy>();

        if(enemy != null)
        {
            enemy.Hit(25);
            Destroy(gameObject);
        }

    }


}
