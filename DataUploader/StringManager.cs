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

        public List<string[]> CreateDataList()
        {
            List<string[]> dataList = new List<string[]>();

            string s;
            while ((s = streamReader.ReadLine()) != null)
            {
                string[] split = s.Split(new Char[] { ' ' });
                dataList.Add(split);
            }

            return dataList;
        }
    }
}
