using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialMaker.Logic
{
    public class Memory
    {
        public readonly string DirectoryPath;

        public Memory(string baseUrl)
        {
            DirectoryPath = baseUrl;
        }

        public string ReadFile(string filename)
        {   
            string text = "";
            Task.Run(() =>
            {
                string filePath = DirectoryPath + filename;

                FileStream stream = File.Open(filePath, FileMode.OpenOrCreate, FileAccess.Read);
                
                using (StreamReader reader = new StreamReader(stream))
                {
                    text = reader.ReadToEnd();
                    reader.Dispose();
                }
                stream.Dispose();
            }).Wait();
            

            
            return text;
        }

        public void WriteFile(string v, string obj)
        {
            File.WriteAllText(DirectoryPath + v, obj);
        }
    }
}