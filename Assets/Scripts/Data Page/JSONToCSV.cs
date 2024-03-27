using UnityEngine;
using System;
using System.IO;
using System.Data;
using Newtonsoft.Json;
using System.Linq;
using System.Text;

namespace Date_Page
{
	public class JSONToCSV : MonoBehaviour
	{
		string jsonFilePath = "";
		string csvFilePath = "";
		
		void Start ()
		{
			
		}

		public void convertJSONToCSV ()
		{
			// find if the data.json file exists
			if (File.Exists(Application.dataPath + "/Data/data.json"))
			{
				jsonFilePath = File.ReadAllText(Application.dataPath + "/Data/data.json");
				Debug.Log(jsonFilePath);
			}
			
			// find if the data.csv file exists
			if (File.Exists(Application.dataPath + "/Data/data.csv"))
			{
				csvFilePath = File.ReadAllText(Application.dataPath + "/Data/data.csv");
				// clear csv file 
				File.WriteAllText(csvFilePath, string.Empty);
			}
			
			string json = File.ReadAllText(jsonFilePath);
			var table = JsonConvert.DeserializeObject<DataTable>(json);

			StringBuilder sb = new StringBuilder();
			
			var columnNames = table.Columns.Cast<DataColumn>().Select(column => column.ColumnName);
			sb.AppendLine(string.Join(",", columnNames));

			foreach (DataRow row in table.Rows)
			{
				var fields = row.ItemArray.Select(field => field);
				sb.AppendLine(string.Join(",", fields));
			}
			File.WriteAllText(csvFilePath, sb.ToString());
		}
	}
}
