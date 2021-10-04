using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class ObjetiveBox : MonoBehaviourPun, IInteractable
{
    public event Action OnGrab;
   
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, 50 * Time.deltaTime, 0));
    }
    private void OnTriggerEnter(Collider _player)
    {
        OnGrab();
        this.GetComponent<AudioSource>().Play();
        this.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
        Destroy(this.gameObject,2);
    }
}
