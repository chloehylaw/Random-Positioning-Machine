using System;
using System.Globalization;
using System.IO;
using System.Text;
using TMPro;
using UnityEngine;

namespace Control_Page
{
    public class DataHandler : MonoBehaviour
    {
        public static DataHandler instance;
    
        private DateTime currentTime;
        public EndDateRow endDateRow;
        public TimeRow timeRow;
        public JobRow jobRow;
        public GravityRow gravityRow;
        public AlgorithmRow algorithmRow;

        private enum JobStatus {Running, Completed, Inactive}

        // home page info
        public TMP_Text jobText;
        public TMP_Text statusText;
        public TMP_Text elapsedTimeText;
        public TMP_Text remainingTimeText;

        public GameObject currentJobName;
        public GameObject currentStatus;
        public GameObject elapsedTimeStatus;
        public GameObject remainingTimeStatus;

    
        // control page info
        [SerializeField] private TMP_Dropdown inputRotationalAlgorithm;

        public class Job
        {
            public string guid;
            public string jobName;
            public string gravityValue;
            public string rotationalAlgorithm;
            public string status;
            public string startTime;
            public string endTime;
        }

        private Job inputJob;
    
        void Start()
        {
            if (FindObjectsOfType<DataHandler>().Length == 1)
            {
                DontDestroyOnLoad(gameObject);
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        
            // home page info
            jobText = currentJobName.GetComponent<TMP_Text>();
            jobText.text = "";
        
            statusText = currentStatus.GetComponent<TMP_Text>();
            statusText.text = "";
        
            elapsedTimeText = elapsedTimeStatus.GetComponent<TMP_Text>();
            elapsedTimeText.text = "";
        
            remainingTimeText = remainingTimeStatus.GetComponent<TMP_Text>();
            remainingTimeText.text = "";
        }

        void Update()
        {
            currentTime = DateTime.Now;
            
            // find if there are any running jobs
            /*if (inputJob.status == JobStatus.Running.ToString())
            {
                Debug.Log(inputJob.startTime);

                DateTime startTime = Convert.ToDateTime(inputJob.startTime);
                DateTime endTime = Convert.ToDateTime(inputJob.endTime);

                jobText.text = inputJob.jobName;
                statusText.text = inputJob.status;
                elapsedTimeText.text = currentTime.Subtract(startTime).ToString();
                remainingTimeText.text = endTime.Subtract(currentTime).ToString();
            }*/
        }

        public void CreateCSVFile()
        {
            if (timeRow.AttemptStart() &&  gravityRow.AttemptStart() && algorithmRow.AttemptStart() && jobRow.AttemptStart())
            {
                
                inputJob = new Job
                {
                    guid = Guid.NewGuid().ToString(),
                    jobName = jobRow.GetJobTitle(),
                    gravityValue = SystemHandler.instance.gravity.ToString(CultureInfo.CurrentCulture),
                    rotationalAlgorithm = inputRotationalAlgorithm.options[inputRotationalAlgorithm.value].text,
                    status = JobStatus.Running.ToString(),
                    startTime = DateTime.Now.ToString("yy-MM-dd HH-mm-ss"),
                    endTime = endDateRow.endDate.text,
                };
            
                StringBuilder sb = new StringBuilder();
                string[] columnNames = { "guid", "jobName", "gravityValue", "rotationalAlgorithm", "status", "saveTime", "startTime", "endTime" };
                sb.AppendLine(string.Join(",", columnNames));

                string[] jobData = { inputJob.guid, inputJob.jobName, inputJob.gravityValue, inputJob.rotationalAlgorithm, inputJob.status, inputJob.startTime, inputJob.startTime, inputJob.endTime };
                sb.AppendLine(string.Join(",", jobData));

                string pathName = Application.dataPath + "/Data/" + inputJob.startTime.Replace(",", "") + "_" + inputJob.jobName + ".csv";
                if (File.Exists(pathName))
                {
                    string[] lines = File.ReadAllLines(pathName);
                    lines[1] = string.Join(",", jobData);
                    File.WriteAllLines(pathName, lines);
                }
                else
                {
                    //File.Create(pathName);
                    File.WriteAllText(pathName, sb.ToString());
                }
                
                /*
                DateTime startTime = Convert.ToDateTime(inputJob.startTime);
                DateTime endTime = Convert.ToDateTime(inputJob.endTime);

                jobText.text = inputJob.jobName;
                statusText.text = inputJob.status;
                elapsedTimeText.text = currentTime.Subtract(startTime).ToString();
                remainingTimeText.text = endTime.Subtract(currentTime).ToString();
                */
                
            }
            else
            {
                Debug.Log("Error " + (gravityRow.AttemptStart()?"":"gravity ") + 
                          (jobRow.AttemptStart()?"":"job ") + 
                          (timeRow.AttemptStart()?"":"time ") + 
                          (algorithmRow.AttemptStart()?"":"algorithm "));
            }
        }
    }
}
