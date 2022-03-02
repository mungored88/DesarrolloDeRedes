using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToPlayer : MonoBehaviour
{
    public float speed=10f;
    public Pathfinding Pf;
    public Rigidbody rb;
    // Update is called once per frame
    
    private Animator m_Animator;
    private static readonly int Forward = Animator.StringToHash("Forward");

    private void Awake()
    {
        m_Animator = GetComponent<Animator>();
    }

    private void Start()
    {
        rb = this.GetComponent<Rigidbody>();
    }
    void Update()
    {

        float step = speed * Time.deltaTime; 
        try {
            transform.position = Vector3.MoveTowards(transform.position, Pf.EnemyPath[1].vPosition, step);
            transform.rotation = Quaternion.Slerp(transform.rotation,
                Quaternion.LookRotation(Pf.EnemyPath[1].vPosition - transform.position), 10 * Time.deltaTime);
            // Animator Update
            m_Animator.SetFloat(Forward, step, 0.1f, Time.deltaTime);
        }
        catch
        {
            return;
        }
    }
}
