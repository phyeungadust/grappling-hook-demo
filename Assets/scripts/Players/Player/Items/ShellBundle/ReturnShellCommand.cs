using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class ReturnShellCommand : Command
{

    private ShellBehaviour shellBehaviour;
    private Command cmd;
    public ReturnShellCommand Init(
        ShellBehaviour shellBehaviour, 
        Command cmd = null
    )
    {
        this.shellBehaviour = shellBehaviour;
        this.cmd = cmd;
        return this;
    }

    public override void Exec()
    {
        this.shellBehaviour.ReturnShell();
        if (this.cmd != null)
        {
            this.cmd.Exec();
        }
    }

}