using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace DataUploader
{
    class StringManager
    {
        StreamReader streamReader;
        public StringManager(StreamReader streamReader)
        {
            this.streamReader = streamReader;
        }

        public List<DataStruct> DataStructList()
        {
            List<DataStruct> dataStructs = new List<DataStruct>();

            string s;
            while ((s = streamReader.ReadLine()) != null)
            {
                string[] split = s.Split(new Char[] { ' ' });
                DataStruct dataStruct = new DataStruct();
                dataStruct.dateTime = DateTime.Parse(split[0]);
                dataStruct.acceleration = float.Parse(split[1]);
                dataStructs.Add(dataStruct);
            }

            return dataStructs;
        }
    }
}
