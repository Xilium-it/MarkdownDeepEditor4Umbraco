using System;
using System.Collections.Generic;
using System.Linq;

namespace Xilium.MarkdownDeepEditor4Umbraco.TextFormatter {
	public class TextFormatterDriver {
		private static readonly Type __defaultTextFormatterType;
		private static readonly TextFormatterBase __defaultTextFormatterInstance = null;
		private static readonly IEnumerable<Type> __textFormatterTypes;
		private static List<TextFormatterBase> __textFormatterInstances = null;

		static TextFormatterDriver() {
			// Ricerco tutte le classi che implementano [TextFormatterBase]
			var tipoBase = typeof(TextFormatterBase);
			__textFormatterTypes = AppDomain.CurrentDomain.GetAssemblies()
			                                .SelectMany(asm => asm.GetTypes())
											.Where(t => tipoBase.IsAssignableFrom(t));

			// Ora cerco il tipo TextFormatter di default
			var tipoAttrDefault = typeof (DefaultTextFormatterAttribute);
			__defaultTextFormatterType = __textFormatterTypes.FirstOrDefault(t => Attribute.IsDefined(t, tipoAttrDefault));
			if (__defaultTextFormatterType == null) __defaultTextFormatterType = typeof(XiliumMarkdownDeepFormatter);

			// Istanzio l'oggetto.
			__defaultTextFormatterInstance = Activator.CreateInstance(__defaultTextFormatterType) as TextFormatterBase;
		}


		/// <summary>
		/// Restituisce il tipo oggetto TextFormatter di default.
		/// </summary>
		public static Type DefaultTextFormatterType {
			get { return __defaultTextFormatterType; }
		}

		/// <summary>
		/// Restituisce l'elenco di tutti i tipi oggetto TextFormatter disponibili.
		/// </summary>
		public static IEnumerable<Type> TextFormatterTypes {
			get { return __textFormatterTypes; }
		}


		/// <summary>
		/// Restituisce il TextFormatter di default.
		/// </summary>
		public static TextFormatterBase DefaultTextFormatter {
			get { return __defaultTextFormatterInstance; }
		}

		/// <summary>
		/// Restituisce l'elenco di tutti i TextFormatter disponibili.
		/// </summary>
		public static List<TextFormatterBase> TextFormatters {
			get {
				if (__textFormatterInstances == null) {
					lock (__textFormatters_locker) {
						//   Perchè due IF?
						// Il 1° è per performance: è probabile che sia FALSE. Il 2° è per eventuale doppio thread su Lock, il primo thread esegue la creazione mentre il secondo aspetta thread;
						// quando il primo termina il secondo ha libero accesso, e senza il secondo IF il secondo thread rieseguirebbe il codice di generazione.
						//   Perchè List<TextFormatterBase> invece di IEnumerable<TextFormatterBase>?
						// Perchè un oggetto IEnumerable è una sorta di funzione, e l'esecuzione viene tardata al momento dell'effettiva richiesta. Questo significa che potrei ottenere la creazione
						// doppia del medesimo TextFormatter.
						if (__textFormatterInstances == null) {
							__textFormatterInstances = new List<TextFormatterBase>(from t in __textFormatterTypes
																				   where t != __defaultTextFormatterType
																				   select Activator.CreateInstance(t) as TextFormatterBase);
							if (__defaultTextFormatterInstance != null) __textFormatterInstances.Add(__defaultTextFormatterInstance);
						}
					}
				}
				return __textFormatterInstances;
			}
		}
		private static object __textFormatters_locker = new object();

	}





}
