using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class test : UdonSharpBehaviour
{

    void FixedUpdate()
    {
        this.transform.position += this.transform.forward * 15 * Time.fixedDeltaTime;
    }

}