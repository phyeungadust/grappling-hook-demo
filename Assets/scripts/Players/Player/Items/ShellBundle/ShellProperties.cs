using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class ShellProperties : UdonSharpBehaviour
{
    public float Speed = 20.0f;
    public float StunTime = 3.0f;
}