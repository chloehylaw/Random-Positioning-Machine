using System;
using System.IO;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class JobListTable : MonoBehaviour
{
    public Transform entryContainer;
    public Transform entryTemplate;
    private void Awake()
    {
        // set the template line to not show
        entryTemplate.gameObject.SetActive(false);
        
        // Get the number of job files
        DirectoryInfo d = new DirectoryInfo(Application.dataPath + "/Data/");
        FileInfo[] f = d.GetFiles("*.csv", SearchOption.AllDirectories);
        int numOfJobFiles = f.Length;
        
        // duplicate the template, length = number of job files
        float templateHeight = 60f;
        for (int i = 0; i < numOfJobFiles; i++)
        {
            Transform entryTransform = Instantiate(entryTemplate, entryContainer);
            RectTransform entryReactTransform = entryTransform.GetComponent<RectTransform>();
            entryReactTransform.anchoredPosition = new Vector2(0, 160f + (-templateHeight * i));
            entryTransform.gameObject.SetActive(true);

            // job numbers
            int jobNumber = i + 1;
            entryTransform.Find("Number").GetComponent<TMP_Text>().text = jobNumber.ToString();

            // Open file to get information (job name and status)
            var filePath = Application.dataPath + "/Data/" + Path.GetFileName(f[i].Name);
            var csvLines = File.ReadAllLines(filePath).Skip(1).ToList();
            
            String jobName = csvLines[0].Split(',')[1];
            String jobStatus = csvLines[0].Split(',')[4];
            
            entryTransform.Find("Job Name").GetComponent<TMP_Text>().text = jobName;
            entryTransform.Find("Job Status").GetComponent<TMP_Text>().text = jobStatus;
            
            // Button to handle opening a mini screen to display info about the job
            Button infoButton = entryTransform.Find("Info").GetComponent<Button>();

            // Button to handle downloading the selected job file
            Button exportButton = entryTransform.Find("Export").GetComponent<Button>();

            // Button to handle deleting the selected job file
            Button deleteButton = entryTransform.Find("Delete").GetComponent<Button>();
            deleteButton.onClick.AddListener(() => DeleteJob(filePath));
        }
    }

    void DeleteJob(String filePath)
    {
        DirectoryInfo dir = new DirectoryInfo(filePath);
        dir.Delete();

    }
}
