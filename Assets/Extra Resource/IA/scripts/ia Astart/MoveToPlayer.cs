using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToPlayer : MonoBehaviour
{
    public float speed=10f;
    public Pathfinding Pf;
    public Rigidbody rb;
    // Update is called once per frame

    private void Start()
    {
        rb = this.GetComponent<Rigidbody>();
    }
    void Update()
    {

        float step = speed * Time.deltaTime; 
        try {
            transform.position = Vector3.MoveTowards(transform.position, Pf.EnemyPath[1].vPosition, step);
        
        
        }
        catch
        {
            return;
        }
    }
}
