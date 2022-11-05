using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class TestBehaviour : UdonSharpBehaviour
{

    private VRCPlayerApi localPlayer;

    void Start()
    {
        this.localPlayer = Networking.LocalPlayer;
    }

    public void Update()
    {
        Debug.Log(
            this.localPlayer.GetVelocity()
        );

        this.localPlayer.SetGravityStrength();

    }

    // test line

}