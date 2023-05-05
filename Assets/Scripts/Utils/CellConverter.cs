using System;
using System.Collections.Generic;
using LevelManipulation;
using Widgets;
using Widgets.BoardWidgets;

namespace Utils
{
    public static class CellConverter
    {
        public static List<List<Cell>> GenerateCell(List<List<CellInfo>> cellInfos, List<List<BoardItemWidget>> items)
        {
            var list = new List<List<Cell>>();

            if (cellInfos.Count != items.Count || cellInfos[0].Count != items[0].Count)
                throw new ArgumentException("items and cells asynchronous");

            for (int i = 0; i < cellInfos.Count; i++)
            {
                var row = new List<Cell>();
                for (int j = 0; j < cellInfos[0].Count; j++)
                {
                    var cell = new Cell(items[i][j], cellInfos[i][j]);
                    row.Add(cell);
                }

                list.Add(row);
            }

            return list;
        }

        public static List<List<BoardItemWidget>> GetItemWidgets(List<List<Cell>> cells)
        {
            var result = new List<List<BoardItemWidget>>();

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