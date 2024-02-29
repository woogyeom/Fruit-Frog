using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage;
    public int per;
    public float projspeed;
    Rigidbody2D rigid;
    public float boomerang;
    public float semiMajorAxis = 2f;
    public float semiMinorAxis = 1f;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }
    public void Init(int damage, int per, Vector3 dir, float projspeed)
    {
        this.damage = damage;
        this.per = per;

        if (per >= 0)
        {
            if (per == 999)
            {
                boomerang = 2;
                Invoke("DeactivateBoomerang", 3f);
            }
            rigid.velocity = dir * projspeed;
        }
    }

    void Update()
    {
        if (boomerang > 0) {
            float time = Time.time;
            float x = semiMajorAxis * Mathf.Cos(time * boomerang);
            float y = semiMinorAxis * Mathf.Sin(time * boomerang);

            Vector2 velocity = new Vector2(x, y);
            rigid.velocity = velocity;

            float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemies") || per == -100)
        {
            return;
        }
        per--;
        if (per < 0) {
            rigid.velocity = Vector2.zero;
            gameObject.SetActive(false);
        }
    }
    void DeactivateBoomerang()
    {
        rigid.velocity = Vector2.zero;
        gameObject.SetActive(false);
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("BulletArea") || per == -100)
        {
            return;
        }

        gameObject.SetActive(false);
    }

}
