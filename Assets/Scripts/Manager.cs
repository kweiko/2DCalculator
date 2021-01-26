using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
Class to handle all global functions, including button size
and calc logic.
*/
public class Manager : MonoBehaviour
{
    public VerticalLayoutGroup buttonGroup;
    public HorizontalLayoutGroup bottomRow; //reference the last row
    public RectTransform canvasRect;
    CalcButton[] bottomButtons; //reference the bottom row's buttons

    public Text digitLabel;
    public Text storedDigitLabel;
    public Text operatorLabel;
    bool errorDisplayed;
    bool displayValid;
    bool specialAction;
    double currentVal;
    double storedVal;
    double result;
    char storedOperator;
    bool canvasChanged;

    private void Awake(){
        bottomButtons = bottomRow.GetComponentsInChildren<CalcButton>();
    }

    // Start is called before the first frame update
    void Start()
    {
        bottomRow.childControlWidth = false;
        canvasChanged = true;
        buttonTapped('c');
    }

    // Update is called once per frame
    void Update()
    {
        if(canvasChanged){
            canvasChanged = false;
            adjustButtons();
        }

    }

    /* This is called automatically when window is changed */
    private void OnRectTransformDimensionsChange(){
        canvasChanged = true;
    }

    /* This method adjusts the buttons on the bottom row:
        1. Get the width of the canvas and divide it by 4 to calc the size of one button
        2. Adjust the button width with spacing
        3. Set all buttons to this width, except the first button on the last row.
        4. Set the first button width to twice that of a normal button plus one horisontal spacing
    */
    void adjustButtons(){
        if(bottomButtons == null || bottomButtons.Length == 0) return;
        
        float buttonSize = canvasRect.sizeDelta.x/4;
        float bWidth = buttonSize - bottomRow.spacing;

        for(int i = 1; i < bottomButtons.Length; i++){
            bottomButtons[i].rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, bWidth);
        }
        bottomButtons[0].rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, bWidth * 2 + bottomRow.spacing);
    }

    void clearCalc(){
        digitLabel.text = "0";
        storedDigitLabel.text = "";
        operatorLabel.text = "";
        specialAction = displayValid = errorDisplayed = false;
        currentVal = result = storedVal = 0;
        storedOperator = ' ';
    }
    
    /*Update the display label*/
    void updateDigitLabel(){
        if(!errorDisplayed){
            digitLabel.text = currentVal.ToString();
        }
        displayValid = false;
    }

    /*Evaluate the calculation based on the passed operator*/
    void calcResult(char activeOp){
        switch(activeOp){
            //plus, minus operator cases needed
            case 'x':
                result = storedVal * currentVal;
                currentVal = result;
                updateDigitLabel();
                break;

            case '÷':
                if(currentVal != 0){
                    result = (storedVal / currentVal);
                } else {
                    errorDisplayed = true;
                    digitLabel.text = "ERROR";
                }
                currentVal = result;
                updateDigitLabel();
                break;

            case '=':
                result = currentVal;
                break;
                
            default: Debug.Log("unknown" + activeOp);
            
            break;
        }
    }

    public void buttonTapped(char caption){
        
        //If there's an error displayed then reset
        if(errorDisplayed){
            clearCalc();
        }

        /* If the button clicked is a number or a dot
        then evaluate the validity of the input and update the calculator display,
        inform that it shows a valid value */
        if((caption >= '0' && caption<= '9') || caption == '.'){
            if(digitLabel.text.Length < 15 || !displayValid){
                if(!displayValid)
                    digitLabel.text = (caption == '.'? "0": "");
                else if (digitLabel.text == "0" && caption != '.')
                    digitLabel.text = "";
                
                digitLabel.text += caption;
                displayValid = true;
            }
        }

        //Otherwise if the C button is clicked
        else if(caption == 'c'){
            //clear the calculator session
        }

        //Otherwise if the polarity sign is clicked,
        //then convert the polarity and update the calculator display
        //inform a special sign has been used
        else if(caption == '±'){
            currentVal = -double.Parse(digitLabel.text);
            updateDigitLabel();
            specialAction = true;
        }

        //Otherwise if the modularity sign is clicked,
        //then convert the value to modular and update the calculator display
        //inform a special sign has been used
        else if(caption == '%'){
            currentVal = double.Parse(digitLabel.text)/100.0d;
            updateDigitLabel();
            specialAction = true;
        }

        /*
        Otherwise if the calculator shows a valid number,
        OR the operator that's been used is "equals",
        OR a special sign has been used, then do the following
        */
        else if (displayValid || storedOperator == '=' || specialAction){
            currentVal = double.Parse(digitLabel.text);
            displayValid = false;

            if(storedOperator!= ' '){
                calcResult(storedOperator);
                storedOperator = ' ';
            }
            operatorLabel.text = caption.ToString();
            storedOperator = caption;
            if(storedOperator != '=')
                storedVal = currentVal;
            updateDigitLabel();
            specialAction = false;
        }
    }

}


