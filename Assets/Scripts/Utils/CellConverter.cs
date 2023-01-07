using System;
using System.Collections.Generic;
using LevelManipulation;
using Widgets;

namespace Utils
{
    public static class CellConverter
    {
        public static List<List<CellWidget>> GenerateCellWidgets(List<List<CellInfo>> cellInfos, List<List<BoardItemWidget>> items)
        {
            List<List<CellWidget>> list = new List<List<CellWidget>>();

            if (cellInfos.Count != items.Count || cellInfos[0].Count != items[0].Count)
                throw new ArgumentException("items and cells asynchronous");

            for (int i = 0; i < cellInfos.Count; i++)
            {
                var row = new List<CellWidget>();
                for (int j = 0; j < cellInfos[0].Count; j++)
                {
                    var cellWidget = new CellWidget(items[i][j], cellInfos[i][j]);
                    row.Add(cellWidget);
                }

                list.Add(row);
            }

            return list;
        }

        public static List<List<BoardItemWidget>> GetItemWidgets(List<List<CellWidget>> cells)
        {
            List<List<BoardItemWidget>> result = new List<List<BoardItemWidget>>();

            foreach (var row in cells)
            {
                List<BoardItemWidget> rowAdd = new List<BoardItemWidget>();
                foreach (var cell in row)
                {
                    rowAdd.Add(cell.BoardItem);
                }
                
                result.Add(rowAdd);
            }

            return result;
        }
    }
}