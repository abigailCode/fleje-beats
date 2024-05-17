using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Score;

public class HandVibrationBehaviour : MonoBehaviour
{



    void OnEnable()
    {
        SlicingBehaviour.OnHandsVibrating += VibRight;
        SlicingBehaviour.OnHandsVibrating += VibLeft;
    }

    void OnDisable()
    {
        SlicingBehaviour.OnHandsVibrating -= VibRight;
        SlicingBehaviour.OnHandsVibrating -= VibLeft;
    }
    public void VibRight()
    {
        Debug.LogWarning("lkdjflkjsdflkerewlrjwlrj l,fdnlfghwer");
        Invoke("startVibRight", .1f);
        Invoke("stopVibRight", .4f);
    }
    public void startVibRight()
    {
        Debug.LogWarning("lkdjflkjsdflkerewlrj1111111111111111111111r");

        OVRInput.SetControllerVibration(1, 1, OVRInput.Controller.RTouch);
    }
    public void stopVibRight()
    {
        OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.RTouch);
    }

    public void VibLeft()
    {
        Invoke("startVibLeft", .1f);
        Invoke("stopVibLeft", .4f);
    }
    public void startVibLeft()
    {
        OVRInput.SetControllerVibration(1, 1, OVRInput.Controller.LTouch);
    }
    public void stopVibLeft()
    {
        OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.LTouch);
    }
}
