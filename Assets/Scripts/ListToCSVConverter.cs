using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using UnityEngine;

public class ListToCSVConverter : MonoBehaviour
{
    string path = "/Users/madsfinnerup/desktop";
    string path02 = Application.dataPath + "/Data/"  + "Saved_HumanData.csv";
    public void ListToCSV(List<People> humanList)
    {        
        StreamWriter write = new StreamWriter(path02);

        write.WriteLine("x-Cordinates, y-Cordinates, timeLeft");

        foreach (People people in humanList) {
            write.WriteLine(people.XCords + "," + people.YCords + "," + people.TimeToFind);
            writer.Write(System.Environment.NewLine);
        }
        write.Flush();
        write.Close();
    }

}
