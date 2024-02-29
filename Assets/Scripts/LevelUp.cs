using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUp : MonoBehaviour
{
    RectTransform rect;
    Item[] items;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        items = GetComponentsInChildren<Item>(true);
    }

    public void Show()
    {
        Next();
        rect.localScale = Vector3.one;
        GameManager.instance.Stop();
    }

    public void Hide()
    {
        foreach (Item item in items) {
            item.gameObject.SetActive(false);
        }
        rect.localScale = Vector3.zero;
        GameManager.instance.Resume();
    }

    public void Select(int index)
    {
       items[index].OnClick();
    }

    void Next()
    {
        foreach (Item item in items)
        {
            item.gameObject.SetActive(false);
        }

        int[] selectedIndices = RandomPick();

        foreach (int index in selectedIndices)
        {
            Item randItem = items[index];
            randItem.gameObject.SetActive(true);
        }
    }

    int[] RandomPick()
    {
        List<int> validIndices = new List<int>();
        List<float> weights = new List<float>();

        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].level < items[i].data.damages.Length || items[i].data.itemType == ItemData.ItemType.Heal)
            {
                validIndices.Add(i);
                
                float weight = 1f;
                if (items[i].data.itemType == ItemData.ItemType.Melee || items[i].data.itemType == ItemData.ItemType.Ranged)
                {
                    weight = 5f;
                }
                else if (items[i].data.itemType == ItemData.ItemType.Heal && GameManager.instance.health >= GameManager.instance.maxHealth)
                {
                    weight = 0f;
                }
                weights.Add(weight);
            }
        }

        if (validIndices.Count <= 3)
        {
            return validIndices.ToArray();
        }

        int[] selectedIndices = new int[3];
        for (int i = 0; i < 3; i++)
        {
            if (validIndices.Count == 0)
            {
                break;
            }

            (int selectedIndex, int weightIndex) = SelectIndexWithWeight(validIndices, weights);

            selectedIndices[i] = selectedIndex;

            validIndices.Remove(selectedIndex);
            weights.RemoveAt(weightIndex);
        }

        return selectedIndices;
    }

    (int, int) SelectIndexWithWeight(List<int> indices, List<float> weights)
    {
        float totalWeight = 0f;
        foreach (float weight in weights)
        {
            totalWeight += weight;
        }

        float randomValue = Random.Range(0f, totalWeight);

        float weightSum = 0f;
        for (int i = 0; i < weights.Count; i++)
        {
            weightSum += weights[i];
            if (randomValue <= weightSum)
            {
                int selectedIndex = indices[i];
                return (selectedIndex, i);
            }
        }

        throw new System.Exception("Index selection error.");
    }


}
