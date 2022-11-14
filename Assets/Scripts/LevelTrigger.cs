using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTrigger : MonoBehaviour
{
    [SerializeField] GameObject EnterInteraction;
    [SerializeField] string  SceneName;


    bool CanTrigger;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            EnterInteraction.SetActive(true);
            CanTrigger = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            EnterInteraction.SetActive(false);
            CanTrigger = false;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (CanTrigger)
            {
                SceneManager.LoadScene(SceneName);
            }
        }
    }

    public void release()
    {
        Movement.instance.canMove = true;
    }

}
