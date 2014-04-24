using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace DAgents.Common
{
    public class RowsConverter<T>
    {
        public List<RowWithObject<T>> Rows = new List<RowWithObject<T>>();
        public Dictionary<string, int> ColumnDictionary = new Dictionary<string, int>();
        public bool ImportComplete { get; set; }
        public bool AnyErrors
        {
            get { return Rows.Any(r => r.Errors.Count > 0); }
        }

        public RowsConverter(List<string> headers)
        {
            for (int i = 0; i < headers.Count; i++)
            {
                var header = headers[i].ToLower().Trim();
                if (!ColumnDictionary.ContainsKey(header))
                    ColumnDictionary.Add(header, i);
            }
        }

        public RowWithObject<T> NewRow(List<string> data = null)
        {
            var row = new RowWithObject<T>(this, data);
            Rows.Add(row);
            return row;
        }
    }

    public class RowWithObject<T>
    {
        public RowsConverter<T> Parent { get; set; }
        public List<string> Data { get; set; }
        public T Object { get; set; }
        public List<string> Errors = new List<string>();

        public RowWithObject(RowsConverter<T> parent, List<string> data = null)
        {
            Parent = parent;
            if (data == null)
                data = new List<string>();
            Data = data;
        }

        public string DataFor(string column)
        {
            if (!Parent.ColumnDictionary.ContainsKey(column))
                return null;

            int colIndex = Parent.ColumnDictionary[column];
            if (colIndex >= Data.Count)
                return null;

            return Data[colIndex];
        }
    }


    // From: http://www.codeproject.com/Articles/670141/Read-and-Write-Microsoft-Excel-with-Open-XML-SDK
    public class SLExcelStatus
    {
        public string Message { get; set; }
        public bool Success
        {
            get { return string.IsNullOrWhiteSpace(Message); }
        }
    }

    public class SLExcelData
    {
        public SLExcelStatus Status { get; set; }
        public Columns ColumnConfigurations { get; set; }
        public List<string> Headers { get; set; }
        public List<List<string>> DataRows { get; set; }
        public string SheetName { get; set; }

        public SLExcelData()
        {
            Status = new SLExcelStatus();
            Headers = new List<string>();
            DataRows = new List<List<string>>();
        }
    }

    public static class SLExcelReader
    {
        private static string GetColumnName(string cellReference)
        {
            var regex = new Regex("[A-Za-z]+");
            var match = regex.Match(cellReference);

            return match.Value;
        }

        private static int ConvertColumnNameToNumber(string columnName)
        {
            var alpha = new Regex("^[A-Z]+$");
            if (!alpha.IsMatch(columnName)) throw new ArgumentException();

            char[] colLetters = columnName.ToCharArray();
            Array.Reverse(colLetters);

            var convertedValue = 0;
            for (int i = 0; i < colLetters.Length; i++)
            {
                char letter = colLetters[i];
                // ASCII 'A' = 65
                int current = i == 0 ? letter - 65 : letter - 64;
                convertedValue += current * (int)Math.Pow(26, i);
            }

            return convertedValue;
        }

        private static IEnumerator<Cell> GetExcelCellEnumerator(Row row)
        {
            int currentCount = 0;
            foreach (Cell cell in row.Descendants<Cell>())
            {
                string columnName = GetColumnName(cell.CellReference);

                int currentColumnIndex = ConvertColumnNameToNumber(columnName);

                for (; currentCount < currentColumnIndex; currentCount++)
                {
                    var emptycell = new Cell()
                    {
                        DataType = null,
                        CellValue = new CellValue(string.Empty)
                    };
                    yield return emptycell;
                }

                yield return cell;
                currentCount++;
            }
        }

        private static string ReadExcelCell(Cell cell, WorkbookPart workbookPart)
        {
            var cellValue = cell.CellValue;
            var text = (cellValue == null) ? cell.InnerText : cellValue.Text;
            if ((cell.DataType != null) && (cell.DataType == CellValues.SharedString))
            {
                text = workbookPart.SharedStringTablePart.SharedStringTable
                    .Elements<SharedStringItem>().ElementAt(
                        Convert.ToInt32(cell.CellValue.Text)).InnerText;
            }
            else if (cell.StyleIndex != null)
            {
                var cellFormat = workbookPart.WorkbookStylesPart.Stylesheet.CellFormats.Elements<CellFormat>().ElementAt(Convert.ToInt32(cell.StyleIndex.Value));
                if (cellFormat.NumberFormatId >= 14 && cellFormat.NumberFormatId <= 22)
                {
                    double doubleVal;
                    if (Double.TryParse(text, out doubleVal))
                    {
                        var date = DateTime.FromOADate(doubleVal);
                        text = date.ToShortDateString();
                    }
                }
            }

            return (text ?? string.Empty).Trim();
        }

        //public SLExcelData ReadExcel(HttpPostedFileBase file)
        //{
        //    var data = new SLExcelData();

        //    // Check if the file is excel
        //    if (file.ContentLength <= 0)
        //    {
        //        data.Status.Message = "You uploaded an empty file";
        //        return data;
        //    }

        //    if (file.ContentType
        //        != "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
        //    {
        //        data.Status.Message
        //            = "Please upload a valid excel file of version 2007 and above";
        //        return data;
        //    }

        //    return ReadExcel(file.InputStream);
        //}

        public static SLExcelData ReadExcel(Stream stream, string path = null)
        {
            var data = new SLExcelData();

            if (stream == null && path == null)
            {
                data.Status.Message = "No stream or path supplied";
                return data;
            }

            // Open the excel document
            WorkbookPart workbookPart; List<Row> rows;
            SpreadsheetDocument document;
            try
            {
                if (stream == null)
                    document = SpreadsheetDocument.Open(path, false);
                else
                    document = SpreadsheetDocument.Open(stream, false);

                workbookPart = document.WorkbookPart;

                var sheets = workbookPart.Workbook.Descendants<Sheet>();
                var sheet = sheets.First();
                data.SheetName = sheet.Name;

                var workSheet = ((WorksheetPart)workbookPart
                    .GetPartById(sheet.Id)).Worksheet;
                var columns = workSheet.Descendants<Columns>().FirstOrDefault();
                data.ColumnConfigurations = columns;

                var sheetData = workSheet.Elements<SheetData>().First();
                rows = sheetData.Elements<Row>().ToList();
            }
            catch (Exception e)
            {
                data.Status.Message = "Unable to open the file";
                return data;
            }

            // Read the header
            if (rows.Count > 0)
            {
                var row = rows[0];
                var cellEnumerator = GetExcelCellEnumerator(row);
                while (cellEnumerator.MoveNext())
                {
                    var cell = cellEnumerator.Current;
                    var text = ReadExcelCell(cell, workbookPart).Trim();
                    data.Headers.Add(text);
                }
            }

            // Read the sheet data
            if (rows.Count > 1)
            {
                for (var i = 1; i < rows.Count; i++)
                {
                    var dataRow = new List<string>();
                    data.DataRows.Add(dataRow);
                    var row = rows[i];
                    var cellEnumerator = GetExcelCellEnumerator(row);
                    while (cellEnumerator.MoveNext())
                    {
                        var cell = cellEnumerator.Current;
                        var text = ReadExcelCell(cell, workbookPart).Trim();
                        dataRow.Add(text);
                    }
                }
            }
            document.Close();

            return data;
        }
    }
}
