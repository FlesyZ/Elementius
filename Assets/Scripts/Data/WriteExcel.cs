using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Data;
using OfficeOpenXml;
using Excel;
using UnityEngine;
using UnityEditor;

public class WriteExcel : MonoBehaviour
{
    public static List<string> list = new List<string>();

    public static string timer;

    public static void Write(string FileName, string Sheet, bool isVertical)
    {
        string path = Application.streamingAssetsPath + "/" + FileName + ".xlsx";
        FileInfo file = new FileInfo(path);

        using (ExcelPackage p = new ExcelPackage(file))
        {
            ExcelWorksheet sheet;
            sheet = p.Workbook.Worksheets[Sheet];

            float read = ReadCount(path, Sheet, isVertical);

            if (isVertical)
            {
                sheet.Cells[((char)(read + 65)).ToString() + 1].Value = timer;

                for (int i = 0; i < read; i++)
                {
                    sheet.Cells["B" + (i + 1)].Value = list[i];
                }
            }
            else
            {
                sheet.Cells["A" + (read + 1)].Value = timer;

                for (int i = 0; i < read; i++)
                {
                    sheet.Cells[((char)(i + 65)).ToString() + 2].Value = list[i];
                }
            }
            p.Save();
            list.Clear();
        }
    }

    static int ReadCount(string Path, string Sheet, bool isVertical)
    {
        FileStream stream = File.Open(Path, FileMode.Open, FileAccess.Read, FileShare.Read);
        IExcelDataReader read = ExcelReaderFactory.CreateOpenXmlReader(stream);
        DataSet result = read.AsDataSet();

        if (isVertical)
            return ReadRows(result, Sheet).Count;
        else
            return ReadColumns(result, Sheet).Count;
    }

    static DataRowCollection ReadRows(DataSet result, string Sheet)
    {
        return result.Tables[Sheet].Rows;
    }

    static DataColumnCollection ReadColumns(DataSet result, string Sheet)
    {
        return result.Tables[Sheet].Columns;
    }
}
