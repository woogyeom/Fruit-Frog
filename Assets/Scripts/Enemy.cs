using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{
    Rigidbody2D rigid;
    Collider2D coll;
    SpriteRenderer spriter;
    Animator anim;
    Material material;
    public RuntimeAnimatorController[] animCon;
    WaitForFixedUpdate wait;
    public Rigidbody2D target;
    public int spritetype;
    Ghost ghostScript;
    Collider2D precoll;

    public float speed;
    public int health;
    public int maxHealth;
    public SpawnData.EnemyType type;
    public int atk;
    public int exp;

    bool isLive;
    bool isPaused;
    bool isHit = false;
    bool isDissolving = false;
    float fade = 1f;
    

    // Start is called before the first frame update
    void Awake()
    {
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        coll = GetComponent<Collider2D>();
        wait = new WaitForFixedUpdate();
        material = GetComponent<SpriteRenderer>().material;
        ghostScript = GetComponent<Ghost>();
    }

    private void OnEnable()
    {
        transform.position = Vector3.zero;
        target = GameManager.instance.player.GetComponent<Rigidbody2D>();
        ModifyCollider(spritetype);
        coll.enabled = true;
        isLive = true;
        health = maxHealth;
        rigid.simulated = true;
        spriter.sortingOrder = 0;
        fade = 1f;
        material.SetFloat("_Fade", fade);
    }
    
    public void Init(SpawnData data)
    {
        spritetype = data.spriteType;
        anim.runtimeAnimatorController = animCon[data.spriteType];
        speed = data.speed;
        maxHealth = data.health;
        health = data.health;
        type = data.enemyType;
        atk = data.atk;
        exp = data.exp;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Bullet") || !isLive)
        {
            return;
        }

        health -= collision.GetComponent<Bullet>().damage;

        if (health > 0) {
            anim.SetTrigger("Hit");
            StartCoroutine("KnockBack");
        } else {
            StartCoroutine(HandleDeath());
        }

    }

    void FixedUpdate()
    {
        if (!GameManager.instance.isLive)
        {
            return;
        }
        if (!isLive)
        {
            return;
        }
        if (!isHit)
        {
            if (type == SpawnData.EnemyType.Melee)
            {
                Vector2 dirVec = target.position - rigid.position;
                Vector2 nextVec = speed * Time.fixedDeltaTime * dirVec.normalized;
                rigid.MovePosition(rigid.position + nextVec);
                rigid.velocity = Vector2.zero;

                if (dirVec.magnitude > 0.1f)
                {
                    anim.SetFloat("Speed", 1f);
                }
                else
                {
                    anim.SetFloat("Speed", 0f);
                }
            } 
            else if (type == SpawnData.EnemyType.Ranged)
            {
                float distanceToPlayer = Vector2.Distance(transform.position, GameManager.instance.player.transform.position);

                if (distanceToPlayer > 3.5f && !isPaused)
                {
                    Vector2 dirVec = target.position - rigid.position;
                    Vector2 nextVec = speed * Time.fixedDeltaTime * dirVec.normalized;
                    rigid.MovePosition(rigid.position + nextVec);
                    rigid.velocity = Vector2.zero;

                    if (dirVec.magnitude > 0.1f)
                    {
                        anim.SetFloat("Speed", 1f);
                    }
                    else
                    {
                        anim.SetFloat("Speed", 0f);
                    }
                }
                else if (distanceToPlayer <= 3.5f && !isPaused)
                {
                    Fire();
                    StartCoroutine(MoveAndPauseRoutine());
                }
                else if (isPaused)
                {
                    rigid.velocity = Vector2.zero;
                    anim.SetFloat("Speed", 0f);
                }
            }
        }
    }

    void LateUpdate()
    {
        if (!GameManager.instance.isLive)
        {
            return;
        }
        if (!isLive)
        {
            if (isDissolving)
		    {
                fade -= Time.deltaTime;

                if (fade <= 0f)
                {
                    fade = 0f;
                    isDissolving = false;
                }
                // Set the property
                material.SetFloat("_Fade", fade);
		    }
        }
        spriter.flipX = target.position.x > rigid.position.x;
    }

    IEnumerator KnockBack()
    {
        isHit = true;
        yield return wait; // 1프레임 대기
        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 dirVec = transform.position - playerPos;
        rigid.AddForce(dirVec.normalized * 5, ForceMode2D.Impulse);
        yield return wait;
        isHit = false;
    }

    IEnumerator HandleDeath()
    {
        isLive = false;
        coll.enabled = false;
        rigid.simulated = false;
        spriter.sortingOrder = -1;

        GameManager.instance.kill++;
        GameObject expPoint = GameManager.instance.pool.Get(6);
        expPoint.transform.position = transform.position;
        expPoint.GetComponent<ExpPoint>().Init(exp);

        isDissolving = true;
        yield return new WaitForSeconds(1f);

        gameObject.SetActive(false);
    }

    IEnumerator MoveAndPauseRoutine()
    {
        isPaused = true;
        yield return new WaitForSeconds(2f);
        isPaused = false;
    }

    void Fire()
    {
        Vector3 dir = transform.position - GameManager.instance.player.transform.position;
        dir = -dir.normalized;

        Transform bullet = GameManager.instance.pool.Get(7).transform;
        bullet.position = transform.position;
        Quaternion additionalRotation = Quaternion.Euler(0, 0, 90f);
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir) * additionalRotation;
        bullet.GetComponent<EnemyBullet>().Init(atk, dir, 5);
    }

    void ModifyCollider(int EnemyID)
    {
        if (coll != null)
        {
            precoll = coll;
            precoll.enabled = false;
        }
        switch (EnemyID)
        {
            case 0: // Slime
                coll = gameObject.AddComponent<CircleCollider2D>();
                ((CircleCollider2D)coll).offset = new Vector2 (0.001502991f, -0.01352693f); 
                ((CircleCollider2D)coll).radius = 0.1336749f;
                break;
            case 1: // Trunk
                coll = gameObject.AddComponent<BoxCollider2D>();
                ((BoxCollider2D)coll).offset = new Vector2 (-0.006284118f, -0.03501152f); 
                ((BoxCollider2D)coll).size = new Vector2 (0.2108843f, 0.249977f);
                break;
            case 2: // Snail
                coll = gameObject.AddComponent<BoxCollider2D>();
                ((BoxCollider2D)coll).offset = new Vector2 (-0.01526147f, -0.01077273f); 
                ((BoxCollider2D)coll).size = new Vector2 (0.3114303f, 0.1971228f);
                break;
            case 3: // Mushroom
                coll = gameObject.AddComponent<BoxCollider2D>();
                ((BoxCollider2D)coll).offset = new Vector2 (-2.980232e-08f, -0.06104571f); 
                ((BoxCollider2D)coll).size = new Vector2 (0.2629529f, 0.2014995f);
                rigid.mass = 5;
                break;
            case 4: // Ghost
                coll = gameObject.AddComponent<BoxCollider2D>();
                ((BoxCollider2D)coll).offset = new Vector2 (0.005863339f, -0.01022977f); 
                ((BoxCollider2D)coll).size = new Vector2 (0.2512261f, 0.2562244f);
                break;
            case 5: // Rabbit
                coll = gameObject.AddComponent<BoxCollider2D>();
                ((BoxCollider2D)coll).offset = new Vector2 (0.01172674f, -0.04736447f); 
                ((BoxCollider2D)coll).size = new Vector2 (0.1808655f, 0.2405888f);
                break;
            case 6: // Bat
                coll = gameObject.AddComponent<BoxCollider2D>();
                ((BoxCollider2D)coll).offset = new Vector2 (0f, 0.026905f); 
                ((BoxCollider2D)coll).size = new Vector2 (0.4583988f, 0.1467748f);
                break;
        }
        if (precoll != null)
        {
            Destroy(precoll);
        }
    }

}

