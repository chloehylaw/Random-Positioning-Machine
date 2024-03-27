using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KeyboardScript : MonoBehaviour
{
    public TMP_InputField TextField;
    //public InputField TextField;
    public GameObject Keyboard, EngLayoutSml, EngLayoutBig, EngLayoutSymb, FrLayoutSml, FrLayoutBig, FrLayoutSymb;
    private int CaretPosition = 0;

  
    public void alphabetFunction(string alphabet)
    {
        
        Debug.Log("TextField.text.Length: " + TextField.text.Length);
        Debug.Log("TextField.caretPosition: " + CaretPosition);
        
        if (CaretPosition == TextField.text.Length)
        {
            TextField.text += alphabet;
        } else
        {
            TextField.text = TextField.text.Insert(CaretPosition, alphabet);
        }
        CaretPosition++;

        Debug.Log("Add letter caret position: " + CaretPosition);
    }

    public void BackSpace() 
    {
        if(TextField.text.Length > 0) TextField.text = TextField.text.Remove(TextField.text.Length - 1);
    }

    public void Delete ()
    {
        TextField.text = "";
    }

    public void LeftArrow ()
    {
        if (CaretPosition > 0)
        {
            CaretPosition--;
        }
        Debug.Log("Left arrow caret position: " + CaretPosition);
    }

    public void RightArrow ()
    {
        if (CaretPosition < TextField.text.Length)
        {
            CaretPosition++;
        }
        Debug.Log("Right arrow caret position: " + CaretPosition);
    }

    public void UpArrow ()
    {
        CaretPosition = TextField.text.Length;
    }

    public void DownArrow ()
    {
        CaretPosition = 0;
    }

    private void CloseAllLayouts()
    {
        EngLayoutSml.SetActive(false);
        EngLayoutBig.SetActive(false);
        EngLayoutSymb.SetActive(false);
        FrLayoutSml.SetActive(false);
        FrLayoutBig.SetActive(false);
        FrLayoutSymb.SetActive(false);
    }

    public void ShowLayout(GameObject SetLayout)
    {
        Debug.Log(SetLayout);
        CloseAllLayouts();
        SetLayout.SetActive(true);
    }

    public void OpenKeyboard ()
    {
        if (Keyboard == null) return;
        Animator animator = Keyboard.GetComponent<Animator>();
        if (animator == null) return;
        EngLayoutSml.SetActive(true);
        animator.SetBool("open", true);
    }

    public void CloseKeyboard ()
    {
        EngLayoutBig.SetActive(false);
        EngLayoutSymb.SetActive(false);
        
        if (Keyboard == null) return;
        Animator animator = Keyboard.GetComponent<Animator>();
        if (animator == null) return;
        animator.SetBool("open", false);
        animator.SetBool("close_eng_small", true);
        StartCoroutine(Delay());
        animator.SetBool("close_eng_small", false);


        /*switch (SetLayout.name)
        {
        case "Eng Small":
            animator.SetBool("close_eng_small", true);
            animator.SetBool("open", false);
            animator.SetBool("other", false);
            break;
        case "Eng Caps":
            animator.SetBool("close_eng_caps", true);
            animator.SetBool("open", false);
            animator.SetBool("other", false);
            break;
        case "Eng Shift":
            animator.SetBool("close_eng_shift", true);
            animator.SetBool("open", false);
            animator.SetBool("other", false);
            break;  
        case "Fr Small":
            animator.SetBool("close_fr_small", true);
            animator.SetBool("open", false);
            animator.SetBool("other", false);
            break;
        case "Fr Caps":
            animator.SetBool("close_fr_caps", true);
            animator.SetBool("open", false);
            animator.SetBool("other", false);
            break;
        case "Fr Shift":
            animator.SetBool("close_fr_shift", true);            
            animator.SetBool("open", false);
            animator.SetBool("other", false);
            break;  
        }*/
    }

    private IEnumerator Delay ()
    {
        yield return new WaitForSeconds(10);
    }
}
