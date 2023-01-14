using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class CarMoveBehaviour : UdonSharpBehaviour
{

    public Rigidbody rb;
    public float speed;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void FixedUpdate()
    {

        // this.rb.MovePosition(
        //     this.transform.position
        //     + this.transform.forward * speed * Time.fixedDeltaTime
        // );

        this.rb.velocity = this.transform.forward * speed;

    }

}
