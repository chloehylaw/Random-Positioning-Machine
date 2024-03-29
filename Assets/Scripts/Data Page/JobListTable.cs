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

    public GameObject infoPanel;
    public TMP_Text jobName;
    public TMP_Text gravityValue;
    public TMP_Text rotationalAlgorithm;
    public TMP_Text status;
    public TMP_Text startTime;
    public TMP_Text endTime;
    public TMP_Text abortTime;
    public Button closeInfoPanel;
    
    private void Awake()
    {
        // set the template line to not show
        entryTemplate.gameObject.SetActive(false);
        infoPanel.gameObject.SetActive(false);
        
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
            infoButton.onClick.AddListener(() => OpenInfoPanel(filePath));

            // Button to handle downloading the selected job file
            Button exportButton = entryTransform.Find("Export").GetComponent<Button>();

            // Button to handle deleting the selected job file
            Button deleteButton = entryTransform.Find("Delete").GetComponent<Button>();
            deleteButton.onClick.AddListener(() => DeleteJob(filePath));
        }
    }

    void OpenInfoPanel(String filePath)
    {
        var csvLines = File.ReadAllLines(filePath).Skip(1).ToList();
            
        jobName.text = csvLines[0].Split(',')[1];
        gravityValue.text = "Gravity Value: " + csvLines[0].Split(',')[2];
        rotationalAlgorithm.text = "Rotational Algorithm: " + csvLines[0].Split(',')[3];
        status.text = "Status: " + csvLines[0].Split(',')[4];
        startTime.text = "Start Time: " + csvLines[0].Split(',')[5];
        endTime.text = "End Time: " + csvLines[0].Split(',')[7];

        if (csvLines[0].Split(',')[4] == Job.JobStatus.Abort.ToString())
        {
            abortTime.text = "Abort Time: " + csvLines[0].Split(',')[8];
        }
        else
        {
            abortTime.text = "";
        }
        infoPanel.gameObject.SetActive(true);
        closeInfoPanel.onClick.AddListener(ClosePanel);
    }

    void ClosePanel()
    {
        infoPanel.gameObject.SetActive(false);
    }

    void DeleteJob(String filePath)
    {
        DirectoryInfo dir = new DirectoryInfo(filePath);
        dir.Delete(true);
    }
}
