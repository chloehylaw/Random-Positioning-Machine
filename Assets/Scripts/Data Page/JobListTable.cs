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
    public Button closeInfoPanel;
    
    public TMP_Text jobName;
    public TMP_Text gravityValue;
    public TMP_Text rotationalAlgorithm;
    public TMP_Text status;
    public TMP_Text startTime;
    public TMP_Text endTime;
    public TMP_Text abortTime;
    
    public GameObject deletePanel;
    public TMP_Text deletePanelJobName;
    public Button cancelDelete;
    public Button confirmDelete;
    
    private void Start()
    {
        // set the template line to not show
        entryTemplate.gameObject.SetActive(false);
        infoPanel.gameObject.SetActive(false);
        //deletePanel.gameObject.SetActive(false);

        DisplayTable();
    }

    public void ControlPageStart()
    {
        // delete all created clones
        var count = entryContainer.childCount;
        for (var i = 1; i < count; i++)
        {
            Destroy(entryContainer.GetChild(i).gameObject);
        }
        
        DisplayTable();
    }

    /// <summary>
    /// Displays the list of jobs in a table
    /// </summary>
    private void DisplayTable()
    {
        // Get the number of job files
        if (!Directory.Exists(Application.dataPath + "/Data/"))
            Directory.CreateDirectory(Application.dataPath + "/Data/");
        DirectoryInfo d = new DirectoryInfo(Application.dataPath + "/Data/");
        Debug.Log(Application.dataPath + "/Data/");
        FileInfo[] f = d.GetFiles("*.csv", SearchOption.AllDirectories);
        var numOfJobFiles = f.Length;
        
        // duplicate the template, length = number of job files
        var templateHeight = 60f;
        for (var i = 0; i < numOfJobFiles; i++)
        {
            RectTransform containerStartPosition = entryContainer.GetComponent<RectTransform>();
            RectTransform containerHeight = entryContainer.GetComponent<RectTransform>();
            containerStartPosition.anchoredPosition = new Vector2(409.255f, transform.position.y);

            if (i > 5)
            {
                var height = 380f + templateHeight * (i - 5);
                containerHeight.sizeDelta = new Vector2(818.5f, height);
                containerStartPosition.anchoredPosition = new Vector2(409.255f, -(height / 2));
            }
            
            Transform entryTransform = Instantiate(entryTemplate, entryContainer);
            RectTransform entryReactTransform = entryTransform.GetComponent<RectTransform>();
            entryReactTransform.anchoredPosition = new Vector2(409.255f, -30f + (-templateHeight * i));
            entryTransform.gameObject.SetActive(true);

            // job numbers
            int jobNumber = i + 1;
            entryTransform.Find("Number").GetComponent<TMP_Text>().text = jobNumber.ToString();

            // Open file to get information (job name and status)
            var filePath = Application.dataPath + "/Data/" + Path.GetFileName(f[i].Name);
            var csvLines = File.ReadAllLines(filePath).Skip(1).ToList();

            //Debug.Log(filePath);
            string fileName = Path.GetFileName(f[i].Name);
            //Debug.Log(Path.GetFileName(f[i].Name));
            
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
            deleteButton.onClick.AddListener(() => OpenDeletePanel(jobName, filePath));
        }
    }

    /// <summary>
    /// By clicking the info button, it opens a panel with the details of the job
    /// </summary>
    /// <param name="filePath"></param>
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

    /// <summary>
    /// Close the info panel
    /// </summary>
    void ClosePanel()
    {
        infoPanel.gameObject.SetActive(false);
    }

    /// <summary>
    /// Opens the delete job panel 
    /// </summary>
    /// <param name="jobName"></param>
    /// <param name="filePath"></param>
    void OpenDeletePanel(String jobName, String filePath)
    {
        deletePanel.gameObject.SetActive(true);
        
        deletePanelJobName.text = jobName;
        
        cancelDelete.onClick.AddListener(ClickCancelButton);
        confirmDelete.onClick.AddListener(() => ClickDeleteButton(filePath));
    }

    /// <summary>
    /// Cancel button for the delete panel
    /// </summary>
    void ClickCancelButton()
    {
        deletePanel.gameObject.SetActive(false);
    }

    /// <summary>
    /// Delete button for the delete panel
    /// </summary>
    /// <param name="filePath"></param>
    void ClickDeleteButton(String filePath)
    {
        var count = entryContainer.childCount;
        for (var i = 1; i < count; i++)
        {
            Destroy(entryContainer.GetChild(i).gameObject);
        }
        File.Delete(filePath);
        deletePanel.gameObject.SetActive(false);
        DisplayTable();
    }
}
