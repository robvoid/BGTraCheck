using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BGTraChecker
{
	public partial class FormConfig : Form
	{
		public List<char> SelectedSymbols { get; private set; } = new List<char>();
		public FormConfig()
		{
			InitializeComponent();
			LoadOptions();
		}

		private void LoadOptions()
		{
			string tmp = Properties.Settings.Default.SepSymbols;
			string[] symbols = tmp.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
			foreach (var s in symbols)
			{
				clbUsingSymbols.Items.Add(s.Trim());
			}
			for (int i = 0; i < clbUsingSymbols.Items.Count; i++)
			{
				clbUsingSymbols.SetItemChecked(i, true);
			}
			//clbUsingSymbols.Refresh();
		}

		private void SaveOptions()
		{
			List<string> syms = new List<string>();
			foreach(string s in clbUsingSymbols.Items) 
			{ 
				syms.Add(s.Trim()); 
			}
			string combined = string.Join(", ", syms);
			Properties.Settings.Default.SepSymbols = combined;
			Properties.Settings.Default.Save();
		}

		private void btRemoveSymbol_Click(object sender, EventArgs e)
		{
			if(clbUsingSymbols.CheckedItems.Count == 0){
				return;
			}
			DialogResult result = MessageBox.Show("确定删除选定条目?", "确认删除", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
			if (result == DialogResult.Yes) {
				clbUsingSymbols.BeginUpdate();
				try
				{
					for(int i=clbUsingSymbols.Items.Count-1; i >= 0; i--)
					{
						if (clbUsingSymbols.GetItemChecked(i))
						{
							clbUsingSymbols.Items.RemoveAt(i);
						}
					}
				}
				finally
				{
					clbUsingSymbols.EndUpdate();
				}
			}
		}

		private void btAddSymbol_Click(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(tbAddSymbol.Text))
			{
				return;
			}
			string cur = tbAddSymbol.Text;
			if (clbUsingSymbols.Items.Contains(cur))
			{
				return;
			}
			clbUsingSymbols.Items.Add(cur);
			clbUsingSymbols.Refresh();
		}

		private void btCancel_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}

		private void btConfirmSymbols_Click(object sender, EventArgs e)
		{
			if (clbUsingSymbols.CheckedItems.Count == 0)
			{
				MessageBox.Show("请选择至少一个分隔符");
				return;
			}
			try
			{
				SelectedSymbols.Clear();
				foreach(var item in clbUsingSymbols.CheckedItems)
				{
					string t = item.ToString().Trim();
					SelectedSymbols.Add(t[0]);
				}
				SaveOptions();
				this.DialogResult = DialogResult.OK;
				this.Close();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);			
			}
		}
	}
}
