using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{

    public Collider2D coll;
    void Awake()
    {
        coll = GetComponent<Collider2D>();
    }

    void Update()
    {
        coll = GetComponent<Collider2D>();
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision == null || !collision.CompareTag("Area"))
            return;
        
        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 myPos = transform.position;

        float diffX = playerPos.x - myPos.x;
        float diffY = playerPos.y - myPos.y;
        float dirX = diffX > 0 ? 1 : -1;
        float dirY = diffY > 0 ? 1 : -1;

        diffX = Mathf.Abs(diffX);
        diffY = Mathf.Abs(diffY);

        switch (transform.tag) {
            case "Ground":
                if (diffX > diffY) {
                    transform.Translate(Vector3.right * dirX * 56);
                } else if (diffX < diffY) {
                    transform.Translate(Vector3.up * dirY * 56);
                } else {
                    transform.Translate(dirX * 56, dirY * 56, 0);
                }
                break;
            case "Enemies":
                if (coll.enabled) {
                        Vector3 dist = playerPos - myPos;
                        float rand = Random.Range(1.8f, 2.0f);
                        transform.Translate(dist * rand);
                }
                break;
            case "Items":
                if (coll.enabled) {
                    Vector3 dist = playerPos - myPos;
                    float rand = Random.Range(1.8f, 2.0f);
                    transform.Translate(dist * rand);
                }
                break;
        }
    }
}
