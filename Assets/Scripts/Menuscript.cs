using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menuscript : MonoBehaviour
{
    public GameObject menu;

    // Update is called once per frame
    void Update()
    {
       //if (Input.GetKeyDown(KeyCode.Escape)) menu.SetActive(true);
    }
    public void ClickInBoton(string scene)
    {
        SceneManager.LoadScene(scene);
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }
    public void Exit()
    {
        Application.Quit();
    }
}
