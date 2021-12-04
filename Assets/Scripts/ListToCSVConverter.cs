using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using UnityEngine;
using System;

public class ListToCSVConverter : MonoBehaviour
{
	private string fileName = $@"results{DateTime.Now.ToString("MMddHHmmss")}.csv";

	private string path = "results";
    //private string path02 = Application.dataPath + "/Data/" + "Saved_HumanData.csv";

    public ListToCSVConverter () {}
    private TimeSpan timeDifferent;

    public void ListToCSV(List<People> humanList)
    {   
        Debug.Log("Writing to CSV");
		if (!Directory.Exists("somedir"))
			Directory.CreateDirectory(path);

		StreamWriter write = File.AppendText(path + "\\" + fileName);        

        write.WriteLine("x-Cordinates, y-Cordinates, time left, found_time");

        foreach (People people in humanList)
        {
            timeDifferent = (people.TimeWhenFound - DroneController.currentStartTime);
            timeDifferent= TimeSpan.FromSeconds(timeDifferent.TotalSeconds*people.time_scale);
            write.WriteLine(people.XCords + "," + people.YCords + "," + people.TimeToFind + "," + timeDifferent);
        }
        write.Flush();
        write.Close();
    }
}