using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreensaveToggle : MonoBehaviour
{
    [SerializeField] public GameObject child;

    public void toggleChild()
    {
        child.SetActive(!child.activeSelf);
    }

}
