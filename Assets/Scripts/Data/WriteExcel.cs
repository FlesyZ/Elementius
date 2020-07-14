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

    public static void Write(string FileName, string Sheet, bool isVertical)
    {
        if (isVertical)
            Write(FileName, Sheet, 0, 1, isVertical, 1, 0);
        else
            Write(FileName, Sheet, 1, 0, isVertical, 1, 0);
    }

    public static void Write(string FileName, string Sheet, int StartColumn, int StartRow, bool isVertical)
    {
        Write(FileName, Sheet, StartColumn, StartRow, isVertical, 1, 0);
    }

    /// <summary>
    /// 寫入活頁簿
    /// </summary>
    /// <param name="FileName">活頁簿名稱</param>
    /// <param name="Sheet">工作表名稱</param>
    /// <param name="StartColumn">開始位置(欄)</param>
    /// <param name="StartRow">開始位置(列)</param>
    /// <param name="isVertical">是否以縱向寫入</param>
    /// <param name="axis">二維代數</param>
    /// <param name="write">寫入數量, 輸入0將以該讀取的指定長度為基準, 輸入負數則以讀取數量為基準</param>
    public static void Write(string FileName, string Sheet, int StartColumn, int StartRow, bool isVertical, int axis, int write)
    {
        string path = Application.streamingAssetsPath + "/" + FileName + ".xlsx";
        FileInfo file = new FileInfo(path);

        using (ExcelPackage p = new ExcelPackage(file))
        {
            ExcelWorksheet sheet;
            sheet = p.Workbook.Worksheets[Sheet];

            float read = (write == 0) ? ReadCount(path, Sheet, isVertical) : (write < 0) ? list.Count : write;

            int n = 0;
            while (n < read)
                for (int i = 1; i <= list.Count; i++)
                {
                    for (int j = 1; j <= axis; j++)
                    {
                        if (isVertical)
                            sheet.Cells[Column(StartColumn + j - 1) + (StartRow + i)].Value = list[n];
                        else
                            sheet.Cells[Column(StartColumn + i - 1) + (StartRow + j)].Value = list[n];
                        n++;
                        if (n >= read) break;
                    }
                    if (n >= read) break;
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

    static string Column(int i)
    {
        string x = "";
        int[] letter = { -1, -1, -1 };
        char?[] vs = new char?[3];

        letter[0] = i % 26;
        if (i / 26 > 0) letter[1] = i / 26 % 26;
        if (i / 26 / 26 > 0) letter[2] = i / 26 / 26;

        for (int j = letter.Length - 1; j >= 0; j--) 
            if (letter[j] >= 0) vs[j] = (char)(letter[j] + 65);
        
        foreach (var item in vs) 
            if (item != null) x += item.ToString();

        return x;
    }
}
