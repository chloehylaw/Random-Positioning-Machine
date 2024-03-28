using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TabButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    public TabGroup tabGroup;

    public Image background;
    public UnityEvent onTabSwitch;
    
    public void OnPointerClick(PointerEventData eventData)
    {
        tabGroup.onTabSelected(this);
        onTabSwitch.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData) 
    {
        tabGroup.onTabEnter(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tabGroup.onTabExit(this);
    }

    private void Start()
    {
        background = GetComponent<Image>();
        tabGroup.addTab(this);
    }

}
