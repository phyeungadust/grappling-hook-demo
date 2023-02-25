using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class WorldSpaceLogger : UdonSharpBehaviour
{

    [SerializeField]
    private int maxLineNum;
    // [SerializeField]
    // private  text;

}