using System.Collections;
using UnityEngine;

public class Boss : MonoBehaviour
{
    Rigidbody2D rigid;
    Collider2D coll;
    SpriteRenderer spriter;
    Animator anim;
    Material material;
    WaitForFixedUpdate wait;
    public Rigidbody2D target;

    public float speed;
    public int health;
    public int maxHealth;
    public int atk;
    public int exp;

    bool isLive;
    bool isHit = false;
    bool isDissolving = false;
    float fade = 1f;


    public bool isCharging = false;
    private Vector2 chargeDirection;
    public float chargeTimer = 0f;
    public float chargeDuration;
    public float chargeRange; // 돌진 범위
    public float chargeSpeedMultiplier; // 돌진 속도 배수
    public bool isChargeCooldown = false;
    public float chargeCooldown;
    

    // Start is called before the first frame update
    void Awake()
    {
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        coll = GetComponent<Collider2D>();
        wait = new WaitForFixedUpdate();
        material = GetComponent<SpriteRenderer>().material;
    }

    private void OnEnable()
    {
        target = GameManager.instance.player.GetComponent<Rigidbody2D>();
        coll.enabled = true;
        isLive = true;
        health = maxHealth;
        rigid.simulated = true;
        spriter.sortingOrder = 0;
        fade = 1f;
        material.SetFloat("_Fade", fade);
    }
    
    public void Init()
    {

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
        if (!GameManager.instance.isLive || !isLive || isHit)
        {
            return;
        }
        
        Move();

        if (isCharging)
        {
            chargeTimer += Time.fixedDeltaTime;
        }
        if (isChargeCooldown)
        {
            chargeCooldown -= Time.fixedDeltaTime;
            if (chargeCooldown <= 0f)
            {
                isChargeCooldown = false;
                chargeCooldown = 10f;
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
        rigid.AddForce(dirVec.normalized * 2, ForceMode2D.Impulse);
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

    IEnumerator ChargeCooldown()
    {
        isChargeCooldown = true;
        yield return new WaitForSeconds(chargeCooldown);
        isChargeCooldown = false;
    }

    void Move()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, target.position);
        if (distanceToPlayer <= chargeRange && !isChargeCooldown && !isCharging)
        {
            isCharging = true;
            chargeDirection = (target.position - rigid.position).normalized;
        }
        if (isCharging)
        {
            if (chargeTimer < chargeDuration)
            {
                Vector2 nextVec = speed * chargeSpeedMultiplier * Time.fixedDeltaTime * chargeDirection;
                rigid.MovePosition(rigid.position + nextVec);
                rigid.velocity = Vector2.zero;
                anim.SetFloat("Speed", 1f);
            }
            else
            {
                isCharging = false;
                chargeTimer = 0f;
                anim.SetFloat("Speed", 0f);
                isChargeCooldown = true;
            }
        }
        else
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
    }
}

