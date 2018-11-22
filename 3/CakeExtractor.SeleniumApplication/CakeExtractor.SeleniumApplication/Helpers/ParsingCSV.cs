using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;

namespace AmazonAdvertisingNavigationTest
{
    public class ParsingCSV
    {
        private string _file { get; set; }
        
        public ParsingCSV(string pathToFile)
        {
            _file = pathToFile;
        }
        
        public List<Dictionary<string, string>> Parse(char delimiter)
        {
            try
            {
                using (var textReader = new StreamReader(_file))
                {
                    var line = textReader.ReadLine();
                    var headerLine = line.Split(delimiter);
                    var skipCount = 0;
                    while (line != null && skipCount < 1)
                    {
                        line = textReader.ReadLine();
                        skipCount++;
                    }
                    var campaignList = new List<Dictionary<string, string>>();
                    while (line != null)
                    {
                        string[] columns = line.Split(delimiter);
                        var campaignItem = new Dictionary<string, string>();
                        for (var i = 0; i <= columns.Length - 1; i++)
                        {
                            campaignItem.Add(headerLine[i], columns[i]);
                        }
                        campaignList.Add(campaignItem);
                        line = textReader.ReadLine();
                    }
                    return campaignList;
                }
            }
            catch (Exception e)
            {
                throw new Exception($"The parse process failed [{_file}]: {e.Message}", e);
            }
        }
    }
}
