using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DnsPolygraph
{

    public partial class UniqueDomains : Form
    {
        private readonly DataTable dtCount = new DataTable();
        
        public UniqueDomains()
        {
            InitializeComponent();
        }



        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void UniqueDomains_Load(object sender, EventArgs e)
        {
            this.KeyPreview = true;
            KeyDown += UniqueDomains_KeyDown;
            dtCount.Columns.Add("Date", typeof(string));
            dtCount.Columns.Add("Count", typeof(int));
            dataGridView1.DataSource = dtCount;
            dataGridView1.ClearSelection();

            //dataGridView1.CurrentCell.Selected = false;
            dataGridView1.Columns[0].Width = 200;
            dataGridView1.Columns[1].Width = 55;
            updateRows();

        }
        
        private void updateRows()
        {
            dtCount.Clear();
            var res = (from x in MainWin.DtSales.AsEnumerable()
                group x by (string)x[2] into y
                select new { Key = y.Key, Count = y.Count() }).ToArray();


            foreach (var item in res)
            {
                //Console.WriteLine(item.key.ToString());
                dtCount.Rows.Add(item.Key.ToString(), item.Count);
            }

            dtCount.DefaultView.Sort = "Count DESC";
            if  (dataGridView1.FirstDisplayedScrollingRowIndex > -1)
               dataGridView1.FirstDisplayedScrollingRowIndex = 0;

        }

        private void UniqueDomains_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.ToString() == "F5")
            {
                updateRows();
            }
        }

        private void UniqueDomains_Activated(object sender, EventArgs e)
        {
            this.updateRows();
        }
    }
}
