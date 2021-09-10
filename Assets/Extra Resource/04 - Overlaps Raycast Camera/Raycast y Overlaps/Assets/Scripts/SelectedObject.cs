using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedObject : MonoBehaviour
{
    public LayerMask myOverMask;
    private void Awake()
    {
        Camera.main.eventMask = myOverMask;
    }

    private void OnMouseDown()
    {
        Debug.Log("Mouse Down");
    }

    private void OnMouseEnter()
    {
        Debug.Log("Mouse Enter");
    }

    private void OnMouseExit()
    {
        Debug.Log("Mouse Exit");
    }

    private void OnMouseOver()
    {
        Debug.Log("Mouse Over");
    }
}
