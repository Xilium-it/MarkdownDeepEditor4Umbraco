using System.Xml;
using Xilium.MarkdownDeepEditor4Umbraco.TextFormatter;
using umbraco.cms.businesslogic.datatype;

namespace Xilium.MarkdownDeepEditor4Umbraco
{
	/// <summary>
	/// Overrides the <see cref="TextData"/> object to return the value as XML.
	/// </summary>
	public class XmlData : TextData
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="XmlData"/> class.
		/// </summary>
		/// <param name="dataType">Type of the data.</param>
		public XmlData(BaseDataType dataType)
			: base(dataType)
		{
		}

		/// <summary>
		/// Transforms the Markdown value to HTML.
		/// </summary>
		/// <param name="data">The data (Markdown) to transform into HTML.</param>
		/// <returns>Returns an XML representation of the data.</returns>
		public override XmlNode ToXMl(XmlDocument data)
		{
			// check that the value isn't null
			if (this.Value != null && !string.IsNullOrEmpty(this.Value.ToString()))
			{
				// transform the markdown into HTML.
				var mddDataEditor = (DataEditor)this._dataType;
				string output = mddDataEditor.TextFormatter.Transform(this.Value.ToString());
				
				// load the HTML into an XML document.
				var xd = new XmlDocument();
				xd.LoadXml(string.Concat("<html>", output, "</html>"));

				// return the XML node.
				return data.ImportNode(xd.DocumentElement, true);
			}
			else
			{
				// otherwise render the value as default (in CDATA)
				return base.ToXMl(data);
			}
		}
	}
}