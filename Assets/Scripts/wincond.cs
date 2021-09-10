using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class wincond : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
       SceneManager.LoadScene("win");
    }
}
