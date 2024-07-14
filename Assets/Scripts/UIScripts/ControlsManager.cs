using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ControlsManager : MonoBehaviour
{
    private const string defaultText = "These are the controls\nThey are READ-ONLY, but will become editable in a future update.";
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TMP_InputField forwardInput;
    [SerializeField] private TMP_InputField backwardInput;
    [SerializeField] private TMP_InputField leftInput;
    [SerializeField] private TMP_InputField rightInput;
    [SerializeField] private TMP_InputField jumpInput;
    [SerializeField] private TMP_InputField crouchInput;
    [SerializeField] private TMP_InputField sprintInput;
    [SerializeField] private TMP_InputField fireInput;
    [SerializeField] private TMP_InputField altFireInput;
    [SerializeField] private TMP_InputField reloadInput;
    [SerializeField] private TMP_InputField menuInput;
    [SerializeField] private TMP_InputField interactInput;
    [SerializeField] private TMP_InputField primaryInput;
    [SerializeField] private TMP_InputField secondaryInput;
    [SerializeField] private TMP_InputField toolInput;
    [SerializeField] private TMP_InputField grenadeInput;
    
    // Start is called before the first frame update
    void Start()
    {
        // SetListeners();
        descriptionText.text = defaultText;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    // Function to set listeners
    private void SetListeners()
    {
        TMP_InputField[] fields = {
            forwardInput, backwardInput, leftInput, rightInput, 
            jumpInput, crouchInput, sprintInput, fireInput, 
            altFireInput, reloadInput, menuInput, interactInput, 
            primaryInput, secondaryInput, toolInput, grenadeInput
        };
        
        string[] inputKeys =
        {
            GlobalSceneManager.Forward, GlobalSceneManager.Backward, GlobalSceneManager.Left, GlobalSceneManager.Right,
            GlobalSceneManager.Jump, GlobalSceneManager.Crouch, GlobalSceneManager.Sprint, GlobalSceneManager.Fire,
            GlobalSceneManager.AltFire, GlobalSceneManager.Reload, GlobalSceneManager.Menu, GlobalSceneManager.Interact,
            GlobalSceneManager.Primary, GlobalSceneManager.Secondary, GlobalSceneManager.Tool, GlobalSceneManager.Grenade
        };

        for (int i = 2; i < fields.Length; i++)
        {
            fields[i].onEndEdit.AddListener(v => ChangeControl(v, inputKeys[i], fields[i]));
            fields[i].onSelect.AddListener(v => InputControl(v, fields[i]));
        }
        
        forwardInput.onEndEdit.AddListener(v => ChangeControl(v, GlobalSceneManager.Forward,
            forwardInput));
        forwardInput.onSelect.AddListener(v => InputControl(v, 
            forwardInput));
        forwardInput.onDeselect.AddListener(v => DeselectInput(v, GlobalSceneManager.Forward,
            forwardInput));
        
        backwardInput.onEndEdit.AddListener(v => ChangeControl(v, GlobalSceneManager.Backward,
            backwardInput));
        backwardInput.onSelect.AddListener(v => InputControl(v, 
            backwardInput));
        backwardInput.onDeselect.AddListener(v => DeselectInput(v, GlobalSceneManager.Backward,
            backwardInput));
    }
    
    // Call when the text field is selected
    public void InputControl(string curr, TMP_InputField inputField)
    {
        /*// Listen for an input
        if (Input.anyKey)
        {
            inputField.text = Input.inputString;
            Debug.Log(Input.inputString);
            descriptionText.text = Input.inputString;
        }*/
        descriptionText.text = defaultText;
    }
    
    // Call when text field is deselected
    public void DeselectInput(string curr, string dictKey, TMP_InputField inputField)
    {
        inputField.text = GlobalSceneManager.inputLabelList[dictKey].ToString();
    }
    
    // Call when a key is pressed
    public void KeyPressed()
    {
        
    }
    
    // Call when the value changes
    public void ChangeTextFieldValue(string input)
    {
        
    }
    
    // Call when the edit is finished
    public void ChangeControl(string change, string control, TMP_InputField inputField)
    {
        // Check first if the keycode exists
        try
        {
            string currChange = change.All(char.IsLetter) ? change.ToLower(): change;
            Input.GetKey(currChange);
            descriptionText.text = "CURRENT KEY PRESSED: "+currChange;
        }
        catch (Exception e) 
        {
            // Tell player control is not a valid input key
            descriptionText.text = string.Format("Sorry, [ {0} ] is not a valid input.", change);
            inputField.text = GlobalSceneManager.inputLabelList[control].ToString();
            throw;
        }
        // Now put the keycode into the control
        KeyCode newControl = (KeyCode) Enum.Parse(typeof(KeyCode), change, true);
        if (GlobalSceneManager.CheckIfInControls(newControl) != null && 
            GlobalSceneManager.CheckIfInControls(newControl) != control)
        {
            descriptionText.text = string.Format("Sorry, [ {0} ] has already been mapped to a control.", change.ToUpper());
            inputField.text = GlobalSceneManager.inputLabelList[control].ToString();
            return;
        }
        descriptionText.text = string.Format("Changed Control for [ {0} ] from [ {1} ] to [ {2} ]", control, 
            GlobalSceneManager.inputLabelList[control], change);
        GlobalSceneManager.inputLabelList[control] = newControl;
        forwardInput.text = newControl.ToString();
    }
}
