using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    [SerializeField] private GameObject marble;
    [SerializeField] private GameObject crosshair;

    void Update(){
        if (marble != null && crosshair != null){
            //transform.position = marble.transform.position - crosshair.transform.position;
            transform.position = BetweenPoints(marble.transform.position, crosshair.transform.position, 0.5f);
        }
    }

    void OnDrawGizmos(){
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }

    private Vector3 BetweenPoints(Vector3 v1, Vector3 v2, float percentage)
    {
        return (v2 - v1) * percentage + v1;
    }
}
