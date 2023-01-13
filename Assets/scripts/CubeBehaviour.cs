using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class CubeBehaviour : UdonSharpBehaviour
{

    public Rigidbody rb;
    public float speed;

    void Start()
    {
        
    }

    void FixedUpdate()
    {
        this.rb.AddForce(this.transform.forward * speed);
    }

}
