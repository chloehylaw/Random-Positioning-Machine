using System;
using System.Collections;
using System.Globalization;
using System.Linq;
using System.IO;
using System.Text;
using TMPro;
using UnityEngine;

namespace Control_Page
{
    public class DataHandler : MonoBehaviour
    {
        public static DataHandler instance;
        public GameObject alertPanel;
        public TMP_Text alertMessage;

        private Job currentJob;


        public TimeRow timeRow;
        public JobRow jobRow;
        public GravityRow gravityRow;
        public AlgorithmRow algorithmRow;

        // control page info
        [SerializeField] private TMP_Dropdown inputRotationalAlgorithm;

        void Start()
        {
            alertPanel.gameObject.SetActive(false);

            if (FindObjectsOfType<DataHandler>().Length == 1)
            {
                DontDestroyOnLoad(gameObject);
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        IEnumerator WaitForFrame()
        {
            yield return new WaitForEndOfFrame();
            currentJob = SystemHandler.instance.currentJob;
        }

        public void UpdateCurrentJob()
        {
            StartCoroutine(WaitForFrame());
        }

        public void CheckRunningJob()
        {
            if (currentJob.status == Job.JobStatus.Running)
            {
                alertPanel.gameObject.SetActive(true);
                alertMessage.text = "Do you want to abort " + currentJob.jobName + " job and start " + jobRow.GetJobTitle() + " job?";
            }
        }

        public void ClickYesButton()
        {
            // set current job and set the status to abort
            currentJob.status = Job.JobStatus.Abort;

            // update the current job CSV file
            UpdateCSV();

            alertPanel.gameObject.SetActive(false);
            CreateCSVFile();
        }

        public void ClickCancelButton()
        {
            alertPanel.gameObject.SetActive(false);
        }

        /// <summary>
        /// Create a new CSV file for the created new job
        /// </summary>
        public void CreateCSVFile()
        {
            if (timeRow.AttemptStart() && gravityRow.AttemptStart() && algorithmRow.AttemptStart() && jobRow.AttemptStart())
            {
                Job inputJob = Instantiate(SystemHandler.instance.EmptyJobPrefab);
                inputJob.guid = Guid.NewGuid();
                inputJob.jobName = jobRow.GetJobTitle();
                inputJob.gravityValue = gravityRow.DesiredGravity;
                inputJob.rotationalAlgorithm = algorithmRow.DesiredAlgorithm;
                inputJob.status = Job.JobStatus.Running;
                inputJob.startTime = DateTime.Now;
                //inputJob.startTime = DateTime.Now.ToString("yy-MM-dd HH-mm-ss");
                inputJob.expectedEndTime = SystemHandler.instance.endDate;
                inputJob.endTime = DateTime.MinValue;
                inputJob.abortTime = DateTime.MinValue;


                SystemHandler.instance.currentJob = inputJob;

                StringBuilder sb = new StringBuilder();
                string[] columnNames =
                {
                    "GUID",
                    "Job Name",
                    "Gravity Value (g)",
                    "Rotational Algorithm",
                    "Status",
                    "Start Time",
                    "Expected End Time",
                    "End Time",
                    "Abort Time"
                };
                sb.AppendLine(string.Join(",", columnNames));

                string[] jobData =
                {
                    inputJob.guid.ToString(),
                    inputJob.jobName,
                    inputJob.gravityValue.ToString(CultureInfo.InvariantCulture),
                    inputJob.rotationalAlgorithm.ToString(),
                    inputJob.status.ToString(),
                    inputJob.startTime.ToString(CultureInfo.InvariantCulture),
                    inputJob.expectedEndTime.ToString(CultureInfo.InvariantCulture),
                    inputJob.endTime.ToString(CultureInfo.InvariantCulture),
                    inputJob.abortTime.ToString(CultureInfo.InvariantCulture)
                };
                sb.AppendLine(string.Join(",", jobData));

                //string pathName = Application.dataPath + "/Data/" + inputJob.startTime.ToString("yy-MM-dd HH-mm-ss").Replace(",", "") + "_" + inputJob.jobName + ".csv";
                string pathName = Application.dataPath + "/Data/" +
                                  inputJob.startTime.ToString("yy-MM-dd HH-mm-ss").Replace(" ", "_") +
                                  "_" + inputJob.jobName + ".csv";

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
            }
            else
            {
                // Debug.Log("Error " + (gravityRow.AttemptStart() ? "" : "gravity ") +
                //           (jobRow.AttemptStart() ? "" : "job ") +
                //           (timeRow.AttemptStart() ? "" : "time ") +
                //           (algorithmRow.AttemptStart() ? "" : "algorithm "));
            }
        }
        /// <summary>
        /// Update the CSV file
        /// </summary>
        public void UpdateCSV()
        {
            string[] jobData =
            {
            SystemHandler.instance.currentJob.guid.ToString(),
            SystemHandler.instance.currentJob.jobName,
            SystemHandler.instance.currentJob.gravityValue.ToString(CultureInfo.InvariantCulture),
            SystemHandler.instance.currentJob.rotationalAlgorithm.ToString(),
            SystemHandler.instance.currentJob.status.ToString(),
            SystemHandler.instance.currentJob.startTime.ToString(CultureInfo.InvariantCulture),
            SystemHandler.instance.currentJob.expectedEndTime.ToString(CultureInfo.InvariantCulture),
            SystemHandler.instance.currentJob.endTime.ToString(CultureInfo.InvariantCulture),
            SystemHandler.instance.currentJob.abortTime.ToString(CultureInfo.InvariantCulture)
            };

            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Join(",", jobData));

            string pathName = Application.dataPath + "/Data/" +
                              SystemHandler.instance.currentJob.startTime.ToString("yy-MM-dd HH-mm-ss").Replace(" ", "_") +
                              "_" + SystemHandler.instance.currentJob.jobName + ".csv";

            string[] lines = File.ReadAllLines(pathName);
            lines[1] = string.Join(",", jobData);
            File.WriteAllLines(pathName, lines);
        }
    }
}