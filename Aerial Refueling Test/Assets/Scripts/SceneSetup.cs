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
    
    public Boolean inputJoystickC5;
    public Boolean inputKeyboardC5;
    public Boolean inputControllerC5;
    
    public Boolean inputJoystickKC135;
    public Boolean inputKeyboardKC135;
    public Boolean inputControllerKC135;

    public Boolean inputJoystickBoomArm;
    public Boolean inputKeyboardBoomArm;
    public Boolean inputControllerBoomArm;

    public GameObject joystickGreenC5;
    public GameObject keyboardGreenC5;
    public GameObject controllerGreenC5;

    public GameObject joystickGreenKC135;
    public GameObject keyboardGreenKC135;
    public GameObject controllerGreenKC135;

    public GameObject joystickGreenBoomArm;
    public GameObject keyboardGreenBoomArm;
    public GameObject controllerGreenBoomArm;

    public Toggle volumetricClouds;
    public Toggle fog;
    public Slider TimeOfDay;
    public GameObject slider;

    public Boolean volumetricCloudsBool;
    public Boolean fogBool;
    public float timeOfDayFloat;

    // Start is called before the first frame update
    bool intToBool(int val)
    {
        if (val != 0)
            return true;
        else
            return false;
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        timeOfDayFloat = slider.GetComponent<Slider>().value;
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
            inputJoystickC5 = false;
            inputKeyboardC5 = false;
            inputControllerC5 = false;



            joystickGreenC5.SetActive(false);
            keyboardGreenC5.SetActive(false);
            controllerGreenC5.SetActive(false);
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
            inputControllerKC135 = false;
            inputKeyboardKC135 = false;
            inputJoystickKC135 = false;



            joystickGreenKC135.SetActive(false);
            keyboardGreenKC135.SetActive(false);
            controllerGreenKC135.SetActive(false);

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
            inputControllerBoomArm = false;
            inputKeyboardBoomArm = false;
            inputJoystickBoomArm = false;

            joystickGreenBoomArm.SetActive(false);
            keyboardGreenBoomArm.SetActive(false);
            controllerGreenBoomArm.SetActive(false);

        }
    }

    public void SetC5InputJoystick()
    {
        if (playerC5)
        {
            if (inputJoystickC5 == false)
            {
                inputControllerC5 = false;
                inputKeyboardC5 = false;
                inputJoystickC5 = true;



                joystickGreenC5.SetActive(true);
                keyboardGreenC5.SetActive(false);
                controllerGreenC5.SetActive(false);


            }
            else
            {
                inputJoystickC5 = false;
                joystickGreenC5.SetActive(false);
            }
        }
    }

    public void SetC5InputKeyboard()
    {
        if (playerC5)
        {
            if (inputKeyboardC5 == false)
            {
                inputControllerC5 = false;
                inputJoystickC5 = false;
                inputKeyboardC5 = true;



                joystickGreenC5.SetActive(false);
                keyboardGreenC5.SetActive(true);
                controllerGreenC5.SetActive(false);

            }
            else
            {
                inputKeyboardC5 = false;
                keyboardGreenC5.SetActive(false);
            }
        }
    }

    public void SetC5InputController()
    {
        if (playerC5)
        {
            if (inputControllerC5 == false)
            {
                inputJoystickC5 = false;
                inputKeyboardC5 = false;
                inputControllerC5 = true;



                joystickGreenC5.SetActive(false);
                keyboardGreenC5.SetActive(false);
                controllerGreenC5.SetActive(true);

            }
            else
            {
                inputControllerC5 = false;
                controllerGreenC5.SetActive(false);
            }
        }
    }

    public void SetKC135InputJoystick()
    {
        if (playerKC135)
        {
            if (inputJoystickKC135 == false)
            {
                inputControllerKC135 = false;
                inputKeyboardKC135 = false;
                inputJoystickKC135 = true;



                joystickGreenKC135.SetActive(true);
                keyboardGreenKC135.SetActive(false);
                controllerGreenKC135.SetActive(false);


            }
            else
            {
                inputJoystickKC135 = false;
                joystickGreenKC135.SetActive(false);
            }
        }
    }

    public void SetKC135InputKeyboard()
    {
        if (playerKC135)
        {
            if (inputKeyboardKC135 == false)
            {
                inputControllerKC135 = false;
                inputKeyboardKC135 = true;
                inputJoystickKC135 = false;



                joystickGreenKC135.SetActive(false);
                keyboardGreenKC135.SetActive(true);
                controllerGreenKC135.SetActive(false);


            }
            else
            {
                inputKeyboardKC135 = false;
                keyboardGreenKC135.SetActive(false);
            }
        }
    }

    public void SetKC135InputController()
    {
        if (playerKC135)
        {
            if (inputControllerKC135 == false)
            {
                inputControllerKC135 = true;
                inputKeyboardKC135 = false;
                inputJoystickKC135 = false;



                joystickGreenKC135.SetActive(false);
                keyboardGreenKC135.SetActive(false);
                controllerGreenKC135.SetActive(true);


            }
            else
            {
                inputControllerKC135 = false;
                controllerGreenKC135.SetActive(false);
            }
        }
    }

    public void SetBoomArmInputJoystick()
    {
        if (playerBoomArm)
        {
            if (inputJoystickBoomArm == false)
            {
                inputControllerBoomArm = false;
                inputKeyboardBoomArm = false;
                inputJoystickBoomArm = true;

                joystickGreenBoomArm.SetActive(true);
                keyboardGreenBoomArm.SetActive(false);
                controllerGreenBoomArm.SetActive(false);
            }
            else
            {
                inputJoystickBoomArm = false;
                joystickGreenBoomArm.SetActive(false);
            }
        }
    }

    public void SetBoomArmInputKeyboard()
    {
        if (playerBoomArm)
        {
            if (inputKeyboardBoomArm == false)
            {
                inputControllerBoomArm = false;
                inputKeyboardBoomArm = true;
                inputJoystickBoomArm = false;

                joystickGreenBoomArm.SetActive(false);
                keyboardGreenBoomArm.SetActive(true);
                controllerGreenBoomArm.SetActive(false);
            }
            else
            {
                inputKeyboardBoomArm = false;
                keyboardGreenBoomArm.SetActive(false);
            }
        }
    }

    public void SetBoomArmInputController()
    {
        if (playerBoomArm)
        {
            if (inputControllerBoomArm == false)
            {
                inputControllerBoomArm = true;
                inputKeyboardBoomArm = false;
                inputJoystickBoomArm = false;

                joystickGreenBoomArm.SetActive(false);
                keyboardGreenBoomArm.SetActive(false);
                controllerGreenBoomArm.SetActive(true);
            }
            else
            {
                inputControllerBoomArm = false;
                controllerGreenBoomArm.SetActive(false);
            }
        }
    }

    public void SetVolumetricClouds()
    {
        if (volumetricCloudsBool == false)
        {
            volumetricCloudsBool = true;
            volumetricClouds.isOn = true;
        }
        else
        {
            volumetricCloudsBool = false;
            volumetricClouds.isOn = false;
        }
    }

    public void SetFog()
    {
        if (fogBool == false)
        {
            fogBool = true;
            fog.isOn = true;
        }
        else
        {
            fogBool = false;
            fog.isOn = false;
        }
    }
    
    int boolToInt(bool val)
    {
        if (val)
            return 1;
        else
            return 0;
    }

    private void OnDisable()
    {
        
        PlayerPrefs.SetInt("playerC5", boolToInt(playerC5));
        PlayerPrefs.SetInt("playerKC135", boolToInt(playerKC135));
        PlayerPrefs.SetInt("playerBoomArm", boolToInt(playerBoomArm));
        
        PlayerPrefs.SetInt("inputJoystickC5", boolToInt(inputJoystickC5));
        PlayerPrefs.SetInt("inputKeyboardC5", boolToInt(inputKeyboardC5));
        PlayerPrefs.SetInt("inputControllerC5", boolToInt(inputControllerC5));

        PlayerPrefs.SetInt("inputJoystickKC135", boolToInt(inputJoystickKC135));
        PlayerPrefs.SetInt("inputKeyboardKC135", boolToInt(inputKeyboardKC135));
        PlayerPrefs.SetInt("inputControllerKC135", boolToInt(inputControllerKC135));

        PlayerPrefs.SetInt("inputJoystickBoomArm", boolToInt(inputJoystickBoomArm));
        PlayerPrefs.SetInt("inputKeyboardBoomArm", boolToInt(inputKeyboardBoomArm));
        PlayerPrefs.SetInt("inputControllerBoomArm", boolToInt(inputControllerBoomArm));

        PlayerPrefs.SetInt("volumetricCloudsBool", boolToInt(volumetricCloudsBool));
        PlayerPrefs.SetInt("fogBool", boolToInt(fogBool));

        PlayerPrefs.SetInt("TimeOfDay", (int)timeOfDayFloat);

    }

}
