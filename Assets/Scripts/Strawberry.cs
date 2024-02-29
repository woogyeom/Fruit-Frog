using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class strawberry : MonoBehaviour
{
    Player player;
    float magnetRange;
    float magnetForce = 12f;
    public int hp;

    void Awake()
    {
        player = GameManager.instance.player;
        magnetRange = GameManager.instance.magnetRange;
    }
    void Update()
    {
        magnetRange = GameManager.instance.magnetRange;
        MagnetEffect();
    }

    void MagnetEffect()
    {
        if (player != null)
        {
            Vector3 direction = player.transform.position - transform.position;
            float distance = direction.magnitude;

            if (distance < magnetRange)
            {
                float magnetStrength = 1f - (distance / magnetRange);
                Vector3 force = direction.normalized * magnetForce * magnetStrength;
                transform.Translate(force * Time.deltaTime);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
        {
            return;
        }

        GameManager.instance.health = Mathf.Min(GameManager.instance.health + (GameManager.instance.maxHealth * hp / 100), GameManager.instance.maxHealth);
        gameObject.SetActive(false);
    }
}
