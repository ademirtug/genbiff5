using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ademirtug.biff5
{
    public class WorkSheet
    {
	    Dictionary<ushort, Row> _rows = new Dictionary<ushort,Row>();

		public void AddCell(ushort row, ushort col, object value)
		{
			Row r = null;
			if (_rows.ContainsKey(row))
				r = _rows[row];
			else
			{
				r = new Row(row);
				_rows.Add(row, r);
			}

			r.AddCell(col, value);
		}


        public Row this[ushort index]
        {
            get { return _rows[index]; }
            set { _rows[index] = value; }
        }

		public void Save(string path)
		{
			FileStream stream = new FileStream(path, FileMode.OpenOrCreate);
			Save(stream);
			
			stream.Flush();
			stream.Close();
		}
		public void Save(Stream stream)
		{
			BinaryWriter wr = new BinaryWriter(stream);
			//bof
			ushort[] _bof = { /*biff5*/0x0409, 0x08, 0x00, 0x10, 0x00, 0x00 };
			foreach (ushort data in _bof)
				wr.Write(data);


			//codepage - win1254
			ushort[] cpdata = { 0x0042, 0x02, 0x04E6 };
			foreach (ushort cdata in cpdata)
				wr.Write(cdata);

			WriteFormat(wr, "General");
			WriteFormat(wr, "dd/mm/yyyy");




	
			//data
			foreach (Row r in _rows.Values)
				r.marshall(wr);

			//eof
			wr.Write(0x000A);
		}

		private void WriteFormat(BinaryWriter wr, string format)
		{
			ushort[] date_format = { 0x001E, 0 };
			byte[] format_text = Encoding.ASCII.GetBytes(format);
			date_format[1] = (ushort)(format_text.Length + 1);

			foreach (ushort dfdata in date_format)
				wr.Write(dfdata);

			wr.Write((byte)(format_text.Length));
			wr.Write(format_text);
		}

    }

	public class DTC
	{
 		public static WorkSheet Convert(System.Data.DataTable table)
 		{
			WorkSheet sheet = new WorkSheet();


			for (ushort e = 0; e < table.Columns.Count; e++)
				sheet.AddCell((ushort)1, (ushort)(e+1), table.Columns[e].ColumnName);



			for (ushort i = 1; i <= table.Rows.Count; i++)
				for (ushort x = 0; x < table.Columns.Count; x++)
					sheet.AddCell( (ushort)(i+1) , (ushort)(x + 1), table.Rows[(i-1)][x]);

			return sheet;
		}
	}
}
