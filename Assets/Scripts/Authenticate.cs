using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Org.MetaFab.Client;

using UnityEngine.UI;

public class Authenticate : MonoBehaviour
{
    const string UsernameKey = "Username";
    const string PasswordKey = "Password";

    [Header("UI elements")]
    [SerializeField] InputField UserName_field;
    [SerializeField] InputField Password_field;
    [SerializeField] Button Login_SignUp;

    private void Start()
    {
        UserName_field.text = PlayerPrefs.GetString(UsernameKey);
        Password_field.text = PlayerPrefs.GetString(PasswordKey);
        Login_SignUp.onClick.AddListener(OnLogin_SignupClicked);
    }

    void OnLogin_SignupClicked()
    {
        Login_SignUp.interactable = false;
        string temp_username = UserName_field.text;
        string temp_password = Password_field.text;
        MetafabManager.LoginPlayer(temp_username, temp_password, (res) =>
        {
            if (!res)
            {
                Login_SignUp.interactable = true;
            }
            else
            {
                PlayerPrefs.SetString(UsernameKey, Configuration.Default.Username);
                PlayerPrefs.SetString(PasswordKey, Configuration.Default.Password);
                // load next scene
                SceneManager.LoadScene(1);
            }
        });
    }

}
