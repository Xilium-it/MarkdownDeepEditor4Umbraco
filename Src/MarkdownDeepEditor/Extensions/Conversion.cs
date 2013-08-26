using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xilium.MarkdownDeepEditor4Umbraco.Extensions {
	public static class Conversion {


		public static int ParseToInt(this string value, int fallbackValue) {
			int fReturn;
			if (int.TryParse(value, out fReturn)) return fReturn;
			return fallbackValue;
		}

		public static T ParseToEnum<T>(this string value, T fallbackValue) where T : struct, System.IConvertible {
			if (typeof(T).IsEnum == false) {
				throw new ArgumentException("T must be an enumerated type");
			}

			T fReturn;
			try {
				fReturn = (T)Enum.Parse(typeof(T), value, true);
			} catch (Exception ex) {
				fReturn = fallbackValue;
			}
			return fReturn;
		}



		public static string ToJson(this bool value) {
			return value ? "true" : "false";
		}
	}
}
