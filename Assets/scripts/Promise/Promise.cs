using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class Promise : UdonSharpBehaviour
{

    public object[] wrappedValues;
    public ObjectsCommand[] commands;
    public string state = "PENDING";
    public int ptr = 0;
    
    public ResolveCommand resolveCommand;

    public static void Test()
    {
        Promise p = new Promise();
        ResolveCommand rc = new ResolveCommand();
        p.Init(new ExecutorCommand().Init(p, rc));
    }

    public Promise Init(ExecutorCommand executor)
    {
        this.commands = new ObjectsCommand[10];
        this.ptr = 0;
        executor.Exec();
        return this;
    }

    public Promise Then(ObjectsCommand objectsCommand)
    {
        if (this.state == "FULFILLED")
        {
            objectsCommand.Init(wrappedValues).Exec();
        }
        else
        {
            if (this.ptr < this.commands.Length)
            {
                this.commands[ptr] = objectsCommand;
                ++this.ptr;
            }
        }
        return this;
    }

}