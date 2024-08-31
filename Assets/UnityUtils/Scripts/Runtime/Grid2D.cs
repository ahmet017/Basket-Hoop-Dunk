using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityUtils
{
    public class Grid2D
    {
        private int rowCount;
        private int columnCount;
        private float cellWidth;
        private float cellHeight;
        private float gridWidth;
        private float gridHeight;
        private float margins;
        private Vector2 center;
        public int RowCount { get { return rowCount; } }
        public int ColumnCount { get { return columnCount; } }
        public float CellWidth { get { return cellWidth; } }
        public float CellHeight { get { return cellHeight; } }
        public Vector2 Center { get { return center; } }


        public void CreateGrid(int rowCount, int columnCount, float gridWidth, float gridHeight, float margins, Vector2 center)
        {
            this.rowCount = rowCount;
            this.columnCount = columnCount;
            this.gridWidth = gridWidth;
            this.gridHeight = gridHeight;
            this.center = center;
            this.margins = margins;

            cellWidth = (gridWidth - (columnCount + 1) * margins) / columnCount;
            cellHeight = (gridHeight - (rowCount + 1) * margins) / rowCount;
        }

        public Vector2 GetWorldPosition(int row, int column)
        {
            return Vector2.right * (center.x + column * (cellWidth + margins) - gridWidth * 0.5f + cellWidth * 0.5f + margins)
                + Vector2.up * (center.y + row * (cellHeight + margins) - gridHeight * 0.5f + cellHeight * 0.5f + margins);
        }
    }

}