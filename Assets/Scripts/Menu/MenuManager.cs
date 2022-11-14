using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    private void Start()
    {
        MetafabManager.GetCurrencyBalance();
    }
    public void OnPlayClicked()
    {
        SceneManager.LoadScene(2);
    }

    public void OnOptionsClicked()
    {
        Debug.Log("On Option");
    }

    public void OnExitClicked()
    {
        Application.Quit();
    }

}
