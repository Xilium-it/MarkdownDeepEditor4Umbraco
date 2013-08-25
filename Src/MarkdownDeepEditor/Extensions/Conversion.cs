using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xilium.MarkdownDeepEditor4Umbraco.Extensions {
	public static class Conversion {

		public static string ToJson(this bool value) {
			return value ? "true" : "false";
		}
	}
}
