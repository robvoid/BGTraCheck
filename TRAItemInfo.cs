using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGTraChecker
{
	internal class TRAItemInfo
	{
		public int TRAId { get; set; } = -1;
		public string Lang { get; set; } = null;
		public int StartLineNum { get; set; } = -1;
		public int LineCount { get; set; } = 0;
		public bool Correct { get; set; } = false;		
		public string[] RawLines { get; set; } = null;
		public List<string> DupLines { get; set; } = new List<string>();
	}
}
