using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class WallShaderController : MonoBehaviour
{
    public Transform player;
    public Material mat;

    Vector3 myPos;

    private void Update()
    {
        myPos = player.position;
        mat.SetVector("_Player", myPos);
    }
}
