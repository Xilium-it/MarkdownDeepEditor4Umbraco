using System;
using System.Collections.Generic;
using System.Linq;

namespace Xilium.MarkdownDeepEditor4Umbraco.TextFormatter {
	public class TextFormatterDriver {
		private static readonly Type __defaultTextFormatterType;
		private static readonly IEnumerable<Type> __textFormatterTypes;

		static TextFormatterDriver() {
			// Find all classes that inherits by [TextFormatterBase]
			var tipoBase = typeof(TextFormatterBase);
			__textFormatterTypes = AppDomain.CurrentDomain.GetAssemblies()
			                                .SelectMany(asm => asm.GetTypes())
											.Where(t => tipoBase.IsAssignableFrom(t));

			// Now find the TextFormatter class with DefaultTextFormatterAttribute attribute (the default TextFormatter)
			var tipoAttrDefault = typeof (DefaultTextFormatterAttribute);
			__defaultTextFormatterType = __textFormatterTypes.FirstOrDefault(t => Attribute.IsDefined(t, tipoAttrDefault));
			if (__defaultTextFormatterType == null) __defaultTextFormatterType = typeof(XiliumMarkdownDeepFormatter);
		}


		/// <summary>
		/// Returns the default TextFormatter class.
		/// </summary>
		public static Type DefaultTextFormatterType {
			get { return __defaultTextFormatterType; }
		}

		/// <summary>
		/// Returns list of available TextFormatter class.
		/// </summary>
		public static IEnumerable<Type> TextFormatterTypes {
			get { return __textFormatterTypes; }
		}

		public static TextFormatterBase CreateInstance(Type textFormatterType, DataEditor dataEditor, Options options) {
			return Activator.CreateInstance(textFormatterType, dataEditor, options) as TextFormatterBase;
		}

		public static TextFormatterBase CreateDefaultTextFormatterInstance(DataEditor dataEditor, Options options) {
			return CreateInstance(TextFormatterDriver.DefaultTextFormatterType, dataEditor, options);
		}

	}





}
