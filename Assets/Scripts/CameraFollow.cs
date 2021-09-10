using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform follow;
    public Vector3 minCamPos, maxCamPos;
    float smoothTimeX, smoothTimeY, smoothTimeZ;
    public float smoothTimeShootX, smoothTimeShootY, smoothTimeShootZ;
    public float smoothTimeTurnX, smoothTimeTurnY, smoothTimeTurnZ;
    private Vector3 velocity;

    void LateUpdate()
    {
        if (follow != null)
        {
            if (follow.tag == "Player")
            {
                smoothTimeX = smoothTimeTurnX;
                smoothTimeY = smoothTimeTurnY;
                smoothTimeZ = smoothTimeTurnZ;
            }
            else
            {
                smoothTimeX = smoothTimeShootX;
                smoothTimeY = smoothTimeShootY;
                smoothTimeZ = smoothTimeShootZ;
            }

            float posX = Mathf.SmoothDamp(this.transform.position.x, follow.transform.position.x, ref velocity.x, smoothTimeX);
            float posY = Mathf.SmoothDamp(this.transform.position.y, follow.transform.position.y, ref velocity.y, smoothTimeY);
            float posZ = Mathf.SmoothDamp(this.transform.position.z, follow.transform.position.z, ref velocity.z, smoothTimeZ);

            try
            {
                this.gameObject.transform.position = new Vector3(Mathf.Clamp(posX, minCamPos.x, maxCamPos.x), Mathf.Clamp(posY, minCamPos.y, maxCamPos.y), Mathf.Clamp(posZ, minCamPos.z, maxCamPos.z));
            }
            catch { }
        }
    }

    internal void SetFollow(GameObject follow)
    {
        this.follow = follow.transform;
    }
}

