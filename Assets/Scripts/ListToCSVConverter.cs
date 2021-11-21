using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using UnityEngine;
public class ListToCSVConverter : MonoBehaviour
{
    private string path = "/Users/madsfinnerup/desktop/SampleCSVFile.csv";
    //private string path02 = Application.dataPath + "/Data/" + "Saved_HumanData.csv";

    public ListToCSVConverter () {}

    public void ListToCSV(List<People> humanList)
    {   
        Debug.Log("Writing to CSV");
        StreamWriter write = new StreamWriter(path);        

        write.WriteLine("x-Cordinates, y-Cordinates, timeLeft");

        foreach (People people in humanList)
        {
            write.WriteLine(people.XCords + "," + people.YCords + "," + people.TimeToFind);
        }
        write.Flush();
        write.Close();
    }
}