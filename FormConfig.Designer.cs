namespace BGTraChecker
{
	partial class FormConfig
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Windows.Forms.Button btAddSymbol;
			this.clbUsingSymbols = new System.Windows.Forms.CheckedListBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.tbAddSymbol = new System.Windows.Forms.TextBox();
			this.btConfirmSymbols = new System.Windows.Forms.Button();
			this.btCancel = new System.Windows.Forms.Button();
			this.btRemoveSymbol = new System.Windows.Forms.Button();
			btAddSymbol = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// clbUsingSymbols
			// 
			this.clbUsingSymbols.Font = new System.Drawing.Font("宋体", 18F);
			this.clbUsingSymbols.FormattingEnabled = true;
			this.clbUsingSymbols.Location = new System.Drawing.Point(20, 68);
			this.clbUsingSymbols.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
			this.clbUsingSymbols.Name = "clbUsingSymbols";
			this.clbUsingSymbols.ScrollAlwaysVisible = true;
			this.clbUsingSymbols.Size = new System.Drawing.Size(300, 184);
			this.clbUsingSymbols.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("宋体", 14F);
			this.label1.Location = new System.Drawing.Point(20, 33);
			this.label1.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(123, 19);
			this.label1.TabIndex = 1;
			this.label1.Text = "使用分隔字符";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("宋体", 14F);
			this.label2.Location = new System.Drawing.Point(347, 33);
			this.label2.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(104, 19);
			this.label2.TabIndex = 2;
			this.label2.Text = "添加分隔符";
			// 
			// tbAddSymbol
			// 
			this.tbAddSymbol.Location = new System.Drawing.Point(351, 68);
			this.tbAddSymbol.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
			this.tbAddSymbol.Name = "tbAddSymbol";
			this.tbAddSymbol.Size = new System.Drawing.Size(246, 29);
			this.tbAddSymbol.TabIndex = 3;
			// 
			// btAddSymbol
			// 
			btAddSymbol.Font = new System.Drawing.Font("宋体", 14F);
			btAddSymbol.Location = new System.Drawing.Point(351, 107);
			btAddSymbol.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
			btAddSymbol.Name = "btAddSymbol";
			btAddSymbol.Size = new System.Drawing.Size(246, 36);
			btAddSymbol.TabIndex = 4;
			btAddSymbol.Text = "追加";
			btAddSymbol.UseVisualStyleBackColor = true;
			btAddSymbol.Click += new System.EventHandler(this.btAddSymbol_Click);
			// 
			// btConfirmSymbols
			// 
			this.btConfirmSymbols.Location = new System.Drawing.Point(20, 263);
			this.btConfirmSymbols.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
			this.btConfirmSymbols.Name = "btConfirmSymbols";
			this.btConfirmSymbols.Size = new System.Drawing.Size(300, 49);
			this.btConfirmSymbols.TabIndex = 5;
			this.btConfirmSymbols.Text = "确认";
			this.btConfirmSymbols.UseVisualStyleBackColor = true;
			this.btConfirmSymbols.Click += new System.EventHandler(this.btConfirmSymbols_Click);
			// 
			// btCancel
			// 
			this.btCancel.Location = new System.Drawing.Point(351, 263);
			this.btCancel.Name = "btCancel";
			this.btCancel.Size = new System.Drawing.Size(246, 49);
			this.btCancel.TabIndex = 6;
			this.btCancel.Text = "取消";
			this.btCancel.UseVisualStyleBackColor = true;
			this.btCancel.Click += new System.EventHandler(this.btCancel_Click);
			// 
			// btRemoveSymbol
			// 
			this.btRemoveSymbol.Location = new System.Drawing.Point(351, 204);
			this.btRemoveSymbol.Name = "btRemoveSymbol";
			this.btRemoveSymbol.Size = new System.Drawing.Size(246, 36);
			this.btRemoveSymbol.TabIndex = 7;
			this.btRemoveSymbol.Text = "删除选定";
			this.btRemoveSymbol.UseVisualStyleBackColor = true;
			this.btRemoveSymbol.Click += new System.EventHandler(this.btRemoveSymbol_Click);
			// 
			// FormConfig
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 19F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(625, 326);
			this.Controls.Add(this.btRemoveSymbol);
			this.Controls.Add(this.btCancel);
			this.Controls.Add(this.btConfirmSymbols);
			this.Controls.Add(btAddSymbol);
			this.Controls.Add(this.tbAddSymbol);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.clbUsingSymbols);
			this.Font = new System.Drawing.Font("宋体", 14F);
			this.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
			this.Name = "FormConfig";
			this.Text = "修改字符串分隔符";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.CheckedListBox clbUsingSymbols;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox tbAddSymbol;
		private System.Windows.Forms.Button btConfirmSymbols;
		private System.Windows.Forms.Button btCancel;
		private System.Windows.Forms.Button btRemoveSymbol;
	}
}