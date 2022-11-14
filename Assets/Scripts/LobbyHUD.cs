using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyHUD : MonoBehaviour
{
    [Header("Life")]
    [SerializeField] TMPro.TMP_Text coinValue;

    public static LobbyHUD instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        refresh();
    }
    public void refresh()
    {
        coinValue.text = MetafabManager.GetCurrencyBalance();

    }
}
