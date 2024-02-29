using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public ItemData.ItemType type;
    public int id;
    public int prefabId;
    public int damage;
    public int count;
    public int per;
    public float projspeed;
    public float atkspeed;
    float timer;
    Player player;

    void Awake()
    {
        player = GameManager.instance.player;
    }

    void Update()
    {
        if (!GameManager.instance.isLive)
        {
            return;
        }
        switch (type)
        {
            case ItemData.ItemType.Melee:
                transform.Rotate(Vector3.forward * projspeed * Time.deltaTime);
                break;

            case ItemData.ItemType.Ranged:
                timer += Time.deltaTime;
                if (timer > atkspeed)
                {
                    timer = 0f;
                    switch (id)
                    {
                        case 1:
                            Fire();
                            break;
                        case 2:
                            FireShotgun();
                            break;
                        default:
                            FireMultipleTimes(count);
                            break;
                    }
                }
                break;
            case ItemData.ItemType.Special:

                break;
            case ItemData.ItemType.Heal:

                break;
        }
    }


    public void LevelUp(ItemData.ItemType type, int damage, int count, int per)
    {
        if (type == this.type)
        {
            this.damage += damage;
            this.count += count;
            this.per += per;
        }
        if (type == ItemData.ItemType.Melee)
        {
            Place();
        }
    }

    public void Init(ItemData data)
    {
        // Basic
        name = data.itemName;
        transform.parent = player.transform;
        transform.localPosition = Vector3.zero;

        // Property
        type = data.itemType;
        id = data.itemId;
        damage = data.baseDamage;
        count = data.baseCount;
        per = data.basePer;
        projspeed = data.projspeed;
        atkspeed = data.atkspeed;

        for (int index = 0; index < GameManager.instance.pool.prefabs.Length; index++)
        {
            if (data.projectile == GameManager.instance.pool.prefabs[index])
            {
                prefabId = index;
                break;
            }
        }

        if (type == ItemData.ItemType.Melee)
        {
            Place();
        }

        player.BroadcastMessage("ApplySpecial", SendMessageOptions.DontRequireReceiver);
    }

    public void Place()
    {
        for (int index = 0; index < count; index++)
        {
            Transform bullet;
            if (index < transform.childCount)
            {
                bullet = transform.GetChild(index);
            }
            else {
                bullet = GameManager.instance.pool.Get(prefabId).transform;
                bullet.parent = transform;
            }

            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;

            Vector3 rotVec = Vector3.forward * 360 * index / count;
            bullet.Rotate(rotVec);
            bullet.Translate(bullet.up * 0.75f, Space.World);

            bullet.GetComponent<Bullet>().Init(damage, -100, Vector3.zero, 0);
        }
    }

    void FireMultipleTimes(int times)
    {
        if (id == 4)
        {
            StartCoroutine(FireWithInterval(times, 0.5f));
        }
        else
        {
            StartCoroutine(FireWithInterval(times, 0.1f));
        }
    }

    IEnumerator FireWithInterval(int times, float interval)
    {
        for (int i = 0; i < times; i++)
        {
            Fire();
            yield return new WaitForSeconds(interval);
        }
    }
    void Fire()
    {
        if (!player.scanner.nearestTarget)
        {
            return;
        }

        Vector3 targetPos = player.scanner.nearestTarget.position;
        Vector3 dir = targetPos - transform.position;
        dir = dir.normalized;

        Transform bullet = GameManager.instance.pool.Get(prefabId).transform;
        bullet.position = transform.position;
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        bullet.GetComponent<Bullet>().Init(damage, per, dir, projspeed);
    }

    void FireShotgun()
    {
        Vector2 lastInputVecNormalized = player.lastInputVec.normalized;

        if (player.lastInputVec != Vector2.zero)
        {
            for (int index = 0; index < count; index++)
            {
                Transform bullet = GameManager.instance.pool.Get(prefabId).transform;
                bullet.position = transform.position;

                float rotationAngle = Mathf.Atan2(lastInputVecNormalized.y, lastInputVecNormalized.x) * Mathf.Rad2Deg;
                rotationAngle += (index - (count / 2)) * (90 / count);

                Quaternion rotation = Quaternion.Euler(0, 0, rotationAngle);
                Vector3 dir = rotation * Vector3.right;

                bullet.rotation = rotation;
                bullet.GetComponent<Bullet>().Init(damage, per, dir, projspeed);
            }
        }
    }

}
