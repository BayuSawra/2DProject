using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatBar : MonoBehaviour
{
    private Character currentCharacter;
    public Image healthImage;
    public Image healthDelayImage;
    public Image powerImage;

    private bool isRecovering;


    private void Update()
    {
        if (healthDelayImage.fillAmount > healthImage.fillAmount)
        {
            healthDelayImage.fillAmount -= Time.deltaTime * 0.3f; //血量延迟减少速度
        }

        if (isRecovering)
        {
            float persentage = currentCharacter.currentPower / currentCharacter.maxPower;
            powerImage.fillAmount = persentage;

            if (persentage >= 1)
            {
                isRecovering = false;
                return;
            }
        }
    }
    public void OnHealthChange(float persentage)//血量变化是传入一个百分比值
    {
        healthImage.fillAmount = persentage;
    }
    
     public void OnPowerChange(Character character)
    {
        isRecovering = true;
        currentCharacter = character;
    }
   

}
