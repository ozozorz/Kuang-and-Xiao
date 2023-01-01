using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public Animator anim;
    public GameObject clickBegin;

    public void onClickBegin()
    {
        anim.enabled = true;
        Destroy(clickBegin);
    }

    public void NewGame()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene("StartingVillage");
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("StartingVillage");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
