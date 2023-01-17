/*using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneSetup : MonoBehaviour
{
    public Boolean playerC5;
    public Boolean playerKC135;
    public Boolean playerBoomArm;
    public Boolean inputJoystick;
    public Boolean inputKeyboard;
    public Boolean inputController;

    public Button joystickButton;
    public Button keyboardButton;
    public Button controllerButton;

    public GameObject joystickGreen;
    public GameObject keyboardGreen;
    public GameObject controllerGreen;

    // Start is called before the first frame update
    void Start()
    {
        // find joystick toggle game object
        joystickButton = GameObject.Find("JoyStickButton").GetComponent<Button>();
        // find keyboard toggle game object
        keyboardButton = GameObject.Find("KeyboardButton").GetComponent<Button>();
        // find controller toggle game object
        controllerButton = GameObject.Find("ControllerButton").GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPlayerC5()
    {
        if (playerC5 == false)
        {
            playerC5 = true;
        } 
        else
        {
            playerC5 = false;
        }
    }

    public void SetPlayerKC135()
    {
        if (playerKC135 == false)
        {
            playerKC135 = true;
        }
        else
        {
            playerKC135 = false;

        }
    }

    public void SetPlayerBoomArm()
    {
        if (playerBoomArm == false)
        {
            playerBoomArm = true;
        }
        else
        {
            playerBoomArm = false;

        }
    }

    public void SetInputJoystick()
    {
        if (inputJoystick == false && joystickButton.onValueChanged != null)
        {
            inputController = false;
            inputKeyboard = false;
            inputJoystick = true;

            keyboardButton.isOn = false;
            controllerButton.isOn = false;
            joystickButton.isOn = true;

            joystickGreen.SetActive(true);
            keyboardGreen.SetActive(false);
            controllerGreen.SetActive(false);


        }
        else
        {
            inputJoystick = false;
            joystickButton.isOn = false;
        }
    }

    public void SetInputKeyboard()
    {
        if (inputKeyboard == false && keyboardButton.onValueChanged != null)
        {
            inputController = false;
            inputJoystick = false;
            inputKeyboard = true;

            joystickButton.isOn = false;
            controllerButton.isOn = false;
            keyboardButton.isOn = true;

            joystickGreen.SetActive(false);
            keyboardGreen.SetActive(true);
            controllerGreen.SetActive(false);

        }
        else
        {
            inputKeyboard = false;
            keyboardButton.isOn = false;
        }
    }

    public void SetInputController()
    {
        if (inputController == false && controllerButton.onValueChanged != null)
        {
            inputJoystick = false;
            inputKeyboard = false;
            inputController = true;

            joystickButton.isOn = false;
            keyboardButton.isOn = false;
            controllerButton.isOn = true;

            joystickGreen.SetActive(false);
            keyboardGreen.SetActive(false);
            controllerGreen.SetActive(true);

        }
        else
        {
            inputController = false;
            controllerButton.isOn = false;
        }
    }


}
*/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneSetup : MonoBehaviour
{
    public Boolean playerC5;
    public Boolean playerKC135;
    public Boolean playerBoomArm;
    public Boolean inputJoystick;
    public Boolean inputKeyboard;
    public Boolean inputController;

    public Button joystickButton;
    public Button keyboardButton;
    public Button controllerButton;

    public GameObject joystickGreen;
    public GameObject keyboardGreen;
    public GameObject controllerGreen;

    // Start is called before the first frame update
    void Start()
    {
        // find joystick toggle game object
        joystickButton = GameObject.Find("JoyStickButton").GetComponent<Button>();
        // find keyboard toggle game object
        keyboardButton = GameObject.Find("KeyboardButton").GetComponent<Button>();
        // find controller toggle game object
        controllerButton = GameObject.Find("ControllerButton").GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetPlayerC5()
    {
        if (playerC5 == false)
        {
            playerC5 = true;
        }
        else
        {
            playerC5 = false;
        }
    }

    public void SetPlayerKC135()
    {
        if (playerKC135 == false)
        {
            playerKC135 = true;
        }
        else
        {
            playerKC135 = false;

        }
    }

    public void SetPlayerBoomArm()
    {
        if (playerBoomArm == false)
        {
            playerBoomArm = true;
        }
        else
        {
            playerBoomArm = false;

        }
    }

    public void SetInputJoystick()
    {
        if (inputJoystick == false)
        {
            inputController = false;
            inputKeyboard = false;
            inputJoystick = true;



            joystickGreen.SetActive(true);
            keyboardGreen.SetActive(false);
            controllerGreen.SetActive(false);


        }
        else
        {
            inputJoystick = false;
        }
    }

    public void SetInputKeyboard()
    {
        if (inputKeyboard == false)
        {
            inputController = false;
            inputJoystick = false;
            inputKeyboard = true;



            joystickGreen.SetActive(false);
            keyboardGreen.SetActive(true);
            controllerGreen.SetActive(false);

        }
        else
        {
            inputKeyboard = false;
        }
    }

    public void SetInputController()
    {
        if (inputController == false)
        {
            inputJoystick = false;
            inputKeyboard = false;
            inputController = true;



            joystickGreen.SetActive(false);
            keyboardGreen.SetActive(false);
            controllerGreen.SetActive(true);

        }
        else
        {
            inputController = false;
        }
    }


}
