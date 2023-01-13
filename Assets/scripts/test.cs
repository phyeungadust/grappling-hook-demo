using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class test : UdonSharpBehaviour
{
    void FixedUpdate()
    {
        this.transform.position = Vector3.MoveTowards(
            this.transform.position,
            this.transform.position + this.transform.forward * 99,
            10 * Time.fixedDeltaTime
        );
    }
}