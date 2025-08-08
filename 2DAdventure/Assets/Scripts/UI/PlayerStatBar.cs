using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatBar : MonoBehaviour
{
    public Image healthImage;
    public Image healthDelayImage;
    public Image powerImage;


    private void Update()
    {
        if (healthDelayImage.fillAmount > healthImage.fillAmount)
        { 
            healthDelayImage.fillAmount -= Time.deltaTime * 0.3f; //血量延迟减少速度
        }
    }
    public void OnHealthChange(float persentage)//血量变化是传入一个百分比值
    {
        healthImage.fillAmount = persentage;
    }
   

}
