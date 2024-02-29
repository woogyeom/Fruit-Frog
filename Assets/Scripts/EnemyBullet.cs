using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public int damage;
    public float projspeed;
    Rigidbody2D rigid;
    
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    public void Init(int damage, Vector3 dir, float projspeed)
    {
        this.damage = damage;
        rigid.velocity = dir * projspeed;

        CancelInvoke("DeactivateBullet");
        Invoke("DeactivateBullet", 2f);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
        {
            return;
        }

        rigid.velocity = Vector2.zero;
        gameObject.SetActive(false);

        GameManager.instance.player.GetComponent<Animator>().SetTrigger("Hit");
        GameManager.instance.health -= this.damage;
        if (GameManager.instance.health < 1)
        {
            GameManager.instance.player.GetComponent<Player>().isDissolving = true;
            GameManager.instance.GameOver();
        }
    }

    void DeactivateBullet()
    {
        rigid.velocity = Vector2.zero;
        gameObject.SetActive(false);
    }
}
