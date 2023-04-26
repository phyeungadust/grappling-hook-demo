using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using TMPro;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class WorldSpaceLogger : UdonSharpBehaviour
{

    [SerializeField]
    private int maxLineNum;
    [SerializeField]
    private TextMeshProUGUI tmPro;

    private string[] loggedLineBuffer;
    private int head = -1;
    private int tail = -1;
    private int numOfBufferedLines = 0;

    void Start()
    {
        this.loggedLineBuffer = new string[this.maxLineNum];
    }

    private void PrintBufferedLines()
    {
        this.tmPro.text = "";
        int ptr = tail;
        int printedLines = 0;
        while (printedLines < this.numOfBufferedLines)
        {
            this.tmPro.text += this.loggedLineBuffer[ptr];
            if (printedLines < this.numOfBufferedLines - 1)
            {
                this.tmPro.text += "\n";
            }
            ++printedLines;
            ptr = (ptr + 1) % this.maxLineNum;
        }
    }

    public void Log(string line)
    {
        Debug.Log(line);
        this.head = (this.head + 1) % this.maxLineNum;
        this.loggedLineBuffer[this.head] = line;
        if (this.numOfBufferedLines == this.maxLineNum)
        {
            this.tail = (this.tail + 1) % this.maxLineNum;
        }
        else
        {
            if (this.numOfBufferedLines == 0)
            {
                this.tail = 0;
            }
            ++this.numOfBufferedLines;
        }
        this.PrintBufferedLines();
    }

}