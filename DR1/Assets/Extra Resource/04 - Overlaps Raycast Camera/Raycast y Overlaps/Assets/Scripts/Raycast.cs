using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycast : MonoBehaviour
{
    //Una layer me permite poder identificar objetos de acuerdo a un index
    //Una layermask me permite poder filtrar con que layers voy a estar interactuando
    public LayerMask mask1;
    public LayerMask mask2;

    public float rayDistance;

    private Vector3 _target;
    private List<Vector3> _targets = new List<Vector3>();

    bool hitted;

    void Start()
    {
        mask1 = new LayerMask();//creo una nueva layer

        //LayerMask.NameToLayer("layer1") me devuelve el indice de la layer
        mask1 += 1 << LayerMask.NameToLayer("layer1"); //indico que tenga en cuenta esta layer y ninguna otra
        mask1 += 1 << LayerMask.NameToLayer("layer2"); //puedo agregar mas layer a la layermask
        mask1 = ~mask1; //puedo invertir mi layermask

        mask2 = LayerMask.GetMask(new string[] { "layer1", "layer2" }); // creo una layer por medio de un array de strings
        mask2 -= 1 << LayerMask.NameToLayer("layer1"); //puedo remover una layer de mi layermask


        _target = transform.position + transform.forward * rayDistance;
    }

    void Update()
    {
        _target = transform.position + transform.forward * rayDistance;

        Ray ray = new Ray()
        {
            origin = transform.position,
            direction = transform.forward
        };

        /* Raycast normal
         * //con la mascara puedo indicar al rayo con que objetos interactuar
         * //el raycasthit devuelde informacion del objeto impactado
         * //se detiene ya sea si alcanzo su maxima distancia o impacto contra algo
        if (Physics.Raycast(ray,out RaycastHit hit,rayDistance, mask1))
        {
            _target = hit.point;
            hitted = true;
        }
        else
            hitted = false;*/

        //Otros tipos

        //float radius = 2.5f;

        //Esfera  
        //if (Physics.SphereCast(ray, radius, out RaycastHit hit, rayDistance, mask1))

        //Caja
        //if (Physics.BoxCast(transform.position, new Vector3(1,1,1), transform.forward , out RaycastHit hit, Quaternion.identity,rayDistance, mask1))

        //Capsula
        //if (Physics.CapsuleCast(transform.position + Vector3.up, transform.position+Vector3.down, radius, transform.forward,out RaycastHit hit, rayDistance, mask1)) 


        //Para obtener multiples hits

        //Devuelve todos los objetos con los que choca el rayo
        RaycastHit[] hits = Physics.RaycastAll(ray, rayDistance, mask1);

        if (hits.Length > 0)
        {
            _targets.Clear();
            _targets.Add(transform.position);
            for (int i = 0; i < hits.Length; i++)
            {
                _targets.Add(hits[i].transform.position);
            }
            _targets = OrderByDist(_targets);
            hitted = true;
        }
        else
            hitted = false;

        //Otras opciones
        //hits = (Physics.SphereCastAll(ray, radius, rayDistance, mask1));

        //hits = Physics.BoxCastAll(transform.position, new Vector3(1, 1, 1), transform.forward, Quaternion.identity, rayDistance, mask1);

        //hits = Physics.CapsuleCastAll(transform.position + Vector3.up, transform.position + Vector3.down, radius, transform.forward, rayDistance, mask1);

    }

    private List<Vector3> OrderByDist(List<Vector3> myList)
    {
        var auxList = myList;
        for (int i = 0; i < auxList.Count; i++)
        {
            for (int j = i; j < auxList.Count; j++)
            {
                var dist1 = Vector3.Distance(transform.position, auxList[i]);
                var dist2 = Vector3.Distance(transform.position, auxList[j]);
                if (dist2 < dist1)
                {
                    var aux = auxList[i];
                    auxList[i] = auxList[j];
                    auxList[j] = aux;
                }
            }
        }
        return auxList;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color =(hitted)? Color.red : Color.yellow;
        //Gizmos.DrawLine(transform.position, _target);

        for (int i = 0; i < _targets.Count-1; i++)
        {
            Gizmos.DrawLine(_targets[i], _targets[i + 1]);
        }
    }
}
