using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ademirtug.biff5;

namespace testbiff5
{
	public partial class main_form : Form
	{
		public main_form()
		{
			InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			WorkSheet ws = new WorkSheet();


			ws.AddCell(1, 1, 11);
			ws.AddCell(2, 2, 2.22);
			ws.AddCell(3, 3, "Akın");
			ws.AddCell(4, 4, DateTime.Now);


			ws.Save("test.xls");

		}
	}
}
