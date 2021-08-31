using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Overlaps : MonoBehaviour
{
    public float force;
    public float explosionDistance;
    public LayerMask mask;

    void Update()
    {
        //Overlaps
        //Dependiendo de que forma se utilice se necesitara pasar diferentes parametros
        //Sirve para obtener todos los objectos con colliders que se encuentren en el rango dado
        //Se puede filtrar a travez de layers los objetos con los que valla a interactuar

        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Sphere me permite generar un volumen de tipo esfera
            Collider[] collection = Physics.OverlapSphere(transform.position, explosionDistance, mask);

            foreach (var item in collection)
            {
                if (item.gameObject.layer == LayerMask.NameToLayer("Box"))
                    Destroy(item.gameObject);

                if(item.gameObject.layer == LayerMask.NameToLayer("MetalBox"))
                {
                    var rb = item.GetComponent<Rigidbody>();

                    if (rb != null)
                    {
                        var distace = (item.transform.position - transform.position);
                        rb.AddForce((distace.normalized / distace.magnitude) * force, ForceMode.Impulse);
                    }
                }
            }
        }

        //Otros tipos

        //Physics.OverlapCapsule();
        //Physics.OverlapBox();
    }
}
