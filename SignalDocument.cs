using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Signals
{
    public class SignalDocument : Document
    {


        private List<SignalValue> signalValues = new List<SignalValue>(); 

   
        public IReadOnlyList<SignalValue> SignalValues  // IReadOnlyList of signal value objects so we make sure we only read from it 
        {
            get { return signalValues; }
        }

        private SignalValue[] testValues = new SignalValue[]
        {
            new SignalValue(10, new DateTime(2015, 1, 1, 0, 0, 0, 111)),
            new SignalValue(20, new DateTime(2015, 1, 1, 0, 0, 1, 876)),
            new SignalValue(30, new DateTime(2015, 1, 1, 0, 0, 2, 300)),
            new SignalValue(10, new DateTime(2015, 1, 1, 0, 0, 3, 232)),
            new SignalValue(-10, new DateTime(2015, 1, 1, 0, 0, 5, 885)),
            new SignalValue(-19, new DateTime(2015, 1, 1, 0, 0, 6, 125))
        };

        public SignalDocument(string name)
                : base(name)
        {
            // For the time being let’s initialize the list of signal values
            // with the testValues array
            signalValues.AddRange(testValues);
        }


        public override void SaveDocument(string filePath)
        {
           
            using (StreamWriter sw = new StreamWriter(filePath))
            
            {
                foreach (SignalValue sv in testValues)
            {
                    string dt = sv.TimeStamp.ToUniversalTime().ToString("o"); // change the time format to universal time 
                    sw.WriteLine(sv.Value +"\t"+ dt);  //store all the testValues in a file 
            }

            }

        }

        public override void LoadDocument(string filePath)
        {
            string line;
            signalValues.Clear();
            using (StreamReader sr = new StreamReader(filePath))   // using keyword to handle the file Exception 
            {
                while ((line = sr.ReadLine()) != null)   // we read line by line until the end of file 
                {
                    // line contains the actual line as a string
                   
                    line = line.Trim();      // remove leading and ending whitespaces from the line 
                    string[] columns = line.Split('\t');    // split the line into 2 parts ( Value , Time )
                    double d = double.Parse(columns[0]);   // convert the first part elements (Value) to double 
                    DateTime dt = DateTime.Parse(columns[1]); // convert the second part elements (Value) to DateTime
                    DateTime localDt = dt.ToLocalTime();  // convert to local time format 
                    SignalValue sv = new SignalValue(d, localDt); // create a new Signal value object 
                    signalValues.Add(sv);  // add it the list of signal values 
                    
                }

            }
            UpdateAllViews();  // view updating 
            TraceValues();   // shows the steps/output after making any move with the program 
        }

        void TraceValues()
        {
            foreach (SignalValue value in signalValues)
                Trace.WriteLine(value.ToString());

        }
    }
}
