using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Object/ItemData")]
public class ItemData : ScriptableObject
{
    public enum ItemType {Melee, Ranged, Special, Heal}

    [Header("# Main Info")]
    public ItemType itemType;
    public int itemId;
    public string itemName;
    [TextArea]
    public string itemDesc;
    public Sprite itemIcon;

    [Header("# Level Data")]
    public int baseDamage;
    public int baseCount;
    public int basePer;
    public int[] damages;
    public int[] counts;
    public int[] per;
    public float projspeed;
    public float atkspeed;

    [Header("# Weapon")]
    public GameObject projectile;
}
