using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class ExecutorCommand : Command
{
    protected ResolveCommand resolveCommand;
    private Promise p;

    public ExecutorCommand Init(Promise p, ResolveCommand resolveCommand)
    {
        this.p = p;
        this.resolveCommand = resolveCommand;
        return this;
    }

    public override void Exec()
    {
        this.resolveCommand.Init(this.p, "hello world").Exec();
    }

}