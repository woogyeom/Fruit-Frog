using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class Danger : MonoBehaviour
{
    public TextMeshProUGUI warningText;
    public float blinkInterval;

    private bool isBlinking = false;

    public void Init()
    {
        warningText.enabled = true;
        StartCoroutine(ActivateBlinkingForThreeSeconds());
    }

    IEnumerator ActivateBlinkingForThreeSeconds()
    {
        StartBlinking();

        yield return new WaitForSeconds(3f);

        StopBlinking();
    }
    private IEnumerator BlinkText()
    {
        while (isBlinking)
        {
            warningText.enabled = !warningText.enabled;
            yield return new WaitForSeconds(blinkInterval);
        }
    }

    public void StartBlinking()
    {
        if (!isBlinking)
        {
            isBlinking = true;
            warningText.enabled = true;
            StartCoroutine(BlinkText());
        }
    }

    public void StopBlinking()
    {
        if (isBlinking)
        {
            isBlinking = false;
            warningText.enabled = false;
            StopCoroutine(BlinkText());
        }
    }
}
