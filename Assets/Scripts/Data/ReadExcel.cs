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

public class ReadExcel : MonoBehaviour
{
    /// <summary>
    /// 讀取Excel活頁簿檔案的內容
    /// </summary>
    /// <param name="FileName">活頁簿名稱</param>
    /// <param name="SheetName">工作表名稱</param>
    /// <param name="column">指定欄, 輸入-1為讀取所有欄</param>
    /// <param name="row">指定列, 輸入-1為讀取所有列</param>
    public static List<string> Read(string FileName, string SheetName, int column, int row)
    {
        List<string> ReadData = new List<string>();

        string path = Application.streamingAssetsPath + "/" + FileName + ".xlsx";
        FileStream stream = File.Open(path, FileMode.Open, FileAccess.Read);
        IExcelDataReader read = ExcelReaderFactory.CreateOpenXmlReader(stream);
        DataSet result = read.AsDataSet();

        // catch its rows and values
        int columns, rows;
        
        if (column == -1)
            columns = result.Tables[SheetName].Columns.Count;
        else
            columns = column + 1;
        if (row == -1)
            rows = result.Tables[SheetName].Rows.Count;
        else
            rows = row + 1;

        // write them on the list
        for (int i = Mathf.Clamp(row, 0, row); i < rows; i++)
        {
            for (int j = Mathf.Clamp(column, 0, column); j < columns; j++)
            {
                string data;
                if (result.Tables[SheetName].Rows[i][j] != null)
                {
                    data = result.Tables[SheetName].Rows[i][j].ToString();
                    ReadData.Add(data);
                }
                
            }
        }
        return ReadData;
    }

}
