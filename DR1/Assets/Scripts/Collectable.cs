using UnityEngine;

public abstract class Collectable : MonoBehaviour
{
    //public GameObject sonido;
    public abstract void DarStats(ICollector collector);
    private void OnTriggerEnter(Collider other)
    {
        if(other is ICollector)
        {
            //COLLECTAR xD
        }
    }
}
