﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ude;
using System.Text.RegularExpressions;
using Ude.Core;
using System.Net.Configuration;

namespace BGTraChecker
{
	public partial class Form1 : Form
	{
		private readonly string ENGTitle = "english";
		private readonly string CNSTitle = "schinese";
		private bool bPrepared = false;
		private string selectedRoot = null;
		private FolderBrowserDialog mFB = new FolderBrowserDialog();
		private List<string> mPatterns = new List<string>();
		private List<string> mDefaultPatterns = new List<string>();

		public Form1()
		{
			InitializeComponent();
			LoadLastFolder();
			LoadDefaultPatterns();
			LoadCurrentPatterns();
			treeView1.Scrollable = true;
		}

		private void LoadDefaultPatterns()
		{
			string strFull = @"@(-?\d+)(?:[^~%""￥]*)([~%""￥])((?!\2).*)\2";
			string strPartial = @"@(-?\d+)(?:[^~%""￥]*)([~%""￥])((?!\2).*)";
			string strEnd = @"[^~""%￥]*([~""%￥])$";
			string strEmptyLine = @"[~%""￥]";
			if(mDefaultPatterns is null)
			{
				mDefaultPatterns = new List<string>();
			}
			mDefaultPatterns.Clear();
			mDefaultPatterns.Add(strFull);
			mDefaultPatterns.Add(strPartial);
			mDefaultPatterns.Add(strEnd);
			mDefaultPatterns.Add(strEmptyLine);
		}

		private void SetCurrentWithDefault()
		{
			if(mPatterns is null)
			{
				mPatterns = new List<string>();
			}
			mPatterns.Clear();
			for(int i = 0; i < mDefaultPatterns.Count; ++i)
			{
				mPatterns.Add(mDefaultPatterns[i]);
			}
		}

		private void LoadCurrentPatterns()
		{
			try
			{
				List<char> list = new List<char>();
				string[] ss = tbCurrentSymbols.Text.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
				if(ss is null)
				{
					SetCurrentWithDefault();
					return;
				}
				if(ss.Length == 0)
				{
					SetCurrentWithDefault();
					return;
				}
				for (int i = 0; i < ss.Length; ++i)
				{
					char c = ss[i].Trim()[0];
					list.Add(c);
				}
				BuildPatterns(list);
			}
			catch(Exception ex)
			{
				SetCurrentWithDefault();
			}
		}

		private void LoadLastFolder()
		{
			mFB.Description = "请选择TRA目录";
			string last = Properties.Settings.Default.LastFolder;
			if(string.IsNullOrEmpty(last))
			{
				mFB.RootFolder = Environment.SpecialFolder.MyComputer;
				return;
			}
			if (!Directory.Exists(last))
			{
				mFB.RootFolder = Environment.SpecialFolder.MyComputer;
			}
			else
			{
				mFB.SelectedPath = last;
			}

			string tmp = Properties.Settings.Default.SepSymbols;
			string[] symbols = tmp.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
			List<char> chars = new List<char>();
			foreach (var s in symbols)
			{
				chars.Add(s[0]);
			}
			tbCurrentSymbols.Text = tmp;
		}

		private void CheckSubNode(ref TreeNode node, string name)
		{
			if (!node.Nodes.ContainsKey(name))
			{
				TreeNode tmpNode = new TreeNode(name);
				tmpNode.Name = name;
				node.Nodes.Add(tmpNode);
			}
		}

		private void btSelectRoot_Click(object sender, EventArgs e)
		{
			if (mFB.ShowDialog() == DialogResult.OK &&
				!string.IsNullOrWhiteSpace(mFB.SelectedPath))
			{
				tbRootPath.Text = mFB.SelectedPath;
			}

			if (!Directory.Exists(tbRootPath.Text))
			{
				MessageBox.Show($"目录不存在: {tbRootPath.Text}");
				return;
			}

			// Use a writable property or method to update the LastFolder value  
			UpdateLastFolder(tbRootPath.Text);

			bool srcFound = false;
			bool dstFound = false;
			try
			{
				cbSRCLang.Items.Clear();
				cbDstLang.Items.Clear();
				cbSRCLang.SelectedIndex = -1;
				cbDstLang.SelectedIndex = -1;
				treeView1.Nodes.Clear();
				treeView1.SelectedNode = null;
				string[] subDirs = Directory.GetDirectories(tbRootPath.Text);
				if (subDirs is null)
				{
					MessageBox.Show("不是有效的TRA目录");
					return;
				}
				if (subDirs.Length == 0)
				{
					MessageBox.Show("不是有效的TRA目录");
					return;
				}
				List<string> subNames = new List<string>();
				for (int i = 0; i < subDirs.Length; ++i)
				{
					string lastName = Path.GetFileName(subDirs[i]);
					subNames.Add(lastName);
					cbSRCLang.Items.Add(lastName);
					cbDstLang.Items.Add(lastName);
					if (lastName.ToLower() == ENGTitle)
					{
						cbSRCLang.SelectedIndex = i;
						srcFound = true;
					}
					if (lastName.ToLower() == CNSTitle)
					{
						cbDstLang.SelectedIndex = i;
						dstFound = true;
					}
				}

				bPrepared = srcFound && dstFound;
				selectedRoot = tbRootPath.Text;
				if (treeView1.Nodes.Count > 0)
				{
					treeView1.Nodes.Clear();
				}
				tbCompareResult.Text = "比对未开始";
			}
			catch (Exception ex)
			{
				MessageBox.Show($"内部错误: {ex.StackTrace}");
			}
		}

		private void UpdateLastFolder(string folderPath)
		{
			// Assuming you have a writable method or property to update the LastFolder value  
			Properties.Settings.Default.LastFolder = folderPath;
			Properties.Settings.Default.Save();
		}

		private Match MatchFullPattern(string line, out int sep)
		{
			sep = 0;
			string pat = mPatterns[0];
			Match match = Regex.Match(line.Trim(), pat);
			if (match.Success && match.Groups.Count > 2)
			{
				string g2 = match.Groups[2].Value;
				sep = (int)(g2[0]);
			}
			return match;
		}

		private Match MatchPartialPattern(string line, out int sep)
		{
			sep = 0;
			string pat = mPatterns[1];
			Match match = Regex.Match(line.Trim(), pat);
			if (match.Success && match.Groups.Count > 2)
			{
				string g2 = match.Groups[2].Value;
				sep = (int)(g2[0]);
			}
			return match;
		}

		private Match MatchEndPattern(string line, out int sep)
		{
			sep = 0;
			string pat = mPatterns[2];
			Match match = Regex.Match(line.Trim(), pat);
			if (match.Success && match.Groups.Count > 1)
			{
				string g1 = match.Groups[1].Value;
				sep = (int)(g1[0]);
			}
			return match;
		}

		private Dictionary<int, TRAItemInfo> ProcessOneFile(string filePath, string lang, string encoding, out List<string> errorLineNums)
		{
			errorLineNums = new List<string>();
			try
			{
				//string patternFull = @"@(\d+)(?:[^~]*)~([^~]+)~";
				//string patternPartial = @"@(\d+)(?:[^~]*)~([^~]+)$";
				//string patternEnd = @"^[^~]*~$";
				Dictionary<int, TRAItemInfo> res = new Dictionary<int, TRAItemInfo>();
				Encoding curEnc = null;
				int lastSep = 0;
				if(encoding is null)
				{
					curEnc = Encoding.UTF8;
				}
				else if(encoding == Ude.Charsets.BIG5)
				{
					curEnc = Encoding.GetEncoding("Big5");
				}
				else
				{
					curEnc = Encoding.GetEncoding(encoding);
				}
				
				string[] allLines = File.ReadAllLines(filePath, curEnc);

				int curLine = 0;
				while(curLine < allLines.Length)
				{
					string start = allLines[curLine];
					
					if (start.StartsWith(@"//"))
					{
						curLine++;
						continue;
					}

					if (start.Contains(@"/*") && !start.Contains(@"*/"))
					{
						int tCnt = 1;
						while(curLine + tCnt < allLines.Length)
						{
							string midLine = allLines[curLine + tCnt];

							if (midLine.Contains(@"*/"))
							{
								break;
							}
							tCnt++;
						}
						curLine += tCnt;
						continue;
					}
					
					Match mtCrucial = Regex.Match(start.Trim(), mPatterns[3]);
					if (!mtCrucial.Success)
					{
						curLine++;
						continue;
					}

					Match mtFull = MatchFullPattern(start, out int sepFullOuter);
					if (mtFull.Success)
					{
						int tid = int.Parse(mtFull.Groups[1].Value);
						if (res.ContainsKey(tid))
						{
							TRAItemInfo todo = res[tid];
							res[tid].DupLines.Add((curLine + 1).ToString());
							curLine++;
							continue;
						}
						TRAItemInfo tmpInfo = new TRAItemInfo();
						tmpInfo.TRAId = tid;
						tmpInfo.StartLineNum = curLine + 1; 
						tmpInfo.LineCount = 1;
						tmpInfo.Correct = true;
						
						res.Add(tid, tmpInfo);
						lastSep = sepFullOuter;
						curLine++;
						continue;
					}

					Match mtPartial = MatchPartialPattern(start, out int sepPartialOuter);
					lastSep = sepPartialOuter;
					if (mtPartial.Success)
					{
						TRAItemInfo tmpInfo = new TRAItemInfo();
						int tid = int.Parse(mtPartial.Groups[1].Value);
						tmpInfo.TRAId = tid;
						tmpInfo.StartLineNum= curLine + 1;

						int tCnt = 1;
						while (tCnt + curLine < allLines.Length)
						{
							string midLine = allLines[curLine + tCnt];

							Match innerFull = MatchFullPattern(midLine, out int sepFullInner);
							if (innerFull.Success)
							{
								if (res.ContainsKey(tid))
								{
									MessageBox.Show($"存在重复TRA序号: {Path.GetFileName(filePath)}, [{tid}]");
								}
								else
								{
									tmpInfo.Correct = false;
									tmpInfo.LineCount = tCnt;
									res.Add(tid, tmpInfo);
								}

								
								int tid2 = int.Parse(innerFull.Groups[1].Value);
								if (res.ContainsKey(tid2))
								{
									TRAItemInfo todo = res[tid2];
									todo.DupLines.Add((curLine + tCnt + 1).ToString());
								}
								else
								{
									TRAItemInfo tmpInfo2 = new TRAItemInfo();
									tmpInfo2.TRAId = tid2;
									tmpInfo2.StartLineNum = curLine + tCnt;
									tmpInfo2.LineCount = 1;
									tmpInfo2.Correct = true;
									res.Add(tid2, tmpInfo2);
								}
								lastSep = sepFullInner;
								tCnt++;
								break;
							}
							Match innerEnd = MatchEndPattern(midLine, out int sepEndInner);
							lastSep = sepEndInner;
							if (innerEnd.Success)
							{
								if(sepEndInner != sepPartialOuter)
								{
									tCnt++;
									continue;
								}
								tmpInfo.Correct = (sepEndInner==sepPartialOuter);
								tmpInfo.LineCount = tCnt + 1;
								if (res.ContainsKey(tid))
								{
									TRAItemInfo todo = res[tid];
									todo.DupLines.Add((tmpInfo.StartLineNum + 1).ToString());
								}
								else
								{
									res.Add(tid, tmpInfo);
								}
								tCnt++;
								break;
							}

							tCnt++;
							
						}
						curLine += tCnt;
						continue;
					}

					Match mtEnd = MatchEndPattern(start, out int sepEndOutter);
					if (mtEnd.Success) 
					{
						string tmp1 = mtEnd.Groups[1].Value;
						if (sepEndOutter == 0)
						{
							errorLineNums.Add($"第{curLine + 1}行");
						}else if(sepEndOutter == lastSep)
						{
							errorLineNums.Add($"第{curLine + 1}行");
						}
						curLine++;
						continue;
					}
					curLine++;
				}

				//System.Console.WriteLine($"file={Path.GetFileName(filePath)}, tra_num={res.Count}, total_lines={allLines.Length}");
				return res;
			}
			catch (Exception ex)
			{
				MessageBox.Show($"解析失败: 文件{Path.GetFileName(filePath)} : {ex.Message}");
				return null;
			}
		}

		private bool CheckJob(bool checkDst=true)
		{
			tbCompareResult.Text = "";
			if (treeView1?.Nodes.Count > 0)
			{
				treeView1?.Nodes.Clear();
			}
			int idxSrc = cbSRCLang.SelectedIndex;
			int idxDst = cbDstLang.SelectedIndex;
			if (idxSrc < 0 || idxDst < 0)
			{
				MessageBox.Show("请先选择语言!");
				return false;
			}
			if (idxSrc == idxDst)
			{
				MessageBox.Show("请选择不同语言！");
				return false;
			}

			if (!bPrepared)
			{
				MessageBox.Show("路径未选择！");
				return false;
			}
			return true;
		}


		private void btCompare_Click(object sender, EventArgs e)
		{
			try
			{
				bool bAllPass = true;
				if(!CheckJob())
				{
					return;
				}

				string srcLangName = cbSRCLang.SelectedItem.ToString();
				string dstLangName = cbDstLang.SelectedItem.ToString();

				string srcDirName = Path.Combine(selectedRoot, srcLangName);
				string dstDirName = Path.Combine(selectedRoot, dstLangName);

				if (!Directory.Exists(srcDirName))
				{
					MessageBox.Show($"路径不存在: {srcDirName}");
					return;
				}
				if (!Directory.Exists(dstDirName))
				{
					MessageBox.Show($"路径不存在: {dstDirName}");
					return;
				}

				string[] srcTras = Directory.GetFiles(srcDirName, "*.tra",SearchOption.AllDirectories);
				string[] dstTras = Directory.GetFiles(dstDirName, "*.tra",SearchOption.AllDirectories);

				Dictionary<string, string> srcPaths = new Dictionary<string, string>();
				Dictionary<string, string> dstPaths = new Dictionary<string, string>();
				SortedDictionary<string, Tuple<string, string>> mutalPaths = new SortedDictionary<string, Tuple<string, string>>();

				for (int i = 0; i < srcTras.Length; ++i)
				{
					//string lastName = Path.GetFileName(srcTras[i]);
					var rela = GetRelativePath(srcDirName, srcTras[i]);
					if (!rela.flag)
					{
						MessageBox.Show($"无法获取相对路径: {srcTras[i]}");
						continue;
					}
					srcPaths.Add(rela.p, srcTras[i]);
				}

				for (int i = 0; i < dstTras.Length; ++i)
				{
					//string lastName = Path.GetFileName(dstTras[i]);
					var rela = GetRelativePath(dstDirName, dstTras[i]);
					if (!rela.flag)
					{
						MessageBox.Show($"无法获取相对路径: {dstTras[i]}");
						continue;
					}
					dstPaths.Add(rela.p, dstTras[i]);
				}

				TreeNode nodeSrcOny = new TreeNode($"{srcLangName}有但{dstLangName}缺失");
				TreeNode nodeDstOny = new TreeNode($"{srcLangName}无但{dstLangName}有");
				TreeNode nodeMisMatch = new TreeNode($"{srcLangName}和{dstLangName}条目不同");
				TreeNode nodeEncodingMisMatch = new TreeNode($"{srcLangName}与{dstLangName}文件编码不同");
				TreeNode nodeError = new TreeNode("文件错误");
				TreeNode nodeDup = new TreeNode("重复tra编号");

				foreach (var src in srcPaths)
				{
					if (!dstPaths.ContainsKey(src.Key))
					{
						TreeNode node = new TreeNode(src.Key);
						node.Name = src.Key;
						nodeSrcOny.Nodes.Add(node);
						bAllPass = false;
					}
					else
					{
						Tuple<string, string> tmp = new Tuple<string, string>(src.Value, dstPaths[src.Key]);
						mutalPaths.Add(src.Key, tmp);
					}
				}
				if (nodeSrcOny.Nodes.Count == 0)
				{
					nodeSrcOny.Nodes.Add(new TreeNode("无"));
				}
				foreach (var dst in dstPaths)
				{
					if (!srcPaths.ContainsKey(dst.Key))
					{
						TreeNode node = new TreeNode(dst.Key);
						nodeDstOny.Nodes.Add(node);
						bAllPass = false;
					}
				}
				if (nodeDstOny.Nodes.Count == 0)
				{
					nodeDstOny.Nodes.Add(new TreeNode("无"));
				}

				foreach (var kvp in mutalPaths)
				{
					(string srcTra, string dstTra) = kvp.Value;
					if (!File.Exists(srcTra) || !File.Exists(dstTra))
					{
						TreeNode tmpNode = new TreeNode(kvp.Key);
						tmpNode.Name = kvp.Key;
						nodeError.Nodes.Add(tmpNode);
					}

					string srcEnc = null;
					string dstEnc = null;
					using (FileStream fs = File.OpenRead(srcTra))
					{
						Ude.CharsetDetector detector = new Ude.CharsetDetector();
						detector.Feed(fs);
						detector.DataEnd();
						srcEnc = detector.Charset;
					}
					using (FileStream fs = File.OpenRead(dstTra))
					{
						Ude.CharsetDetector detector = new Ude.CharsetDetector();
						detector.Feed(fs);
						detector.DataEnd();
						dstEnc = detector.Charset;
					}
					if (srcEnc != dstEnc)
					{
						TreeNode node = new TreeNode($"{kvp.Key}: [{srcLangName}]{srcEnc} vs [{dstLangName}]{dstEnc}");
						nodeEncodingMisMatch.Nodes.Add(node);
						bAllPass = false;
					}

					Dictionary<int, TRAItemInfo> srcInfo = ProcessOneFile(srcTra, srcLangName, srcEnc, out List<string> errorLineSrc);
					Dictionary<int, TRAItemInfo> dstInfo = ProcessOneFile(dstTra, dstLangName, dstEnc, out List<string> errorLineDst);

					if(errorLineDst.Count + errorLineSrc.Count > 0)
					{
						string srcErr = string.Join(",", errorLineSrc);
						string dstErr = string.Join(",", errorLineDst);

						CheckSubNode(ref nodeError, kvp.Key);

						if (!string.IsNullOrEmpty(srcErr)) {
							string tLine = $"{srcLangName}语法错误=>[{srcErr}]";
							nodeError.Nodes[kvp.Key].Nodes.Add(new TreeNode(tLine));
						}
						if (!string.IsNullOrEmpty(dstErr)) {
							string tLine = $"{dstLangName}语法错误=>[{dstErr}]";
							nodeError.Nodes[kvp.Key].Nodes.Add(new TreeNode(tLine));
						}
					}

					List<string> srcMissing = new List<string>();
					List<string> dstMissing = new List<string>();
					List<string> lineCntMiss = new List<string>();
					List<string> srcErrItem = new List<string>();
					List<string> dstErrItem = new List<string>();
					List<string> srcDupItem = new List<string>();
					List<string> dstDupItem = new List<string>();

					foreach (var iData in srcInfo)
					{
						int traID = iData.Key;
						TRAItemInfo tInfo = iData.Value as TRAItemInfo;
						if (!dstInfo.ContainsKey(traID))
						{
							CheckSubNode(ref nodeMisMatch, kvp.Key);
							dstMissing.Add($"@{traID}");
						}
						else
						{
							int otherCnt = dstInfo[traID].LineCount;
							if(tInfo.LineCount != otherCnt)
							{
								CheckSubNode(ref nodeMisMatch, kvp.Key);
								lineCntMiss.Add($"@{traID}");
							}
						}
						
						if (!tInfo.Correct)
						{
							CheckSubNode(ref nodeMisMatch, kvp.Key);
							srcErrItem.Add($"@{traID}");
						}
						if(tInfo.DupLines.Count > 0)
						{
							CheckSubNode(ref nodeDup, kvp.Key);
							string pay1 = string.Join(",",tInfo.DupLines);
							string pay2 = $"@{traID}: {tInfo.StartLineNum}=>[{pay1}]";
							srcDupItem.Add(pay2);
						}
					}
					foreach (var iData in dstInfo)
					{
						int traID = iData.Key;
						TRAItemInfo tInfo = iData.Value as TRAItemInfo;
						if (!srcInfo.ContainsKey(traID))
						{
							CheckSubNode(ref nodeMisMatch, kvp.Key);
							srcMissing.Add($"@{traID}");
						}
						if (!tInfo.Correct)
						{
							CheckSubNode(ref nodeMisMatch, kvp.Key);
							dstErrItem.Add($"@{traID}");
						}
						if(tInfo.DupLines.Count > 0)
						{
							CheckSubNode(ref nodeDup, kvp.Key);
							string pay1 = string.Join (",",tInfo.DupLines);
							string pay2 = $"@{traID}: {tInfo.StartLineNum}=>[{pay1}]";
							dstDupItem.Add(pay2);
						}
					}
					if (srcMissing.Count > 0)
					{
						bAllPass = false;
						string payload = string.Join(",",srcMissing.ToArray());
						string tLine = $"{dstLangName}多余: [{payload}]";
						nodeMisMatch.Nodes[kvp.Key].Nodes.Add(tLine);
					}
					if(dstMissing.Count > 0)
					{
						bAllPass = false;
						string payload = string.Join(",",dstMissing.ToArray());
						string tLine = $"{dstLangName}缺少: [{payload}]";
						nodeMisMatch.Nodes[kvp.Key].Nodes.Add(tLine);
					}
					if (lineCntMiss.Count > 0)
					{
						bAllPass = false;
						string payload = string.Join(",",lineCntMiss.ToArray());
						string tLine = $"{srcLangName}与{dstLangName}行数不一致: [{payload}]";
						nodeMisMatch.Nodes[kvp.Key].Nodes.Add(tLine);
					}
					if (srcErrItem.Count > 0)
					{
						bAllPass = false;
						string payload = string.Join(",",srcErrItem.ToArray());
						string tLine = $"{srcLangName}条目未正确闭合: [{payload}]";
						nodeMisMatch.Nodes[kvp.Key].Nodes.Add(tLine);
					}
					if (dstErrItem.Count > 0)
					{
						bAllPass = false;
						string payload = string.Join(",", dstErrItem.ToArray());
						string tLine = $"{dstLangName}条目未正确闭合: [{payload}]";
						nodeMisMatch.Nodes[kvp.Key].Nodes.Add(tLine);
					}
					if (srcDupItem.Count > 0)
					{
						bAllPass = false;
						string payload = string.Join("||",srcDupItem.ToArray());
						string tLine = $"{srcLangName}重复条目: {payload}";
						nodeDup.Nodes[kvp.Key].Nodes.Add (tLine);
					}
					if(dstDupItem.Count > 0)
					{
						bAllPass = false;
						string payload = string.Join("||", dstDupItem.ToArray());
						string tLine = $"{dstLangName}重复条目: {payload}";
						nodeDup.Nodes[kvp.Key].Nodes.Add(tLine);
					}
				}

				if (nodeError.Nodes.Count == 0)
				{
					nodeError.Nodes.Add(new TreeNode("无"));
				}
				if (nodeEncodingMisMatch.Nodes.Count == 0)
				{
					nodeEncodingMisMatch.Nodes.Add(new TreeNode("无"));
				}
				if (nodeMisMatch.Nodes.Count == 0)
				{
					nodeMisMatch.Nodes.Add(new TreeNode("无"));
				}
				if (nodeDup.Nodes.Count == 0)
				{
					nodeDup.Nodes.Add(new TreeNode("无"));
				}
				treeView1.Nodes.Add(nodeSrcOny);
				treeView1.Nodes.Add(nodeDstOny);
				treeView1.Nodes.Add(nodeEncodingMisMatch);
				treeView1.Nodes.Add(nodeError);
				treeView1.Nodes.Add(nodeDup);
				treeView1.Nodes.Add(nodeMisMatch);
				
				if (bAllPass)
				{
					tbCompareResult.Text = "全部比对通过!";
				}
				else
				{
					tbCompareResult.Text = "存在差异!!!";
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"内部错误: {ex.StackTrace}");
			}
		}

		private void cbSRCLang_SelectedIndexChanged(object sender, EventArgs e)
		{
			bPrepared = true;
		}

		private void cbDstLang_SelectedIndexChanged(object sender, EventArgs e)
		{
			bPrepared = true;
		}

		private void Form1_Load(object sender, EventArgs e)
		{

		}

		private void btSetSymbols_Click(object sender, EventArgs e)
		{
			using (FormConfig dialog = new FormConfig())
			{
				if(dialog.ShowDialog() == DialogResult.OK)
				{
					List<char> syms = dialog.SelectedSymbols;
					BuildPatterns(syms);
					string show1 = string.Join(", ",syms.ToArray());
					tbCurrentSymbols.Text = show1;
				}
			}
		}

		private void BuildPatterns(List<char> syms)
		{
			try
			{
				string delimiters = "";
				for (int i = 0; i < syms.Count; ++i)
				{
					char c = syms[i];
					if(c == '\"')
					{
						delimiters += @"""";
					}
					else if (c == '^' || c == '-' || c == '\\' || c == ']' || c == '[')
					{
						delimiters += $"\\{c}";
					}
					else
					{
						delimiters += c;
					}
				}
				string patFull = $@"@(-?\d+)(?:[^{delimiters}]*)([{delimiters}])((?!\2).*)\2";
				string patPartial = $@"@(-?\d+)(?:[^{delimiters}]*)([{delimiters}])((?!\2).*)";
				string patEnd = $@"[^{delimiters}]*([{delimiters}])$";
				string patEmptyLine = $@"[{delimiters}]";
				mPatterns.Clear();
				mPatterns.Add(patFull);
				mPatterns.Add(patPartial);
				mPatterns.Add(patEnd);
				mPatterns.Add(patEmptyLine);
			}
			catch (Exception ex)
			{
				SetCurrentWithDefault();
				MessageBox.Show($"构建正则错误: {ex.Message}, 使用默认模式");
			}
		}

		private void btDetectEncoding_Click(object sender, EventArgs e)
		{
			if(!CheckJob())
			{
				return;
			}
			string srcLangName = cbSRCLang.SelectedItem.ToString();
			string srcDirName = Path.Combine(selectedRoot, srcLangName);
			if (!Directory.Exists(srcDirName))
			{
				MessageBox.Show($"路径不存在: {srcDirName}");
				return;
			}
			string[] srcTras = Directory.GetFiles(srcDirName, "*.tra",SearchOption.AllDirectories);

			SortedDictionary<string, string> srcPaths = new SortedDictionary<string, string>();
			for (int i = 0; i < srcTras.Length; ++i)
			{
				var rela = GetRelativePath(srcDirName, srcTras[i]);
				if(!rela.flag)
				{
					MessageBox.Show(rela.p);
					return;
				}
				srcPaths.Add(rela.p, srcTras[i]);
			}

			TreeNode tNode1 = new TreeNode($"{srcLangName}编码检测结果");
			tNode1.Name = "Encoding";
			treeView1.Nodes.Add(tNode1);

			int totalCnt = srcPaths.Count;
			int okCnt = 0;
			foreach (var kvp in srcPaths)
			{
				try
				{
					string srcEnc = null;
					using (FileStream fs = File.OpenRead(kvp.Value))
					{
						Ude.CharsetDetector detector = new Ude.CharsetDetector();
						detector.Feed(fs);
						detector.DataEnd();
						srcEnc = detector.Charset;
					}


					TreeNode node = new TreeNode($"{kvp.Key} 编码: {srcEnc}");
					treeView1.Nodes["Encoding"].Nodes.Add(node);
					okCnt++;
				}
				catch(Exception ex)
				{
					TreeNode node = new TreeNode($"{kvp.Key} 检测失败: {ex.Message}");
				}
			}
			tbCompareResult.Text = $"{okCnt}/{totalCnt}编码检测完成";

		}

		private (bool flag, string p) GetRelativePath(string fromPath, string toPath)
		{
			if (string.IsNullOrEmpty(fromPath)) return (false, $"路径不存在: {fromPath}");
			if (string.IsNullOrEmpty(toPath)) return (false, $"路径不存在: {toPath}");
			

			// 规范化路径并添加目录分隔符
			fromPath = Path.GetFullPath(fromPath);
			toPath = Path.GetFullPath(toPath);

			// 检查是否在同一根目录下（如C:\或D:\）
			string fromRoot = Path.GetPathRoot(fromPath);
			string toRoot = Path.GetPathRoot(toPath);

			if (!string.Equals(fromRoot, toRoot, StringComparison.OrdinalIgnoreCase))
				return (true,toPath); // 不同根目录，返回绝对路径

			// 分割路径为目录数组
			string[] fromDirs = fromPath.Substring(fromRoot.Length).Split(
				new[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar },
				StringSplitOptions.RemoveEmptyEntries);

			string[] toDirs = toPath.Substring(toRoot.Length).Split(
				new[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar },
				StringSplitOptions.RemoveEmptyEntries);

			// 计算公共前缀长度
			int commonLength = 0;
			while (commonLength < fromDirs.Length &&
				   commonLength < toDirs.Length &&
				   string.Equals(fromDirs[commonLength], toDirs[commonLength], StringComparison.OrdinalIgnoreCase))
			{
				commonLength++;
			}

			// 构建相对路径
			var relativePath = new System.Text.StringBuilder();

			// 添加"../"直到回到公共目录
			for (int i = commonLength; i < fromDirs.Length; i++)
			{
				relativePath.Append("..");
				relativePath.Append(Path.DirectorySeparatorChar);
			}

			// 添加目标目录的剩余部分
			for (int i = commonLength; i < toDirs.Length; i++)
			{
				relativePath.Append(toDirs[i]);
				relativePath.Append(Path.DirectorySeparatorChar);
			}

			// 如果是文件路径，移除最后的分隔符
			if (!toPath.EndsWith(Path.DirectorySeparatorChar.ToString()) &&
				!toPath.EndsWith(Path.AltDirectorySeparatorChar.ToString()))
			{
				if (relativePath.Length > 0)
					relativePath.Length -= 1;
			}

			return (true,relativePath.ToString());
		}

		private void btRecoverPath_Click(object sender, EventArgs e)
		{
			if (!CheckJob())
			{
				return;
			}
			try
			{
				string srcLangName = cbSRCLang.SelectedItem.ToString();
				string dstLangName = cbDstLang.SelectedItem.ToString();

				string srcDirName = Path.Combine(selectedRoot, srcLangName);
				string dstDirName = Path.Combine(selectedRoot, dstLangName);

				if (!Directory.Exists(srcDirName))
				{
					MessageBox.Show($"路径不存在: {srcDirName}");
					return;
				}
				if (!Directory.Exists(dstDirName))
				{
					MessageBox.Show($"路径不存在: {dstDirName}");
					return;
				}

				string[] srcTras = Directory.GetFiles(srcDirName, "*.tra", SearchOption.AllDirectories);
				string[] dstTras = Directory.GetFiles(dstDirName, "*.tra", SearchOption.AllDirectories);

				Dictionary<string, Tuple<string, string>> srcPaths = new Dictionary<string, Tuple<string, string>>();
				Dictionary<string, Tuple<string, string>> dstPaths = new Dictionary<string, Tuple<string, string>>();


				List<string> srcHit = new List<string>();
				List<string> srcNotInDst = new List<string>();
				List<string> dstNotInSrc = new List<string>();

				TreeNode nodeDstNotInSrc = new TreeNode($"{dstLangName}有但{srcLangName}缺失(未移动)");
				nodeDstNotInSrc.Name = "DstNotInSrc";
				treeView1.Nodes.Add(nodeDstNotInSrc);
				TreeNode nodeSrcNotInDst = new TreeNode($"{srcLangName}有但{dstLangName}缺失(未移动)");
				nodeSrcNotInDst.Name = "SrcNotInDst";
				treeView1.Nodes.Add(nodeSrcNotInDst);
				TreeNode nodeDupTra = new TreeNode("重复TRA文件(未移动)");
				nodeDupTra.Name = "DupTra";
				treeView1.Nodes.Add(nodeDupTra);
				TreeNode nodeErrTra = new TreeNode("错误TRA文件(未移动)");
				nodeErrTra.Name = "ErrTra";
				treeView1.Nodes.Add(nodeErrTra);
				TreeNode nodeStats = new TreeNode("结果统计");
				nodeStats.Name = "Stats";
				treeView1.Nodes.Add(nodeStats);

				int dstCnt = dstTras.Length;
				int dirCreated = 0;
				int fileMoved = 0;
				int fileNotMoved = 0;
				int errCnt = 0;

				for (int i = 0; i < srcTras.Length; ++i)
				{
					var rela = GetRelativePath(srcDirName, srcTras[i]);
					if (!rela.flag)
					{
						errCnt++;
						nodeErrTra.Nodes.Add(new TreeNode($"无法获取相对路径: {srcTras[i]}"));
						continue;
					}
					string baseName = Path.GetFileName(rela.p);
					if (srcPaths.ContainsKey(baseName))
					{
						var tt = srcPaths[baseName];
						string nodeTxt = $"{srcLangName}{tt.Item1}已存在，{rela.p}请手工处理";
						treeView1.Nodes["DupTra"].Nodes.Add(new TreeNode(nodeTxt));
						continue;
					}
					else
					{
						srcPaths.Add(Path.GetFileName(rela.p), Tuple.Create(rela.p, srcTras[i]));
					}
				}
				for (int i = 0; i < dstTras.Length; ++i)
				{
					var rela = GetRelativePath(dstDirName, dstTras[i]);
					if (!rela.flag)
					{
						nodeErrTra.Nodes.Add(new TreeNode($"无法获取相对路径: {dstTras[i]}"));
						fileNotMoved++;
						errCnt++;
						continue;
					}
					if (dstPaths.ContainsKey(Path.GetFileName(rela.p)))
					{
						var tt = dstPaths[Path.GetFileName(rela.p)];
						string nodeTxt = $"{dstLangName}{tt.Item1}已存在，{rela.p}请手工处理";
						treeView1.Nodes["DupTra"].Nodes.Add(new TreeNode(nodeTxt));
						fileNotMoved++;
						continue;
					}
					else
					{
						dstPaths.Add(Path.GetFileName(rela.p), Tuple.Create(rela.p, dstTras[i]));
					}
				}

				

				foreach (var kvp in dstPaths)
				{
					string dstName = kvp.Key;
					string dstRelativePath = kvp.Value.Item1;
					if (!srcPaths.ContainsKey(dstName))
					{
						dstNotInSrc.Add(dstName);
						TreeNode node = new TreeNode(dstRelativePath);
						treeView1.Nodes["DstNotInSrc"].Nodes.Add(node);
						fileNotMoved++;
					}
				}


				foreach (var kvp in srcPaths)
				{
					string srcName = kvp.Key;
					if (dstPaths.ContainsKey(srcName))
					{
						srcHit.Add(srcName);
					}
					else
					{
						srcNotInDst.Add(srcName);
						TreeNode node = new TreeNode(kvp.Value.Item1);
						treeView1.Nodes["SrcNotInDst"].Nodes.Add(node);
						fileNotMoved++;
					}
				}

				foreach(string key in srcHit)
				{
					var pair_src = srcPaths[key];
					var pair_dst = dstPaths[key];

					string sub = Path.GetDirectoryName(pair_src.Item1);
					if (string.IsNullOrEmpty(sub))
					{
						fileNotMoved++;
						continue;
					}
					string sub_in_dst = Path.Combine(dstDirName, sub);
					if(!Directory.Exists(sub_in_dst))
					{
						try
						{
							Directory.CreateDirectory(sub_in_dst);
							dirCreated++;
							
						}
						catch (Exception ex)
						{
							MessageBox.Show($"创建目录失败: {sub_in_dst}, 错误: {ex.Message}");
							continue;
						}
					}
					string target_path = Path.Combine(sub_in_dst, key);
					if (!File.Exists(target_path))
					{
						File.Move(pair_dst.Item2, target_path);
						fileMoved++;
					}

				}



				if (nodeDstNotInSrc.Nodes.Count == 0)
				{
					nodeDstNotInSrc.Nodes.Add(new TreeNode("无"));
				}
				if (nodeSrcNotInDst.Nodes.Count == 0)
				{
					nodeSrcNotInDst.Nodes.Add(new TreeNode("无"));
				}
				if(nodeDupTra.Nodes.Count == 0)
				{
					nodeDupTra.Nodes.Add(new TreeNode("无"));
				}
				if(nodeErrTra.Nodes.Count == 0)
				{
					nodeErrTra.Nodes.Add(new TreeNode("无"));
				}
				nodeStats.Nodes.Add(new TreeNode($"目标语言{dstLangName}包含TRA数量: {srcPaths.Count}"));
				nodeStats.Nodes.Add(new TreeNode($"创建目录数: {dirCreated}"));
				nodeStats.Nodes.Add(new TreeNode($"移动文件数: {fileMoved}"));
				nodeStats.Nodes.Add(new TreeNode($"未移动文件数: {fileNotMoved}"));
				nodeStats.Nodes.Add(new TreeNode($"文件错误数: {errCnt}"));

				tbCompareResult.Text = "重建路径完成";
			}
			catch (Exception ex)
			{
				MessageBox.Show($"内部错误: {ex.StackTrace}");
				return;
			}
		}
	}
}
