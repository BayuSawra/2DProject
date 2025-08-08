using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatBar : MonoBehaviour
{
    public Image healthImage;
    public Image healthDelayImage;
    public Image powerImage;

    public void OnHealthChange(float persentage)//血量变化是传入一个百分比值
    {
        healthImage.fillAmount = persentage;
    }
   

}
