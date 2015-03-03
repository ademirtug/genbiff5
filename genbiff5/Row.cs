using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ademirtug.biff5
{
    public class Row
    {
        Dictionary<ushort, Cell> _cells = new Dictionary<ushort, Cell>();
		ushort rownum = 0;

		public Row(ushort _rownum)
		{
			rownum = --_rownum;
		}

		public void AddCell(ushort col, object value)
		{
			_cells[col] = new Cell(rownum, col, value);
		}		

		public Cell this[ushort index]
        {
            get 
            {
                return _cells[index];
            }
            set 
            {
                _cells[index] = value;
            }
        }


		public void marshall(BinaryWriter wr)
		{
			foreach (Cell c in _cells.Values)
				c.marshall(wr);
		}

    }
}
