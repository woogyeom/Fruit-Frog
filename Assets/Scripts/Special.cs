using UnityEngine;

public class Special : MonoBehaviour
{
    public int id;

    public void Init(ItemData data)
    {
        name = data.itemName;
        transform.parent = GameManager.instance.player.transform;
        transform.localPosition = Vector3.zero;

        id = data.itemId;
        ApplySpecial();
    }

    public void LevelUp()
    {
        ApplySpecial();
    }
    void ApplySpecial()
    {
        switch (id) {
            case 0:
                PowerUp();
                break;
            case 1:
                ProjectileUp();
                break;
            case 2:
                MaxHealthUp();
                break;
            case 3:
                MagnetUp();
                break;
        }
    }
    void PowerUp()
    {
        Weapon[] weapons = transform.parent.GetComponentsInChildren<Weapon>();
        foreach (Weapon weapon in weapons) {
            weapon.damage += 3;
        }
    }

    void ProjectileUp()
    {
        Weapon[] weapons = transform.parent.GetComponentsInChildren<Weapon>();

        foreach (Weapon weapon in weapons) {
            weapon.count += 1;
            if (weapon.GetComponent<Weapon>().type == ItemData.ItemType.Melee)
            {
                weapon.Place();
            }
        }
    }

    void MaxHealthUp()
    {
        GameManager.instance.maxHealth += 20;
        GameManager.instance.health = Mathf.Min(GameManager.instance.health + 30, GameManager.instance.maxHealth);
    }

    void MagnetUp()
    {
        GameManager.instance.magnetRange += 0.25f;
    }
}
