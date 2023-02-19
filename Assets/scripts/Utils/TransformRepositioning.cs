using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class TransformRepositioning : UdonSharpBehaviour
{

    [SerializeField]
    private Transform target;

    [SerializeField]
    private InputField xPosField;
    [SerializeField]
    private InputField yPosField;
    [SerializeField]
    private InputField zPosField;
    [SerializeField]
    private InputField xRotField;
    [SerializeField]
    private InputField yRotField;
    [SerializeField]
    private InputField zRotField;

    void Start()
    {
        this.xPosField.text = this.target.localPosition.x.ToString();
        this.yPosField.text = this.target.localPosition.y.ToString();
        this.zPosField.text = this.target.localPosition.z.ToString();
        this.xRotField.text = this.target.localEulerAngles.x.ToString();
        this.yRotField.text = this.target.localEulerAngles.y.ToString();
        this.zRotField.text = this.target.localEulerAngles.z.ToString();
    }

    public void SetXPos()
    {
        if (float.TryParse(this.xPosField.text, out float result))
        {
            this.target.localPosition = new Vector3(
                result,
                this.target.localPosition.y,
                this.target.localPosition.z
            );
        }
    }

    public void SetYPos()
    {
        if (float.TryParse(this.yPosField.text, out float result))
        {
            this.target.localPosition = new Vector3(
                this.target.localPosition.x,
                result,
                this.target.localPosition.z
            );
        }
    }
    public void SetZPos()
    {
        if (float.TryParse(this.zPosField.text, out float result))
        {
            this.target.localPosition = new Vector3(
                this.target.localPosition.x,
                this.target.localPosition.y,
                result
            );
        }
    }
    public void SetXRot()
    {
        if (float.TryParse(this.xRotField.text, out float result))
        {
            this.target.localEulerAngles = new Vector3(
                result,
                this.target.localEulerAngles.y,
                this.target.localEulerAngles.z
            );
        }
    }
    public void SetYRot()
    {
        if (float.TryParse(this.yRotField.text, out float result))
        {
            this.target.localEulerAngles = new Vector3(
                this.target.localEulerAngles.x,
                result,
                this.target.localEulerAngles.z
            );
        }
    }
    public void SetZRot()
    {
        if (float.TryParse(this.zRotField.text, out float result))
        {
            this.target.localEulerAngles = new Vector3(
                this.target.localEulerAngles.x,
                this.target.localEulerAngles.y,
                result
            );
        }
    }
}