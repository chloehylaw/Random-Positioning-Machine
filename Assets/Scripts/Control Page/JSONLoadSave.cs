using System;
using System.IO;
using System.Linq;
using Date_Page;
using TMPro;
using UnityEngine;
namespace Control_Page
{
    public class JSONLoadSave : MonoBehaviour
    {
        public EndDateRow endDateRow;
        public JobRow jobRow;
        public TimeRow timeRow;
        public GravityRow gravityRow;
    
        private enum JobStatus {Running, Completed, Inactive}

        private DateTime currentTime;
    
        // home page info
        public TMP_Text jobText;
        public TMP_Text statusText;
        public TMP_Text elapsedTimeText;
        public TMP_Text remainingTimeText;

        public GameObject jobName;
        public GameObject currentStatus;
        public GameObject elapsedTimeStatus;
        public GameObject remainingTimeStatus;

        [SerializeField] private TMP_InputField inputJobName;
        [SerializeField] private TMP_InputField inputTimeDay;
        [SerializeField] private TMP_InputField inputTimeHour;
        [SerializeField] private TMP_InputField inputTimeMin;
        [SerializeField] private TMP_InputField inputTimeSec;
        [SerializeField] private TMP_InputField inputGravityValue;
        [SerializeField] private TMP_Dropdown inputGravityUnit;
        [SerializeField] private TMP_Dropdown inputRotationalAlgorithm;

        [Serializable]
        public class Job
        {
            public string guid;
            public string jobName;
            public int timeDay;
            public int timeHour;
            public int timeMin;
            public int timeSec;
            public string gravityValue;
            public string gravityUnit;
            public string rotationalAlgorithm;
            public string status;
            public string saveTime;
            public string startTime;
            public string endTime;
        }

        [Serializable]
        public class JobList
        {
            public Job[] job;
        }

        public JobList myJobList = new JobList();
        public Job myJob = new Job();
    
        // Start is called before the first frame update
        void Start()
        {
            // find if the data.json file exists
            if (File.Exists(Application.dataPath + "/Data/data.json"))
            {
                string str = File.ReadAllText(Application.dataPath + "/Data/data.json");
                myJobList = JsonUtility.FromJson<JobList>(str);
            }
        
            // home page info
            jobText = jobName.GetComponent<TMP_Text>();
            jobText.text = "";
        
            statusText = currentStatus.GetComponent<TMP_Text>();
            statusText.text = "";
        
            elapsedTimeText = elapsedTimeStatus.GetComponent<TMP_Text>();
            elapsedTimeText.text = "";
        
            remainingTimeText = remainingTimeStatus.GetComponent<TMP_Text>();
            remainingTimeText.text = "";
        }

        void Update ()
        {
            currentTime = DateTime.Now;

            // find if there are any running jobs
            var runningJob = myJobList.job.FirstOrDefault(j => j.status == JobStatus.Running.ToString());

            if (runningJob == null) return;
            {
                var runningJobIndex = Array.FindIndex(myJobList.job, j => j.status == JobStatus.Running.ToString());
                DateTime startTime = Convert.ToDateTime(myJobList.job[runningJobIndex].startTime);
                Debug.Log(myJobList.job[runningJobIndex].endTime);

                DateTime endTime = Convert.ToDateTime(myJobList.job[runningJobIndex].endTime);

                jobText.text = myJobList.job[runningJobIndex].jobName;
                statusText.text = myJobList.job[runningJobIndex].status;
                elapsedTimeText.text = currentTime.Subtract(startTime).ToString();
                remainingTimeText.text = endTime.Subtract(currentTime).ToString();
            }
        }

        private bool ValidateJobName ()
        {
            // check if empty
            if (String.IsNullOrEmpty(inputJobName.text))
            {
                jobRow.WarningMessage("String is empty.");
                return false;
            }
        
            // check if name already exists
            if (myJobList.job.Any(j => j.jobName == inputJobName.text))
            {
                jobRow.WarningMessage("Job name already exists.");
                return false;
            }
            return true;
        }

        private bool ValidateTime ()
        {
            string day = inputTimeDay.text;
            string hour = inputTimeHour.text;
            string min = inputTimeMin.text;
            string sec = inputTimeSec.text;

            bool anyNotEmpty = !String.IsNullOrEmpty(day) || !String.IsNullOrEmpty(hour) || !String.IsNullOrEmpty(min) || !String.IsNullOrEmpty(sec);
        
            // if all inputs are empty
            if (!anyNotEmpty)
            {
                timeRow.WarningMessage("Empty time input.");
                return false;
            }
        
            // check if at least one is not 0
            bool anyNotZero = day != "0" || hour != "0" || min != "0" || sec != "0";
        
            // if empty or negative, set to 0
            if (string.IsNullOrEmpty(day) || day.Contains("-") || !int.TryParse(day, out _))
            {
                day = "0";
                timeRow.WarningMessage("Invalid day.");
                return false;
            }
            

            if (string.IsNullOrEmpty(hour) || hour.Contains("-") || !int.TryParse(hour, out _))
            {
                hour = "0";
                timeRow.WarningMessage("Invalid day.");
                return false;
            }

            if (string.IsNullOrEmpty(min) || min.Contains("-") || !int.TryParse(min, out _))
            {
                min = "0";
                timeRow.WarningMessage("Invalid day.");
                return false;
            }

            if (string.IsNullOrEmpty(sec) || sec.Contains("-") || !int.TryParse(sec, out _))
            {
                sec = "0";
                timeRow.WarningMessage("Invalid day.");
                return false;
            }
        
            return anyNotZero;
        }

        private bool ValidateGravityInput ()
        {
            string gravity = inputGravityValue.text;

            if (string.IsNullOrEmpty(gravity))
            {
                gravityRow.WarningMessage("Empty gravity input.");
                return false;
            }

            if (double.TryParse(gravity, out var number)) return number >= 0;
            gravityRow.WarningMessage("Invalid gravity input.");
            return false;

        }

        public void ValidateForm ()
        {
            if (!ValidateJobName() || !ValidateTime() || !ValidateGravityInput())
            {
                AddRecord();
            } else
            {
                Debug.Log("Refill form");
            }
        }

        // cant make it a list rip so array it is
        public void AddRecord ()
        {
            // collect data from input fields
            Job inputJob = new Job
            {
                guid = Guid.NewGuid().ToString(),
                jobName = inputJobName.text,
                timeDay = int.Parse(inputTimeDay.text),
                timeHour = int.Parse(inputTimeHour.text),
                timeMin = int.Parse(inputTimeMin.text),
                timeSec = int.Parse(inputTimeSec.text),
                gravityValue = inputGravityValue.text,
                gravityUnit = inputGravityUnit.options[inputGravityUnit.value].text,
                rotationalAlgorithm = inputRotationalAlgorithm.options[inputRotationalAlgorithm.value].text,
                status = JobStatus.Inactive.ToString(),
                saveTime = DateTime.Now.ToString(),
                startTime = "",
                endTime = endDateRow.endDate.text
            };

            // make temp array to hold old data and copy data into temp array
            JobList myJobListTemp = new JobList
            {
                job = myJobList.job
            };
        
            // make the original longer
            myJobList.job = new Job[myJobListTemp.job.Length + 1];

            // copy the old data back across
            for (int i = 0; i < myJobListTemp.job.Length; i++)
            {
                myJobList.job[i] = myJobListTemp.job[i];
            }
        
            // add new data
            myJobList.job[myJobListTemp.job.Length] = inputJob;
        
            // write data
            OutputJson();
        }

        void OutputJson ()
        {
            string str = JsonUtility.ToJson(myJobList,true);
            File.WriteAllText(Application.dataPath + "/Data/data.json", str);
        }
    
        public void StartJob ()
        {
            // find job by guid 
            var job = myJobList.job.FirstOrDefault(j => j.guid == myJob.guid);

            // if guid doesn't exist
            if (job == null)
            {
                // add job to record
                AddRecord();
            
                // change the status of other "Running" jobs to "Inactive"
                foreach (var otherJob in myJobList.job.Where(j => j.status == JobStatus.Running.ToString()))
                {
                    otherJob.status = JobStatus.Inactive.ToString();
                }
            
                // change the status of current job to "Running" and edit start time
                var currentJobIndex = Array.FindIndex(myJobList.job, j => j.guid == myJob.guid);
                myJobList.job[currentJobIndex].status = JobStatus.Running.ToString();
                myJobList.job[currentJobIndex].startTime = currentTime.ToString();
            } 
        
            // change status to "Running" and edit start time
            if (job != null)
            {
                job.status = JobStatus.Running.ToString();
                job.startTime = currentTime.ToString();
            };
        }
    }
}