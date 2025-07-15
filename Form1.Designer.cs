namespace BGTraChecker
{
	partial class Form1
	{
		/// <summary>
		/// 必需的设计器变量。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 清理所有正在使用的资源。
		/// </summary>
		/// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows 窗体设计器生成的代码

		/// <summary>
		/// 设计器支持所需的方法 - 不要修改
		/// 使用代码编辑器修改此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{
			this.btSelectRoot = new System.Windows.Forms.Button();
			this.tbRootPath = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.cbSRCLang = new System.Windows.Forms.ComboBox();
			this.cbDstLang = new System.Windows.Forms.ComboBox();
			this.btCompare = new System.Windows.Forms.Button();
			this.treeView1 = new System.Windows.Forms.TreeView();
			this.tbCompareResult = new System.Windows.Forms.TextBox();
			this.btSetSymbols = new System.Windows.Forms.Button();
			this.tbCurrentSymbols = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.btRecoverPath = new System.Windows.Forms.Button();
			this.btDetectEncoding = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// btSelectRoot
			// 
			this.btSelectRoot.Font = new System.Drawing.Font("宋体", 14F);
			this.btSelectRoot.Location = new System.Drawing.Point(16, 18);
			this.btSelectRoot.Name = "btSelectRoot";
			this.btSelectRoot.Size = new System.Drawing.Size(152, 45);
			this.btSelectRoot.TabIndex = 1;
			this.btSelectRoot.Text = "选择Tra目录";
			this.btSelectRoot.UseVisualStyleBackColor = true;
			this.btSelectRoot.Click += new System.EventHandler(this.btSelectRoot_Click);
			// 
			// tbRootPath
			// 
			this.tbRootPath.Font = new System.Drawing.Font("宋体", 14F);
			this.tbRootPath.Location = new System.Drawing.Point(174, 28);
			this.tbRootPath.Name = "tbRootPath";
			this.tbRootPath.ReadOnly = true;
			this.tbRootPath.Size = new System.Drawing.Size(829, 29);
			this.tbRootPath.TabIndex = 2;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("宋体", 14F);
			this.label1.Location = new System.Drawing.Point(12, 90);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(85, 19);
			this.label1.TabIndex = 3;
			this.label1.Text = "基础语言";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("宋体", 14F);
			this.label2.Location = new System.Drawing.Point(320, 90);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(85, 19);
			this.label2.TabIndex = 4;
			this.label2.Text = "翻译语言";
			// 
			// cbSRCLang
			// 
			this.cbSRCLang.Font = new System.Drawing.Font("宋体", 14F);
			this.cbSRCLang.FormattingEnabled = true;
			this.cbSRCLang.Location = new System.Drawing.Point(103, 87);
			this.cbSRCLang.Name = "cbSRCLang";
			this.cbSRCLang.Size = new System.Drawing.Size(171, 27);
			this.cbSRCLang.TabIndex = 5;
			this.cbSRCLang.SelectedIndexChanged += new System.EventHandler(this.cbSRCLang_SelectedIndexChanged);
			// 
			// cbDstLang
			// 
			this.cbDstLang.Font = new System.Drawing.Font("宋体", 14F);
			this.cbDstLang.FormattingEnabled = true;
			this.cbDstLang.Location = new System.Drawing.Point(411, 87);
			this.cbDstLang.Name = "cbDstLang";
			this.cbDstLang.Size = new System.Drawing.Size(199, 27);
			this.cbDstLang.TabIndex = 6;
			this.cbDstLang.SelectedIndexChanged += new System.EventHandler(this.cbDstLang_SelectedIndexChanged);
			// 
			// btCompare
			// 
			this.btCompare.Font = new System.Drawing.Font("宋体", 14F);
			this.btCompare.Location = new System.Drawing.Point(12, 170);
			this.btCompare.Name = "btCompare";
			this.btCompare.Size = new System.Drawing.Size(262, 53);
			this.btCompare.TabIndex = 7;
			this.btCompare.Text = "对比条目";
			this.btCompare.UseVisualStyleBackColor = true;
			this.btCompare.Click += new System.EventHandler(this.btCompare_Click);
			// 
			// treeView1
			// 
			this.treeView1.Font = new System.Drawing.Font("宋体", 14F);
			this.treeView1.Location = new System.Drawing.Point(12, 229);
			this.treeView1.Name = "treeView1";
			this.treeView1.Size = new System.Drawing.Size(983, 552);
			this.treeView1.TabIndex = 8;
			// 
			// tbCompareResult
			// 
			this.tbCompareResult.Font = new System.Drawing.Font("宋体", 18F);
			this.tbCompareResult.Location = new System.Drawing.Point(324, 179);
			this.tbCompareResult.Name = "tbCompareResult";
			this.tbCompareResult.ReadOnly = true;
			this.tbCompareResult.Size = new System.Drawing.Size(286, 35);
			this.tbCompareResult.TabIndex = 9;
			// 
			// btSetSymbols
			// 
			this.btSetSymbols.Font = new System.Drawing.Font("宋体", 14F);
			this.btSetSymbols.Location = new System.Drawing.Point(692, 177);
			this.btSetSymbols.Name = "btSetSymbols";
			this.btSetSymbols.Size = new System.Drawing.Size(177, 36);
			this.btSetSymbols.TabIndex = 10;
			this.btSetSymbols.Text = "调整分隔符";
			this.btSetSymbols.UseVisualStyleBackColor = true;
			this.btSetSymbols.Click += new System.EventHandler(this.btSetSymbols_Click);
			// 
			// tbCurrentSymbols
			// 
			this.tbCurrentSymbols.Font = new System.Drawing.Font("宋体", 14F);
			this.tbCurrentSymbols.Location = new System.Drawing.Point(692, 132);
			this.tbCurrentSymbols.Name = "tbCurrentSymbols";
			this.tbCurrentSymbols.ReadOnly = true;
			this.tbCurrentSymbols.Size = new System.Drawing.Size(307, 29);
			this.tbCurrentSymbols.TabIndex = 11;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Font = new System.Drawing.Font("宋体", 14F);
			this.label3.Location = new System.Drawing.Point(688, 90);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(218, 19);
			this.label3.TabIndex = 12;
			this.label3.Text = "当前选定字符串分隔符：";
			// 
			// btRecoverPath
			// 
			this.btRecoverPath.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.btRecoverPath.Location = new System.Drawing.Point(12, 128);
			this.btRecoverPath.Name = "btRecoverPath";
			this.btRecoverPath.Size = new System.Drawing.Size(262, 35);
			this.btRecoverPath.TabIndex = 13;
			this.btRecoverPath.Text = "重建目录结构";
			this.btRecoverPath.UseVisualStyleBackColor = true;
			this.btRecoverPath.Click += new System.EventHandler(this.btRecoverPath_Click);
			// 
			// btDetectEncoding
			// 
			this.btDetectEncoding.Font = new System.Drawing.Font("宋体", 12F);
			this.btDetectEncoding.Location = new System.Drawing.Point(324, 130);
			this.btDetectEncoding.Name = "btDetectEncoding";
			this.btDetectEncoding.Size = new System.Drawing.Size(286, 35);
			this.btDetectEncoding.TabIndex = 14;
			this.btDetectEncoding.Text = "检测基础语言编码";
			this.btDetectEncoding.UseVisualStyleBackColor = true;
			this.btDetectEncoding.Click += new System.EventHandler(this.btDetectEncoding_Click);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoScroll = true;
			this.ClientSize = new System.Drawing.Size(1011, 805);
			this.Controls.Add(this.btDetectEncoding);
			this.Controls.Add(this.btRecoverPath);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.tbCurrentSymbols);
			this.Controls.Add(this.btSetSymbols);
			this.Controls.Add(this.tbCompareResult);
			this.Controls.Add(this.treeView1);
			this.Controls.Add(this.btCompare);
			this.Controls.Add(this.cbDstLang);
			this.Controls.Add(this.cbSRCLang);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.tbRootPath);
			this.Controls.Add(this.btSelectRoot);
			this.Name = "Form1";
			this.Text = "TRA条目对比检查";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btSelectRoot;
		private System.Windows.Forms.TextBox tbRootPath;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox cbSRCLang;
		private System.Windows.Forms.ComboBox cbDstLang;
		private System.Windows.Forms.Button btCompare;
		private System.Windows.Forms.TreeView treeView1;
		private System.Windows.Forms.TextBox tbCompareResult;
		private System.Windows.Forms.Button btSetSymbols;
		private System.Windows.Forms.TextBox tbCurrentSymbols;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button btRecoverPath;
		private System.Windows.Forms.Button btDetectEncoding;
	}
}

