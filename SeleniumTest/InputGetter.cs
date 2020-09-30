using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SeleniumTest
{
    class Input
    {
        public static List<InputInfo> GetInputInfos()
        {
            try
            {
                string[] data = File.ReadAllLines(Properties.Resources.input);
                var listInfos = new List<InputInfo>();

                for(int i = 0; i < data.Length; i++)
                {
                    var currentLine = data[i].Trim();
                    var splitedData = currentLine.Split('\t');

                    if (splitedData.Length < 3) throw new ArgumentOutOfRangeException();

                    InputInfo inputInfo = new InputInfo(splitedData[0], splitedData[1], splitedData[2]);
                    listInfos.Add(inputInfo);
                }

                return listInfos;


            }catch(FileNotFoundException e)
            {
                MessageBox.Show("Cannot read input file, please check again!","error reading input");
                
            }catch(ArgumentOutOfRangeException e)
            {
                MessageBox.Show("Some line of input file is not valid, please check again!");
            }

            Application.Exit();
            return null;
        }
    }
}
