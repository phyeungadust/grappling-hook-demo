using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class DoAfterSmokeAnimFinish : UdonSharpBehaviour
{

    private bool initiated = false;
    private ParticleSystem smoke;
    private Command cmd;

    public DoAfterSmokeAnimFinish Initiate(ParticleSystem smoke, Command cmd)
    {
        this.smoke = smoke;
        this.cmd = cmd;
        this.initiated = true;
        return this;
    }

    void Update()
    {
        if (this.initiated)
        {
            if (!this.smoke.isPlaying)
            {
                this.initiated = false;
                this.cmd.Exec();
            }
        }
    }

}