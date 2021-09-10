using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Grenade : MonoBehaviour
{

    public float range = 30f;

    public float force;
    public float explotionTime = 3f;
    public LayerMask mask;

    public float explosionDistance = 500;
    
    public FX explotionEffect;

    public GrenadeType grenadeType;
    protected abstract void Explode();

    public Grenade setSpawnPosition(Transform pos)
    {
        this.transform.position = pos.position;
        this.transform.rotation = pos.rotation;
        return this;
    }
    public void Launch(Vector3 playerVelocity)
    {
        this.GetComponent<Rigidbody>().AddForce((this.transform.forward * range) + playerVelocity, ForceMode.Impulse);
        Invoke("Explode", explotionTime);
    }

    public static void TurnOn(Grenade nade)
    {
        nade.gameObject.SetActive(true);
    }

    public static void TurnOff(Grenade nade)
    {
        nade.gameObject.SetActive(false);
    }

    public enum GrenadeType
    {
        FRAG_NADE,
    }

}


///TO:DO_ VER COMO MOSTRAR LINEA VISUAL DE GRANADA
//public Rigidbody rb;
//public GameObject cursor;
//public LayerMask layer;
//public Transform shootPoint;
//public LineRenderer lineVisual;
//public int linesSegment = 10;

//public Camera cam;
//// Start is called before the first frame update
//void Start()
//{
//    lineVisual.positionCount = linesSegment;
//}

//// Update is called once per frame
//void Update()
//{
//    LaunchProyectil();
//}
//void LaunchProyectil()
//{
//    Ray camRay = cam.ScreenPointToRay(Input.mousePosition);
//    RaycastHit hit;

//    if(Physics.Raycast(camRay, out hit, 100f, layer))
//    {
//        cursor.SetActive(true);
//        cursor.transform.position = hit.point + Vector3.up * 0.1f;

//        Vector3 vo = CalculateVelocity(hit.point, shootPoint.position, 1f);

//        Visualize(vo);

//        //transform.rotation = Quaternion.LookRotation(vo);

//        if (Input.GetKeyDown(KeyCode.G)) {

//            Rigidbody obj = Instantiate(rb, shootPoint.position, Quaternion.identity);
//            obj.velocity = vo;
//        }
//    }
//}

//Vector3 CalculateVelocity(Vector3 target, Vector3 origin, float time)
//{

//    Vector3 distance = target - origin;
//    Vector3 distanceXz = distance;
//    distanceXz.y = 0;


//    float sY = distance.y;
//    float sXz = distanceXz.magnitude;

//    float Vxz = sXz * time;
//    float Vy = (sY / time) + (0.5f * Mathf.Abs(Physics.gravity.y) * time);

//    Vector3 result = distanceXz.normalized;
//    result *= Vxz;
//    result.y = Vy;

//    return result;

//}


//void Visualize(Vector3 vo) 
//{ 
//    for(int i = 0; i<linesSegment; i++)
//    {
//        Vector3 pos = CalculatePositionIntTime(vo, i / (float)linesSegment);
//        lineVisual.SetPosition(i, pos);
//    }
//}
//Vector3 CalculatePositionIntTime(Vector3 vo, float time) {

//    Vector3 Vxz = vo;
//    vo.y = 0f;

//    Vector3 result = shootPoint.position + vo * time;
//    float sY = (-0.5f * Mathf.Abs(Physics.gravity.y) * (time * time)) + (vo.y * time) + shootPoint.position.y;

//    result.y = sY;
//    return result;
//}