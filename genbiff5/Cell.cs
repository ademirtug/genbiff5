using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ademirtug.biff5
{
    public class Cell
    {
		cell_storage storage;
		
		public Cell(ushort _row, ushort col, object value)
		{
			if (value is int || value is short || value is ushort || value is uint)
				storage = new int_storage(_row, --col, Convert.ToInt32(value));
			else if (value is double || value is float)
				storage = new double_storage(_row, --col, Convert.ToDouble(value));
			else if (value is string)
				storage = new str_storage(_row, --col, value.ToString());
			else if (value is DateTime)
				storage = new datetime_storage(_row, --col, (DateTime)value);
			
		}
		public void marshall(BinaryWriter wr)
		{
			storage.marshall(wr);
		}
    }

	public abstract class cell_storage
	{
		protected ushort col = 0;
		protected ushort row = 0;
		protected ushort opcode = 0x0201;
		public abstract void marshall(BinaryWriter wr);
	}

	public class str_storage : cell_storage
	{
		string _v;

		public str_storage(ushort _row, ushort _col, string value)
		{
			row = _row;
			col = _col;
			opcode = 0x0204;
			_v = value;
		}
		public override void marshall(BinaryWriter wr)
		{
			ushort[] cell_data = { 0x0204, (ushort)(8 + _v.Length), row, col, 0, (ushort)_v.Length };
			
			byte[] bdata = Encoding.GetEncoding("iso-8859-9").GetBytes(_v);

			foreach (ushort d in cell_data)
				wr.Write(d);

			foreach (byte bd in bdata)
				wr.Write(bd);

		}
	}

	public class int_storage : cell_storage
	{
		int _v;
		public int_storage(ushort _row, ushort _col, int value)
		{
			row = _row;
			col = _col;
			opcode = 0x027E;
			_v = value;
		}
		public override void marshall(BinaryWriter wr)
		{
			ushort[] cell_data = { opcode, 10, row, col, 0 };

			foreach (ushort d in cell_data)
				wr.Write(d);

			wr.Write(((_v << 2) | 2));
		}
	}

	public class double_storage : cell_storage
	{
		double _v;
		public double_storage(ushort _row, ushort _col, double value) 
		{
			row = _row;
			col = _col;
			opcode = 0x0203;
			_v = value;
		}
		public override void marshall(BinaryWriter wr)
		{
			ushort[] cell_data = { opcode, 14, row, col, 0 };

			foreach (ushort d in cell_data)
				wr.Write(d);

			wr.Write(_v);
		}
	}
	public class datetime_storage : cell_storage
	{
		DateTime _v;
		public datetime_storage(ushort _row, ushort _col, DateTime value)
		{
			row = _row;
			col = _col;
			opcode = 0x0203;
			_v = value;
		}
		public override void marshall(BinaryWriter wr)
		{

			//DateTime bd = new DateTime(1899, 12, 31);
			//TimeSpan ts = _v - bd;

			//ushort days = (ushort)(ts.Days + 1);
			//ushort[] cell_data = { 0x0002, 09, 0, 0 };
			//cell_data[2] = (ushort)row;
			//cell_data[3] = (ushort)col;


			////WriteUshortArray(cell_data);
			//foreach (ushort d in cell_data)
			//	wr.Write(d);

			//wr.Write(new byte[] { 0x0, 1, 0x0 });
			//wr.Write(days);

			string _vx = _v.ToShortDateString();

			ushort[] cell_data = { 0x0204, (ushort)(8 + _vx.Length), row, col, 0, (ushort)_vx.Length };

			byte[] bdata = Encoding.GetEncoding("iso-8859-9").GetBytes(_vx);

			foreach (ushort d in cell_data)
				wr.Write(d);

			foreach (byte bd in bdata)
				wr.Write(bd);
			
		}
	}
}
