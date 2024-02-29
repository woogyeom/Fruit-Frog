using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public Vector2 inputVec;
    public Vector2 lastInputVec;
    public Vector2 nextVec;
    Rigidbody2D rigid;
    public float speed;
    public Scanner scanner;
    SpriteRenderer spriter;
    Animator anim;
    Material material;
    public bool isDissolving = false;
    float fade = 1f;
    

    // Start is called before the first frame update
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        scanner = GetComponent<Scanner>();
        material = GetComponent<SpriteRenderer>().material; 
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Enemies"))
        {
            return;
        }
        
        anim.SetTrigger("Hit");
        if (collision.gameObject.GetComponent<Enemy>() != null)
        {
            GameManager.instance.health -= collision.gameObject.GetComponent<Enemy>().atk;
        }
        else
        {
            GameManager.instance.health -= collision.gameObject.GetComponent<Boss>().atk;
        }
        
        if (GameManager.instance.health < 1)
        {
            isDissolving = true;
            GameManager.instance.GameOver();
        }
    }

    void FixedUpdate()
    {
        if (!GameManager.instance.isLive)
        {
            return;
        }
        nextVec = speed * inputVec * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
    }

    void LateUpdate()
    {
        if (!GameManager.instance.isLive)
        {
            if (isDissolving)
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
            return;
        }
        anim.SetFloat("Speed", inputVec.magnitude);
        if (inputVec.x != 0) {
            spriter.flipX = inputVec.x < 0;
        }
    }

    void OnMove(InputValue value)
    {
        inputVec = value.Get<Vector2>();
        if (inputVec != Vector2.zero)
        {
            lastInputVec = inputVec;
        }
    }
}
