using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class VRPCManager : UdonSharpBehaviour
{

    public GameObject[] setActiveIfVR;
    public GameObject[] setActiveIfDesktop;
    public GameObject[] setActiveRegardless;

    public override void OnPlayerJoined(VRCPlayerApi player)
    {
        if (player.isLocal)
        {
            GameObject[] listToSetActive;
            if (Networking.LocalPlayer.IsUserInVR())
            {
                listToSetActive = setActiveIfVR;
            }
            else
            {
                listToSetActive = setActiveIfDesktop;
            }
            foreach (GameObject o in listToSetActive)
            {
                o.SetActive(true);
            }
            foreach (GameObject o in setActiveRegardless)
            {
                o.SetActive(true);
            }
        }
    }

}