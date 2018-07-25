using System.IO;

namespace mlForm
{
    class GetColumns
    {
        public string[] getColumns()
        {
            FileStream fs = new FileStream(@"iris-data.txt", FileMode.Open, FileAccess.Read);
            StreamReader sw = new StreamReader(fs);
            string line1 = sw.ReadLine();
            string[] allLines = line1.Split(',');
            string[] a = new string[allLines.Length - 1];
            for (int i = 0; i < allLines.Length-1; i++)
            {
                a[i] = allLines[i];
            }
            return a;
        }
    }
}
