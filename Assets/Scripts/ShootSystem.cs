using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class ShootSystem : MonoBehaviour
{
    private Vector3 mousePressDownPos;
    private Vector3 mouseReleasePos;
    public float forceMultiplier;
    private Rigidbody rb;
    private bool isShoot;

    

    public float x_Force, y_Force;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
        
    }

    
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
    

        if(GameManager.isPlay)
        mousePressDownPos = Input.mousePosition;
    }

    private void OnMouseUp()
    {
        if (GameManager.isPlay)
        {
            mouseReleasePos = Input.mousePosition;
            Shoot(mouseReleasePos - mousePressDownPos);
        }
          
    }
    

    void Shoot(Vector3 Force)
    {
        Debug.Log(Force.magnitude);
        if(isShoot)
        {
            return;
        }
        Vector3 clone = new Vector3(100, 100, 100);
        if(Force.magnitude ==0)
            rb.AddForce(new Vector3(x_Force * clone.x, y_Force * clone.y, clone.y) * forceMultiplier);
        else
            rb.AddForce(new Vector3( x_Force* Force.x, y_Force * Force.y, Force.y) * forceMultiplier);


        gameObject.tag = "ball";

        isShoot = true;
    }
}
