using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabGroup : MonoBehaviour
{
    public List<TabButton> tabButtons;
    Color darkGray;
    Color lightGray;
    public TabButton selectedTab;
    public List<GameObject> objectsToSwap;

    private void Start()
    {
        ColorUtility.TryParseHtmlString("#282828", out lightGray);
        ColorUtility.TryParseHtmlString("#181818", out darkGray);
    }

    public void addTab(TabButton button)
    {
        if(tabButtons == null)
        {
            tabButtons = new List<TabButton>();
        }
        tabButtons.Add(button);
    }

    public void onTabEnter(TabButton button)
    {
        ResetTabs();
        if (selectedTab == null || button != selectedTab)
        {
            button.background.color = lightGray;
        }
    }

    public void onTabExit(TabButton button)
    {
        ResetTabs();
    }

    public void onTabSelected(TabButton button)
    {
        selectedTab = button;
        ResetTabs();
        button.background.color = lightGray;

        int index = button.transform.GetSiblingIndex();
        for (int i = 0; i < objectsToSwap.Count; i++)
        {
            if( i == index)
            {
                objectsToSwap[i].SetActive(true);
            } else
            {
                objectsToSwap[i].SetActive(false);
            }
        }
    }

    public void ResetTabs()
    {
        foreach(TabButton button in tabButtons)
        {
            if(selectedTab != null && button == selectedTab)
            {
                continue;
            }
            button.background.color = darkGray;
        }
    }
}
