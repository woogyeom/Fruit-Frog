using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUD : MonoBehaviour
{
    public enum InfoType {Exp, Level, Kill, Time, Health}
    public InfoType type;

    TMP_Text myText;
    Slider mySlider;
    float lastVal;

    void Awake()
    {
        myText = GetComponent<TMP_Text>();
        mySlider = GetComponent<Slider>();

        lastVal = 0;
    }

    void LateUpdate()
    {
        if (!GameManager.instance.isLive)
        {
            //return;
        }
        switch (type) {
            case InfoType.Exp:
                float curExp = GameManager.instance.exp;
                float maxExp = GameManager.instance.nextEXP;
                mySlider.value = Mathf.Lerp(lastVal, curExp / maxExp, Time.deltaTime * 4);
                lastVal = mySlider.value;
                break;
            case InfoType.Level:
                if (GameManager.instance.level >= 38)
                {
                    myText.text = string.Format("Level: MAX");
                }
                else
                {
                    myText.text = string.Format("Level: {0:F0}", GameManager.instance.level);
                }
                break;
            case InfoType.Kill:
                myText.text = string.Format("Score: {0:F0}", GameManager.instance.kill);
                break;
            case InfoType.Time:
                int min = Mathf.FloorToInt(GameManager.instance.gameTime / 60);
                int sec = Mathf.FloorToInt(GameManager.instance.gameTime % 60);
                myText.text = string.Format("{0:D2}:{1:D2}", min, sec);
                break;
            case InfoType.Health:
                float curHealth = GameManager.instance.health;
                float maxHealth = GameManager.instance.maxHealth;
                mySlider.value = curHealth / maxHealth;
                break;
        }
    }
}
