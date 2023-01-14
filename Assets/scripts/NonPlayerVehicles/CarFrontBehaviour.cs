
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class CarFrontBehaviour : UdonSharpBehaviour
{

    public float pushFactor;

    public override void OnPlayerTriggerStay(VRCPlayerApi player)
    {

        // Debug.Log("collided");

        Vector3 acceleration = this.transform.forward * pushFactor;

        player.SetVelocity(
            player.GetVelocity() + acceleration * Time.fixedDeltaTime
        );
    }

    // public void OnCollisionEnter(Collision collision)
    // {

    //     Debug.Log("car front collided with " + collision);

    //     Vector3 acceleration = this.transform.forward * pushFactor;

    //     collision.rigidbody.velocity += acceleration * Time.fixedDeltaTime;

    // }

    public void OnTriggerEnter(Collider collider)
    {

        // Debug.Log("car front collided with " + collider);

        Vector3 acceleration = this.transform.forward * pushFactor;

        Rigidbody rb = collider.GetComponent<Rigidbody>();

        if (rb != null)
        {
            collider.GetComponent<Rigidbody>().velocity += acceleration * Time.fixedDeltaTime;
        }

    }

}