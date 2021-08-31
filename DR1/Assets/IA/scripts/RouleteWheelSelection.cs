using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
//using UnityEditor.Animations;
using UnityEngine;

public class RouleteWheelSelection 
{
   public float velocity;

    public string Velocity(Dictionary<string, float> options)
    {     
        float total = 0;    
        foreach (var item in options)        
                 total += item.Value;
        
        //  Debug.Log("total");
        // Debug.Log(total);
        float random = Random.Range(0, total);
        // Debug.Log("random");
        //Debug.Log(random);

        foreach (var item in options)
        {
            random -= item.Value;
            if (random <= 0)
            {            
                //Debug.Log("item.Key");
                //Debug.Log(item.Key);
                if (item.Key == "speed1")                
                                  velocity = 10f;   
                if (item.Key == "speed2")                
                                  velocity = 20f;                
                if (item.Key == "speed3")
                                  velocity = 40f;    
                                         
             return item.Key;
            }
        }
     return "";
    }
}


