using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenPause : MonoBehaviour, IScreen
{
    Button[] buttons;

    string _result;

    private void Awake()
    {
        buttons = GetComponentsInChildren<Button>();

        foreach (var button in buttons)
        {
            button.interactable = false;
        }
    }

    public void BTN_Return()
    {
        _result = "Return";

        ScreenManag.Instance.Pop();
    }

  
    public void Activate()
    {
        foreach (var button in buttons)
        {
            button.interactable = true;
        }
    }

    public void Deactivate()
    {
        foreach (var button in buttons)
        {
            button.interactable = false;
        }
    }

    public string Free()
    {
        Destroy(gameObject);

        return _result;
    }
}
