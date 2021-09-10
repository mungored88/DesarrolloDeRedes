using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DefeatScreen : MonoBehaviour
{
    public Text timeText;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Countdown());
    }


    IEnumerator Countdown()
    {
        float ticks = 0;

        while (ticks <= 3)
        {
            ticks += Time.deltaTime;
            timeText.text = (3-Mathf.Floor(ticks)).ToString();

            yield return null;
        }

        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
    
}
