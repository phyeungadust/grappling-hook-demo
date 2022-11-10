using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class TestBehaviour : UdonSharpBehaviour
{

    void Start()
    {
        Debug.Log(Physics.gravity);
    }

    public void Update()
    {
    }

}