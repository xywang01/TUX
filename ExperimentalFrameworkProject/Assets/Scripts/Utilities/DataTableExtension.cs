﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public static class DataTableExtension
{

    const string TabSeparator = "\t";
    const string CommaSeparator = ", ";
    const int TruncateDefault = 10;

    public static void PrintToConsole(this DataTable dt) {
        Debug.Log(dt.AsString());
    }

    

    public static string AsString(this DataTable dt, string separator = TabSeparator, int truncate = TruncateDefault) {
        string headerString = HeaderAsString(dt, separator, truncate);

        string tableString = string.Join(Environment.NewLine,
                                 dt.Rows.OfType<DataRow>().Select(x => string.Join(separator, x.ItemArray)));
        return headerString + "\n" + tableString;
    }

    public static string HeaderAsString(this DataTable dt, string separator = TabSeparator, int truncate = TruncateDefault) {
        string headerString = string.Join(separator, truncate < 0 ? 
                                       dt.Columns.OfType<DataColumn>().Select(x => string.Join(separator, x.ColumnName)) : 
                                       dt.Columns.OfType<DataColumn>().Select(x => string.Join(separator, x.ColumnName.Truncate(truncate))));
        return headerString;
    }

    public static string AsString(this DataRow row, string separator = TabSeparator, int truncate = TruncateDefault) {
        string rowString  = string.Join(separator, row.ItemArray.Select(c => c.ToString().Truncate(truncate)).ToArray());
        return rowString;
    }

    public static string AsStringWithHeader(this DataRow row, DataTable dt, string separator = TabSeparator, int truncate = TruncateDefault) {
        string headerString = dt.HeaderAsString();
        string rowString = string.Join(separator, row.ItemArray.Select(c => c.ToString().Truncate(truncate)).ToArray());
        return headerString + "\n" + rowString;
    }

    /// <summary>
    /// Randomly shuffles the item order of this list
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    public static DataTable Shuffle(this DataTable table) {
        int n = table.Rows.Count;
        List<DataRow> shuffledRows = new List<DataRow>();
        foreach (DataRow row in table.Rows) {
            shuffledRows.Add(row);
        }
        
        while (n > 1) {
            n--;
            int k = Random.Range(0, n + 1);
            DataRow value = shuffledRows[k];
            shuffledRows[k] = shuffledRows[n];
            shuffledRows[n] = value;
        }

        DataTable shuffledTable = table.Clone();
        foreach (DataRow row in shuffledRows) {
            shuffledTable.ImportRow(row);
        }

        return shuffledTable;
    }

    public static string FormattedForCSV(this DataTable dt) {
        return dt.AsString(CommaSeparator, truncate:-1);
    }

}
