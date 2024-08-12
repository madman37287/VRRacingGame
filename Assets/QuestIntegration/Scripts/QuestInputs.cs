using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.XR;

public class QuestInputs : MonoBehaviour
{
    public Transform RightController;           //Transform of right controller
    public Transform LeftController;            //Transform of left controller
    public static QuestControllerInput Right;   //Controller variables
    public static QuestControllerInput Left;    //Controller variables

    //UI elements for debugging 
    string s;
    public TMP_Text UI_Text;

    List<InputDevice> leftHandControllers = new List<InputDevice>();
    List<InputDevice> rightHandControllers = new List<InputDevice>();

    void OnEnable()
    {
        //Getting current devices
        List<InputDevice> allDevices = new List<InputDevice>();
        InputDevices.GetDevices(allDevices);
        foreach (InputDevice device in allDevices) InputDevices_deviceConnected(device);

        //In case new devices are added
        InputDevices.deviceConnected += InputDevices_deviceConnected;
        InputDevices.deviceDisconnected += InputDevices_deviceDisconnected;
    }

    private void OnDisable()
    {   
        //Removing devices
        InputDevices.deviceConnected -= InputDevices_deviceConnected;
        InputDevices.deviceDisconnected -= InputDevices_deviceDisconnected;
    }

    private void InputDevices_deviceConnected(InputDevice device)
    {
        //Getting left controller
        InputDeviceCharacteristics leftControllerCharacteristics = InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Controller 
                                                                                                               | InputDeviceCharacteristics.Left;
        if ((device.characteristics & leftControllerCharacteristics) == leftControllerCharacteristics)
        {
            leftHandControllers.Add(device);
            Left.SetDevice(device);
        }

        //Getting right controller
        InputDeviceCharacteristics rightControllerCharacteristics = InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Controller 
                                                                                                               | InputDeviceCharacteristics.Right;
        if ((device.characteristics & rightControllerCharacteristics) == rightControllerCharacteristics)
        {
            rightHandControllers.Add(device);
            Right.SetDevice(device);
        }
    }

    private void InputDevices_deviceDisconnected(InputDevice device)
    {
        //Removing left controller
        InputDeviceCharacteristics leftControllerCharacteristics = InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Controller   
                                                                                                               | InputDeviceCharacteristics.Left;
        if ((device.characteristics & leftControllerCharacteristics) == leftControllerCharacteristics)
        {
            leftHandControllers.Remove(device);
            Left = new QuestControllerInput();
        }

        //Removing right controller
        InputDeviceCharacteristics rightControllerCharacteristics = InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Controller 
                                                                                                               | InputDeviceCharacteristics.Right;
        if ((device.characteristics & rightControllerCharacteristics) == rightControllerCharacteristics)
        {
            rightHandControllers.Remove(device);
            Right = new QuestControllerInput();
        }
    }


    void Update()
    {
        bool outTrigger, outGrip, outButton1, outButton2, outThumbStickClick;
        Vector2 outThumbStick;

        //We are collecting the input values of the left controller and sending them to the SetInput method in the QuestControllerInput struct
        if (leftHandControllers.Count > 0)
        {
            leftHandControllers[0].TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out outTrigger);
            leftHandControllers[0].TryGetFeatureValue(UnityEngine.XR.CommonUsages.gripButton, out outGrip);
            leftHandControllers[0].TryGetFeatureValue(UnityEngine.XR.CommonUsages.primaryButton, out outButton1);
            leftHandControllers[0].TryGetFeatureValue(UnityEngine.XR.CommonUsages.secondaryButton, out outButton2);
            leftHandControllers[0].TryGetFeatureValue(UnityEngine.XR.CommonUsages.primary2DAxis, out outThumbStick);
            leftHandControllers[0].TryGetFeatureValue(UnityEngine.XR.CommonUsages.primary2DAxisClick, out outThumbStickClick);
            Left.SetInput(outTrigger, outGrip, outButton1, outButton2, outThumbStick.x, outThumbStick.y, outThumbStickClick);
        }

        //We are collecting the input values of the right controller and sending them to the SetInput method in the QuestControllerInput struct
        if (rightHandControllers.Count > 0)
        {
            rightHandControllers[0].TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out outTrigger);
            rightHandControllers[0].TryGetFeatureValue(UnityEngine.XR.CommonUsages.gripButton, out outGrip);
            rightHandControllers[0].TryGetFeatureValue(UnityEngine.XR.CommonUsages.primaryButton, out outButton1);
            rightHandControllers[0].TryGetFeatureValue(UnityEngine.XR.CommonUsages.secondaryButton, out outButton2);
            rightHandControllers[0].TryGetFeatureValue(UnityEngine.XR.CommonUsages.primary2DAxis, out outThumbStick);
            rightHandControllers[0].TryGetFeatureValue(UnityEngine.XR.CommonUsages.primary2DAxisClick, out outThumbStickClick);
            Right.SetInput(outTrigger, outGrip, outButton1, outButton2, outThumbStick.x, outThumbStick.y, outThumbStickClick);
        }

        //We are collecting the position and rotation values of the controllers and sending them to the SetOrientation method
        //in the QuestControllerInput struct
        Left.SetOrientation(LeftController.position, LeftController.rotation);
        Right.SetOrientation(RightController.position, RightController.rotation);

        //We are displaying the controller button values in the game world for real time observation
        if (UI_Text != null)
        {
            s = "Inputs:\n\n";
            s += "Right:\n";
            s += Right.Debug() + "\n";

            s += "Left:\n";
            s += Left.Debug() + "\n";
            UI_Text.text = s;
        }
    }
}


//Keeps controller button variables
public struct QuestControllerInput
{
    public QuestControllerButton Trigger;           //Trigger button
    public QuestControllerButton Grip;              //Grip button
    public QuestControllerButton Button1;           //A/X Button
    public QuestControllerButton Button2;           //B/Y Button
    public Vector2 ThumbStick;                      //Thumbstick
    public QuestControllerButton ThumbStickClick;   //Thumbstick click button
    public Vector3 Position;                        //Controller position
    public Quaternion Rotation;                     //Controller rotation
    
    InputDevice device;                             //Unity's input device associated with that controller

    public void SetDevice(InputDevice _d) { device = _d; }  //To assign the device to controller

    //Takes in the collected input values and updates the button values it keeps accordingly
    public void SetInput(bool _t, bool _g, bool _b1, bool _b2, float _tsHorizontal, float _tsVertical, bool _tsClick)
    {
        Button1.UpdateButton(_b1);
        Button2.UpdateButton(_b2);
        Trigger.UpdateButton(_t);
        Grip.UpdateButton(_g);
        ThumbStick = new Vector2(_tsHorizontal, _tsVertical);
        ThumbStickClick.UpdateButton(_tsClick);
    }

    //Takes in the collected position and rotation values and updates the values it keeps accordingly
    public void SetOrientation(Vector3 p, Quaternion r)
    {
        Position = p;
        Rotation = r;
    }

    //We are displaying the controller button values in the game world for real time observation
    public string Debug()
    {
        string s = "";
        s += "T:\t" + Trigger.Current + "\t" + Trigger.Down + "\t" + Trigger.Up + "\n";
        s += "G:\t" + Grip.Current + "\t" + Grip.Down + "\t" + Grip.Up + "\n";
        s += "B1:\t" + Button1.Current + "\t" + Button1.Down + "\t" + Button1.Up + "\n";
        s += "B2:\t" + Button2.Current + "\t" + Button2.Down + "\t" + Button2.Up + "\n";

        s += "TS:\t" + ThumbStick.x + "\t" + ThumbStick.y + "\n";
        s += "TSC:\t" + ThumbStickClick.Current + "\t" + ThumbStickClick.Down + "\t" + ThumbStickClick.Up + "\n";
        s += "POS:\t" + Position.x.ToString("F2") + "\t" + Position.y.ToString("F2") + "\t" + Position.z.ToString("F2") + "\n";
        s += "ROT:\t" + Rotation.eulerAngles.x.ToString("F1") + "\t" + Rotation.eulerAngles.y.ToString("F1") + "\t" 
                                                                     + Rotation.eulerAngles.z.ToString("F1") + "\n";
        return s;
    }

    //Haptic feedback sent to the controller (1 second vibration with 0.5 amplitude).
    public void Vibrate()
    {
        device.SendHapticImpulse(0, 0.5f, 1.0f);
    }
}

//Keeps button states
public struct QuestControllerButton
{
    public bool Down;       //Just activated, similar to Input.GetKeyDown()
    public bool Up;         //Just deactivated, similar to Input.GetKeyUp()
    public bool Current;    //Most recent value, similar to Input.GetKey()
    bool last;              //Keeps the last value to detect changes

    //Detects the changes in the button states
    public void UpdateButton(bool _current)
    {
        Current = _current;
        if (Current != last)
        {
            if (Current) Down = true;
            else Up = true;
            last = Current;
        }
        else
        {
            if (Down) Down = false;
            if (Up) Up = false;
        }
    }
}