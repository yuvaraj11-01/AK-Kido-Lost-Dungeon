using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject Menu, Leaderboard;
    private void Start()
    {
        //MetafabManager.GetCurrencyBalance();
    }
    public void OnPlayClicked()
    {
        SceneManager.LoadScene(2);
    }

    public void OnLeaderboardClicked()
    {
        Leaderboard.SetActive(true);
        Menu.SetActive(false);
    }

    public void OnCloseLeaderboard()
    {
        Leaderboard.SetActive(false);
        Menu.SetActive(true);
    }

    public void OnExitClicked()
    {
        Application.Quit();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnCloseLeaderboard();
        }
    }

}
