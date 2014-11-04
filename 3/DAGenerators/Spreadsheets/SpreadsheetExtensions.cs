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

            UpdateRangesForRowInsert(workSheet, rowStart, count);
            UpdateDrawingsForRowInsertDelete(workSheet, rowStart, count);
        }

        public static void DeleteRowZ(this ExcelWorksheet workSheet, int rowFrom, int rows)
        {
            workSheet.DeleteRow(rowFrom, rows);

            //UpdateRangesForRowInsertDelete(workSheet, rowFrom, -rows); // TODO
            UpdateDrawingsForRowInsertDelete(workSheet, rowFrom, -rows);
        }

        private static void UpdateDrawingsForRowInsertDelete(this ExcelWorksheet workSheet, int rowStart, int count)
        {
            foreach (ExcelDrawing drawing in workSheet.Drawings)
            {
                if (drawing.From.Row + 1 >= rowStart) // From.Row is 0-based, so add one for the comparison
                {
                    drawing.SetPosition(drawing.From.Row + count, drawing.From.RowOff, drawing.From.Column, drawing.From.ColumnOff);
                    // NOTE: doesn't work well when RowOff or ColumnOff are not zero
                }
            }
        }

        // TODO: also update ranges for RowDelete (when count is negative)
        // From: https://epplus.codeplex.com/workitem/13628
        private static void UpdateRangesForRowInsert(this ExcelWorksheet workSheet, int rowStart, int count)
        {
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
