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

        public TimeRow timeRow;
        public JobRow jobRow;
        public GravityRow gravityRow;
        public AlgorithmRow algorithmRow;

        // control page info
        [SerializeField] private TMP_Dropdown inputRotationalAlgorithm;

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
    }
}
