using System;
using System.Collections.Generic;
using System.Linq;

namespace Xilium.MarkdownDeepEditor4Umbraco.MarkdownFormatter {
	public class MarkdownFormatterDriver {
		private static Type __defaultMarkdownFormatterType;
		private static MarkdownFormatterBase __defaultMarkdownFormatterInstance = null;
		private static IEnumerable<Type> __markdownFormatterTypes;
		private static List<MarkdownFormatterBase> __markdownFormatterInstances = null;

		static MarkdownFormatterDriver() {
			// Ricerco tutte le classi che implementano [MarkdownFormatBase]
			var tipoBase = typeof(MarkdownFormatterBase);
			__markdownFormatterTypes = AppDomain.CurrentDomain.GetAssemblies()
			                                .SelectMany(asm => asm.GetTypes())
											.Where(t => tipoBase.IsAssignableFrom(t));

			// Ora cerco il tipo MarkdownFormatter di default
			var tipoAttrDefault = typeof (DefaultMarkdownFormatterAttribute);
			__defaultMarkdownFormatterType = __markdownFormatterTypes.FirstOrDefault(t => Attribute.IsDefined(t, tipoAttrDefault));
			if (__defaultMarkdownFormatterType == null) __defaultMarkdownFormatterType = typeof(XiliumMarkdownDeepFormatter);

			// Istanzio l'oggetto.
			__defaultMarkdownFormatterInstance = Activator.CreateInstance(__defaultMarkdownFormatterType) as MarkdownFormatterBase;
		}


		/// <summary>
		/// Restituisce il tipo oggetto MarkdownFormatter di default.
		/// </summary>
		public static Type DefaultMarkdownFormatterType {
			get { return __defaultMarkdownFormatterType; }
		}

		/// <summary>
		/// Restituisce l'elenco di tutti i tipi oggetto MarkdownFormatter disponibili.
		/// </summary>
		public static IEnumerable<Type> MarkdownFormatterTypes {
			get { return __markdownFormatterTypes; }
		}


		/// <summary>
		/// Restituisce il MarkdownFormatter di default.
		/// </summary>
		public static MarkdownFormatterBase DefaultMarkdownFormatter {
			get { return __defaultMarkdownFormatterInstance; }
		}

		/// <summary>
		/// Restituisce l'elenco di tutti i MarkdownFormatter disponibili.
		/// </summary>
		public static List<MarkdownFormatterBase> MarkdownFormatters {
			get {
				if (__markdownFormatterInstances == null) {
					lock (__markdownFormatters_locker) {
						//   Perchè due IF?
						// Il 1° è per performance: è probabile che sia FALSE. Il 2° è per eventuale doppio thread su Lock, il primo thread esegue la creazione mentre il secondo aspetta thread;
						// quando il primo termina il secondo ha libero accesso, e senza il secondo IF il secondo thread rieseguirebbe il codice di generazione.
						//   Perchè List<MarkdownFormatterBase> invece di IEnumerable<MarkdownFormatterBase>?
						// Perchè un oggetto IEnumerable è una sorta di funzione, e l'esecuzione viene tardata al momento dell'effettiva richiesta. Questo significa che potrei ottenere la creazione
						// doppia del medesimo MarkdownFormatter.
						if (__markdownFormatterInstances == null) {
							__markdownFormatterInstances = new List<MarkdownFormatterBase>(from t in __markdownFormatterTypes
																					   where t != __defaultMarkdownFormatterType
																					   select Activator.CreateInstance(t) as MarkdownFormatterBase);
							if (__defaultMarkdownFormatterInstance != null) __markdownFormatterInstances.Add(__defaultMarkdownFormatterInstance);
						}
					}
				}
				return __markdownFormatterInstances;
			}
		}
		private static object __markdownFormatters_locker = new object();

	}





}
