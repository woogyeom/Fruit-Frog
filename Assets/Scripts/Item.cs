using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public ItemData data;
    public int level;
    public Weapon weapon;
    public Special special;
    Image icon;
    TMP_Text textLevel;
    TMP_Text textName;
    TMP_Text textDesc;

    void Awake()
    {
        icon = GetComponentsInChildren<Image>()[1];
        icon.sprite = data.itemIcon;

        TMP_Text[] texts = GetComponentsInChildren<TMP_Text>();
        textLevel = texts[0];
        textName = texts[1];
        textDesc = texts[2];
        textName.text = data.itemName;
    }
    void OnEnable()
    {
        textLevel.text = "Lv. " + (level + 1);
        if (level == 0)
        {
            switch (data.itemType) {
            case ItemData.ItemType.Melee:
            case ItemData.ItemType.Ranged:
                if (data.itemId == 1 && data.itemType == ItemData.ItemType.Ranged)
                {
                    textDesc.text = string.Format(data.itemDesc, data.baseDamage, data.basePer);
                }
                else
                {
                    textDesc.text = string.Format(data.itemDesc, data.baseDamage, data.baseCount);
                }
                break;
            case ItemData.ItemType.Special:
                if (data.itemId == 0) {
                    textDesc.text = string.Format(data.itemDesc, data.baseDamage);
                }
                else if (data.itemId == 1)
                {
                    textDesc.text = string.Format(data.itemDesc, data.baseCount);
                }
                else
                {
                    textDesc.text = string.Format(data.itemDesc);
                }
                break;
            case ItemData.ItemType.Heal:
                textDesc.text = string.Format(data.itemDesc);
                break;
            }
        }
        else
        {
            switch (data.itemType) {
            case ItemData.ItemType.Melee:
            case ItemData.ItemType.Ranged:
                if (data.itemId == 1 && data.itemType == ItemData.ItemType.Ranged)
                {
                    textDesc.text = string.Format(data.itemDesc, data.damages[level], data.per[level]);
                }
                else
                {
                    textDesc.text = string.Format(data.itemDesc, data.damages[level], data.counts[level]);
                }
                break;
            case ItemData.ItemType.Special:
                if (data.itemId == 0) {
                    textDesc.text = string.Format(data.itemDesc, data.damages[level]);
                }
                else if (data.itemId == 1)
                {
                    textDesc.text = string.Format(data.itemDesc, data.counts[level]);
                }
                else
                {
                    textDesc.text = string.Format(data.itemDesc);
                }
                break;
            case ItemData.ItemType.Heal:
                textDesc.text = string.Format(data.itemDesc);
                break;
            }
        }
    }

    public void OnClick()
    {
        switch(data.itemType)
        {
            case ItemData.ItemType.Melee:
            case ItemData.ItemType.Ranged:
                if (level == 0) {
                    GameObject newWeapon = new GameObject();
                    weapon = newWeapon.AddComponent<Weapon>();
                    weapon.Init(data);
                }
                else
                {
                    int nextDamage = 0;
                    int nextCount = 0;
                    int nextPer = 0;

                    nextDamage += data.damages[level];
                    if (level < data.counts.Length)
                    nextCount += data.counts[level];
                    if (level < data.per.Length)
                    nextPer += data.per[level];

                    weapon.LevelUp(data.itemType, nextDamage, nextCount, nextPer);
                }
                break;
            case ItemData.ItemType.Special:
                if (level == 0) {
                    GameObject newSpecial = new GameObject();
                    special = newSpecial.AddComponent<Special>();
                    special.Init(data);
                }
                else
                {
                    special.LevelUp();
                }
                break;
            case ItemData.ItemType.Heal:
                GameManager.instance.health = Mathf.Min(GameManager.instance.health + GameManager.instance.maxHealth / 2, GameManager.instance.maxHealth);
                break;
        }
        level++;

        if (level == data.damages.Length + 1 && data.itemType != ItemData.ItemType.Heal) {
            GetComponent<Button>().interactable = false;
        }
    }
}
