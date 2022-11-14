using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthHUD : MonoBehaviour
{
    [SerializeField] Image heart1, heart2;
    [SerializeField] Sprite Full, Half, Empty;

    public static HealthHUD instance;

    private void Awake()
    {
        instance = this;
    }

    public void SetHealth(int count)
    {
        switch (count)
        {
            case 0:
                heart1.sprite = Empty; heart2.sprite = Empty;
                break;
            case 1:
                heart1.sprite = Empty; heart2.sprite = Half;
                break;
            case 2:
                heart1.sprite = Empty; heart2.sprite = Full;
                break;
            case 3:
                heart1.sprite = Half; heart2.sprite = Full;
                break;
            case 4:
                heart1.sprite = Full; heart2.sprite = Full;
                break;
        }
    }

}
