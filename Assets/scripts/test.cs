using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class test : UdonSharpBehaviour
{

    [SerializeField]
    private ParticleSystem smoke;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            if (this.smoke.isPlaying)
            {
                this.smoke.Stop();
            }
            else
            {
                this.smoke.Play();
            }
        }
    }

}