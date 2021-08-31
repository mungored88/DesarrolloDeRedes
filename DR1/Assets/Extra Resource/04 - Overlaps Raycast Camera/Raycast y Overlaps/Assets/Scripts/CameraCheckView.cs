using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCheckView : MonoBehaviour
{
    public Transform target;

    void Update()
    {
        var dir = target.position - transform.position;

        transform.forward = dir;
        var pos = target.position - dir.normalized * 10;

        if (Physics.Raycast(transform.position, dir.normalized, out RaycastHit hit, dir.magnitude))
        {
            if (hit.transform != target)
                pos = hit.point;
        }

        transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime * 20);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(Camera.main.transform.position, target.position);
    }
}
