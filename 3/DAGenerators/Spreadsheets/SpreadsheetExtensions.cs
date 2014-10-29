using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using System.Collections.Generic;
using System.Linq;

namespace DAGenerators.Spreadsheets
{
    public static class SpreadsheetExtensions
    {
        public static void InsertRowZ(this ExcelWorksheet workSheet, int rowStart, int count, int copyStylesFromRow)
        {
            workSheet.InsertRow(rowStart, count, copyStylesFromRow);

            foreach (ExcelDrawing drawing in workSheet.Drawings)
            {
                if (drawing.From.Row >= rowStart)
                {
                    drawing.SetPosition(drawing.From.Row + count, drawing.From.RowOff, drawing.From.Column, drawing.From.ColumnOff);
                }
            }
        }

        // From: https://epplus.codeplex.com/workitem/13628
        public static void InsertRowX(this ExcelWorksheet workSheet, int rowStart, int count, int copyStylesFromRow)
        {
            workSheet.InsertRow(rowStart, count, copyStylesFromRow);

            // key: NamedRange, value: Is named range encompassing inserted rows
            var rangesToUpdate = new Dictionary<ExcelNamedRange, bool>();
            foreach (ExcelNamedRange range in workSheet.Names)
            {
                try
                {
                    if (range.End.Row >= rowStart)
                        rangesToUpdate.Add(range, range.Start.Row < rowStart);
                }
                catch
                {
                    /* Excel Defined Names may contain other refersTo values than ranges.
                     * We can skip those. */
                }
            }
            foreach (var range in rangesToUpdate)
            {
                workSheet.Names.Remove(range.Key.Name);
                ExcelRangeBase newRange;
                if (range.Value)
                    newRange = workSheet.Cells[range.Key.Start.Row, range.Key.Start.Column, range.Key.End.Row + count, range.Key.End.Column];
                else
                    newRange = range.Key.Offset(count, 0);

                var newNamedRange = workSheet.Names.Add(range.Key.Name, newRange);
                newNamedRange.NameComment = range.Key.NameComment;
                newNamedRange.IsNameHidden = range.Key.IsNameHidden;
            }
        }
        public static void InsertRowY(this ExcelWorksheet workSheet, int rowStart, int count, int copyStylesFromRow)
        {
            workSheet.InsertRow(rowStart, count, copyStylesFromRow);
            foreach (var name in workSheet.Workbook.Names)
            {
                var x = name;
            }

            var ng = (from item in workSheet.Workbook.Names
                      where item.Worksheet.Name.ToUpper() == workSheet.Name.ToUpper() &&
                            item.Address.ToUpper().Contains(workSheet.Name.ToUpper()) &&
                            item.Start.Row >= rowStart
                      select item).ToList();

            for (int i = 0; i < ng.Count(); i++)
            {
                workSheet.Workbook.Names.Remove(ng[i].Name);
                var newitem = ng[i].Offset(count, 0);
                workSheet.Workbook.Names.Add(ng[i].Name, newitem);
            }
        }
    }
}
