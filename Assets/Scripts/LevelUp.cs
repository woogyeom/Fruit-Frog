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
        // 모든 아이템을 비활성화
        foreach (Item item in items)
        {
            item.gameObject.SetActive(false);
        }

        int[] selectedIndices = RandomPick();

        // 선택된 아이템을 활성화
        foreach (int index in selectedIndices)
        {
            Item randItem = items[index];
            randItem.gameObject.SetActive(true);
        }
    }

    // ... (이 코드는 기존 코드를 기반으로 합니다.)

    int[] RandomPick()
    {
        List<int> validIndices = new List<int>();
        List<float> weights = new List<float>();

        // 유효한 아이템의 인덱스를 찾아서 리스트에 추가하고 가중치 할당
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].level < items[i].data.damages.Length || items[i].data.itemType == ItemData.ItemType.Heal)
            {
                // level이 damages의 길이보다 작은 경우에만 유효한 아이템으로 간주
                validIndices.Add(i);
                
                // 여기서 가중치를 할당하는 방법을 정의
                // 예를 들어, 가중치를 아이템의 레벨에 비례하게 할당할 수 있음
                float weight = 1f; // 기본 가중치
                if (items[i].data.itemType == ItemData.ItemType.Melee || items[i].data.itemType == ItemData.ItemType.Ranged)
                {
                    weight = 5f; // melee나 ranged 아이템의 경우 가중치를 5로 할당
                }
                else if (items[i].data.itemType == ItemData.ItemType.Heal && GameManager.instance.health >= GameManager.instance.maxHealth)
                {
                    weight = 0f;
                }
                weights.Add(weight);
                //Debug.Log(items[i].data.itemName + ' ' + weight);
            }
        }

        // 가중치가 0이 아닌 아이템이 3개 이하이면 그대로 반환
        if (validIndices.Count <= 3)
        {
            return validIndices.ToArray();
        }

        // 가중치에 따라 아이템 선택
        int[] selectedIndices = new int[3];
        for (int i = 0; i < 3; i++)
        {
            // 유효한 아이템이 없다면 중단
            if (validIndices.Count == 0)
            {
                break;
            }

            // 가중치를 기반으로 랜덤하게 하나 선택
            (int selectedIndex, int weightIndex) = SelectIndexWithWeight(validIndices, weights);

            // 선택된 아이템을 결과 배열에 추가
            selectedIndices[i] = selectedIndex;

            // 선택된 아이템을 제거하여 중복 선택 방지
            validIndices.Remove(selectedIndex);
            weights.RemoveAt(weightIndex);
        }

        return selectedIndices;
    }

    // 가중치를 기반으로 인덱스를 선택하는 함수
    (int, int) SelectIndexWithWeight(List<int> indices, List<float> weights)
    {
        // 가중치의 합을 구함
        float totalWeight = 0f;
        foreach (float weight in weights)
        {
            totalWeight += weight;
        }

        // 0에서 가중치의 합까지의 랜덤한 값 생성
        float randomValue = Random.Range(0f, totalWeight);

        // 누적 가중치를 기반으로 선택된 인덱스를 찾음
        float weightSum = 0f;
        for (int i = 0; i < weights.Count; i++)
        {
            weightSum += weights[i];
            if (randomValue <= weightSum)
            {
                int selectedIndex = indices[i];
                // 선택된 인덱스와 해당 인덱스의 가중치 리스트 내 인덱스를 함께 반환
                return (selectedIndex, i);
            }
        }

        // 여기까지 왔다면 오류가 있음
        // 이 부분은 실제로는 발생하지 않아야 함
        // 코드 검증을 위해 예외처리를 추가하는 것이 좋음
        throw new System.Exception("Index selection error.");
    }


}
