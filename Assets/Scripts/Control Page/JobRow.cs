using System.Collections;
using System.Collections.Generic;
using System.IO;
using Control_Page;
using TMPro;
using UnityEngine;

public class JobRow : MonoBehaviour
{
    TMP_InputField JobTitle;

    // Start is called before the first frame update
    void Start()
    {
        JobTitle = GetComponentsInChildren<TMP_InputField>()[0];
    }

    public string GetJobTitle()
    {
        return JobTitle.text;
    }

    public bool AttemptStart()
    {
        foreach (var path in Directory.GetFiles(Application.dataPath + "/Data/"))
        {
            if (path[(path.IndexOf('_')+1)..] == JobTitle.text + ".csv")
            {
                return false;
            }
        }

        return true;
    }

}
