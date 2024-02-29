using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpPoint : MonoBehaviour
{
    Player player;
    float magnetRange;
    float magnetForce = 12f;
    public int exp;

    void Awake()
    {
        player = GameManager.instance.player;
        magnetRange = GameManager.instance.magnetRange;
    }
    public void Init(int val)
    {
        exp = val;
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

        GameManager.instance.getExp(exp);
        gameObject.SetActive(false);
    }
}
