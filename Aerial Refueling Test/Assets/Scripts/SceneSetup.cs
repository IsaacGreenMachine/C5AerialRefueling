using System;
using UnityEngine;
using UnityEngine.UI;

public class SceneSetup : MonoBehaviour
{
    /// <summary>
    /// The booleans for the player selection.
    /// </summary>
    public Boolean playerC5;
    public Boolean playerKC135;
    public Boolean playerBoomArm;

    /// <summary>
    /// The booleans for the input selection, based on player selection.
    /// </summary>
    public Boolean inputJoystickC5;
    public Boolean inputKeyboardC5;
    public Boolean inputControllerC5;
    
    public Boolean inputJoystickKC135;
    public Boolean inputKeyboardKC135;
    public Boolean inputControllerKC135;

    public Boolean inputJoystickBoomArm;
    public Boolean inputKeyboardBoomArm;
    public Boolean inputControllerBoomArm;
    
    /// <summary>
    /// GameObjects for toggling the selection of the player within the settings page.
    /// </summary>
    public GameObject playerC5GO;
    public GameObject playerKC135GO;
    public GameObject playerBoomArmGO;

    /// <summary>
    /// GameObjects for toggling the selection of input based on player selection.
    /// </summary>
    public GameObject joystickGreenC5;
    public GameObject keyboardGreenC5;
    public GameObject controllerGreenC5;

    public GameObject joystickGreenKC135;
    public GameObject keyboardGreenKC135;
    public GameObject controllerGreenKC135;

    public GameObject joystickGreenBoomArm;
    public GameObject keyboardGreenBoomArm;
    public GameObject controllerGreenBoomArm;

    /// <summary>
    /// GameObject for toggling the selection of fog and VC.
    /// </summary>
    public GameObject fogGO;
    public GameObject volumetricCloudsGO;

    /// <summary>
    /// The time of day slider
    /// </summary>
    public Slider TimeOfDay;
    public GameObject slider;

    /// <summary>
    /// Booleans to be able to store fog and VC settings into PlayerPrefs.
    /// </summary>
    public Boolean volumetricCloudsBool;
    public Boolean fogBool;
    /// <summary>
    /// Float for storing time of day in PlayerPrefs
    /// </summary>
    public float timeOfDayFloat;

    /// <summary>
    /// Helper function to convert int to bool, PlayerPrefs only stores ints and strings
    /// </summary>
    bool intToBool(int val)
    {
        if (val != 0)
            return true;
        else
            return false;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Grab all of the values from PlayerPrefs to load the chosen settings
        playerC5 = intToBool(PlayerPrefs.GetInt("playerC5"));
        playerKC135 = intToBool(PlayerPrefs.GetInt("playerKC135"));
        playerBoomArm = intToBool(PlayerPrefs.GetInt("playerBoomArm"));

        inputJoystickC5 = intToBool(PlayerPrefs.GetInt("inputJoystickC5"));
        inputKeyboardC5 = intToBool(PlayerPrefs.GetInt("inputKeyboardC5"));
        inputControllerC5 = intToBool(PlayerPrefs.GetInt("inputControllerC5"));

        inputJoystickKC135 = intToBool(PlayerPrefs.GetInt("inputJoystickKC135"));
        inputKeyboardKC135 = intToBool(PlayerPrefs.GetInt("inputKeyboardKC135"));
        inputControllerKC135 = intToBool(PlayerPrefs.GetInt("inputControllerKC135"));

        inputJoystickBoomArm = intToBool(PlayerPrefs.GetInt("inputJoystickBoomArm"));
        inputKeyboardBoomArm = intToBool(PlayerPrefs.GetInt("inputKeyboardBoomArm"));
        inputControllerBoomArm = intToBool(PlayerPrefs.GetInt("inputControllerBoomArm"));

        volumetricCloudsBool = intToBool(PlayerPrefs.GetInt("volumetricCloudsBool"));
        fogBool = intToBool(PlayerPrefs.GetInt("fogBool"));
        timeOfDayFloat = PlayerPrefs.GetInt("TimeOfDay");

        // If we have previously selected settings, set them based on PlayerPrefs
        if (playerC5)
        {
            playerC5GO.SetActive(true);
            if (inputJoystickC5)
            {
                joystickGreenC5.SetActive(true);
            }
            if (inputKeyboardC5)
            {
                keyboardGreenC5.SetActive(true);
            }
            if (inputControllerC5)
            {
                controllerGreenC5.SetActive(true);
            }
            else
            {
                inputKeyboardC5 = true;
                keyboardGreenC5.SetActive(true);
            }
        }

        // If we have previously selected settings, set them based on PlayerPrefs
        if (playerKC135)
        {
            playerKC135GO.SetActive(true);
            if (inputJoystickKC135)
            {
                joystickGreenKC135.SetActive(true);
            }
            if (inputKeyboardKC135)
            {
                keyboardGreenKC135.SetActive(true);
            }
            if (inputControllerKC135)
            {
                controllerGreenKC135.SetActive(true);
            }
            else
            {
                inputKeyboardKC135 = true;
                keyboardGreenKC135.SetActive(true);
            }
        }

        // If we have previously selected settings, set them based on PlayerPrefs
        if (playerBoomArm)
        {
            playerBoomArmGO.SetActive(true);
            if (inputJoystickBoomArm)
            {
                joystickGreenBoomArm.SetActive(true);
            }
            if (inputKeyboardBoomArm)
            {
                keyboardGreenBoomArm.SetActive(true);
            }
            if (inputControllerBoomArm)
            {
                controllerGreenBoomArm.SetActive(true);
            }
            else
            {
                inputKeyboardBoomArm = true;
                keyboardGreenBoomArm.SetActive(true);
            }
        }

        // If we have previously selected settings, set them based on PlayerPrefs
        if (volumetricCloudsBool)
        {
            volumetricCloudsGO.SetActive(true);
        }
        // If we have previously selected settings, set them based on PlayerPrefs
        if (fogBool)
        {
            fogGO.SetActive(true);
        }
        // Sets the slider based on PlayerPrefs
        slider.GetComponent<Slider>().value = timeOfDayFloat;
    }

    // Update is called once per frame
    void Update()
    {
        // updates the timeOfDayFloat based on the slider, needs to be checked every frame, hence why it's in update
        timeOfDayFloat = slider.GetComponent<Slider>().value;
    }

    /// <summary>
    /// Toggles whether the C5 is Player or AI controlled
    /// </summary>
    public void SetPlayerC5()
    {
        // c5 is false and we click to toggle, turn it on
        if (playerC5 == false)
        {
            playerC5 = true;
            playerC5GO.SetActive(true);
        }
        // if c5 is selected, clear the player inputs and toggle off the c5
        else
        {
            playerC5 = false;
            inputJoystickC5 = false;
            inputKeyboardC5 = false;
            inputControllerC5 = false;


            playerC5GO.SetActive(false);
            joystickGreenC5.SetActive(false);
            keyboardGreenC5.SetActive(false);
            controllerGreenC5.SetActive(false);
        }
    }

    /// <summary>
    /// Toggles whether the KC135 is Player or AI controlled
    /// </summary>
    public void SetPlayerKC135()
    {
        // kc135 is false and we click to toggle, turn it on
        if (playerKC135 == false)
        {
            playerKC135 = true;
            playerKC135GO.SetActive(true);
        }
        // if kc135 is selected, clear the player inputs and toggle off the kc135
        else
        {
            playerKC135 = false;
            inputControllerKC135 = false;
            inputKeyboardKC135 = false;
            inputJoystickKC135 = false;


            playerKC135GO.SetActive(false);
            joystickGreenKC135.SetActive(false);
            keyboardGreenKC135.SetActive(false);
            controllerGreenKC135.SetActive(false);

        }
    }

    /// <summary>
    /// Toggles whether the BoomArm is Player or AI controlled
    /// </summary>
    public void SetPlayerBoomArm()
    {
        // boomArm is false and we click to toggle, turn it on
        if (playerBoomArm == false)
        {
            playerBoomArm = true;
            playerBoomArmGO.SetActive(true);
        }
        // if boomArm is selected, clear the player inputs and toggle off the c5
        else
        {
            playerBoomArm = false;
            inputControllerBoomArm = false;
            inputKeyboardBoomArm = false;
            inputJoystickBoomArm = false;

            playerBoomArmGO.SetActive(false);
            joystickGreenBoomArm.SetActive(false);
            keyboardGreenBoomArm.SetActive(false);
            controllerGreenBoomArm.SetActive(false);

        }
    }

    /// <summary>
    /// Allows toggle of C5 Joystick control
    /// </summary>
    public void SetC5InputJoystick()
    {
        // make sure c5 is player controlled
        if (playerC5)
        {
            // joystick is selected, clear all other inputs and select joystick
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

    /// <summary>
    /// Allows toggle of C5 Keyboard control
    /// </summary>
    public void SetC5InputKeyboard()
    {
        // make sure c5 is player controlled
        if (playerC5)
        {
            // keyboard is selected, clear all other inputs and select keyboard
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

    /// <summary>
    /// Allows toggle of C5 Controller control
    /// </summary>
    public void SetC5InputController()
    {
        // make sure c5 is player controlled
        if (playerC5)
        {
            // controller is selected, clear all other inputs and select controller
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

    /// <summary>
    /// Allows toggle of KC135 Joystick control
    /// </summary>
    public void SetKC135InputJoystick()
    {
        // make sure kc135 is player controlled
        if (playerKC135)
        {
            // joystick is selected, clear all other inputs and select joystick
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

    /// <summary>
    /// Allows toggle of KC135 Keyboard control
    /// </summary>
    public void SetKC135InputKeyboard()
    {
        // make sure kc135 is player controlled
        if (playerKC135)
        {
            // keyboard is selected, clear all other inputs and select keyboard
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

    /// <summary>
    /// Allows toggle of KC135 Controller control
    /// </summary>
    public void SetKC135InputController()
    {
        // make sure kc135 is player controlled
        if (playerKC135)
        {
            // controller is selected, clear all other inputs and select controller
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

    /// <summary>
    /// Allows toggle of BoomArm Joystick control
    /// </summary>
    public void SetBoomArmInputJoystick()
    {
        // make sure BoomArm is player controlled
        if (playerBoomArm)
        {
            // joystick is selected, clear all other inputs and select joystick
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

    /// <summary>
    /// Allows toggle of BoomArm Keyboard control
    /// </summary>
    public void SetBoomArmInputKeyboard()
    {
        // make sure BoomArm is player controlled
        if (playerBoomArm)
        {
            // keyboard is selected, clear all other inputs and select keyboard
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

    /// <summary>
    /// Allows toggle of BoomArm Controller control
    /// </summary>
    public void SetBoomArmInputController()
    {
        // make sure BoomArm is player controlled
        if (playerBoomArm)
        {
            // controller is selected, clear all other inputs and select controller
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

    /// <summary>
    /// Toggle for turning volumetric clouds on/off
    /// </summary>
    public void SetVolumetricClouds()
    {
        if (volumetricCloudsBool == false)
        {
            volumetricCloudsBool = true;
            volumetricCloudsGO.SetActive(true);
        }
        else
        {
            volumetricCloudsBool = false;
            volumetricCloudsGO.SetActive(false);
        }
    }

    /// <summary>
    /// Toggle for turning fog on/off
    /// </summary>
    public void SetFog()
    {
        if (fogBool == false)
        {
            fogBool = true;
            fogGO.SetActive(true);
        }
        else
        {
            fogBool = false;
            fogGO.SetActive(false);
        }
    }
    
    /// <summary>
    /// Helper function to turn bools to int (PlayerPrefs does not allow storage of bools)
    /// </summary>
    int boolToInt(bool val)
    {
        if (val)
            return 1;
        else
            return 0;
    }

    /// <summary>
    /// When the settings page is closed, send the true/false values and ToD float to PlayerPrefs to store
    /// </summary>
    private void OnDisable()
    {
        // Set int in Player prefs, as this name, using our helper function on the Booleans we created to track our toggles
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
