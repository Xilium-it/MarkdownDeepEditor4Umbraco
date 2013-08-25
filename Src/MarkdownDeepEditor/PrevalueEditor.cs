﻿using System;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using Xilium.MarkdownDeepEditor4Umbraco.Extensions;
using umbraco.BusinessLogic;
using umbraco.cms.businesslogic.datatype;
using umbraco.interfaces;

[assembly: WebResource("Xilium.MarkdownDeepEditor4Umbraco.Resources.Styles.PrevalueEditor.css", "text/css", PerformSubstitution = true)]

namespace Xilium.MarkdownDeepEditor4Umbraco
{
	/// <summary>
	/// The Prevalue Editor for the Markdown data-type.
	/// </summary>
	public sealed class PrevalueEditor : WebControl, IDataPrevalue
	{
		/// <summary>
		/// The underlying base data-type.
		/// </summary>
		private readonly BaseDataType m_DataType;

		/// <summary>
		/// An object to temporarily lock writing to the database.
		/// </summary>
		private static readonly object m_Locker = new object();

		/// <summary>
		/// The CheckBox for enabling editing history on the WMD editor.
		/// </summary>
		private CheckBox EnableHistory;

		/// <summary>
		/// The CheckBox for enabling the WMD Editor.
		/// </summary>
		private CheckBox EnableWmd;

		/// <summary>
		/// The RadioButtonList for the preview settings.
		/// </summary>
		private RadioButtonList PreviewOptions;

		/// <summary>
		/// The RadioButtonList for the output formats.
		/// </summary>
		private RadioButtonList OutputFormats;

		/// <summary>
		/// The TextBox control for the height of the data-type.
		/// </summary>
		private TextBox TextBoxHeight;

		/// <summary>
		/// The TextBox control for the help URL.
		/// </summary>
		private TextBox TextBoxHelpUrl;

		/// <summary>
		/// The TextBox control for the width of the data-type.
		/// </summary>
		private TextBox TextBoxWidth;

		/// <summary>
		/// Initializes a new instance of the <see cref="PrevalueEditor"/> class.
		/// </summary>
		/// <param name="dataType">Type of the data.</param>
		/// <param name="dbType">Type of the db.</param>
		public PrevalueEditor(BaseDataType dataType, DBTypes dbType)
			: base()
		{
			this.m_DataType = dataType;
			this.m_DataType.DBType = dbType;
		}

		/// <summary>
		/// Gets the editor.
		/// </summary>
		/// <value>The editor.</value>
		public Control Editor
		{
			get
			{
				return this;
			}
		}

		/// <summary>
		/// Gets the PreValue options for the data-type.
		/// </summary>
		/// <typeparam name="T">The type of the resulting object.</typeparam>
		/// <returns>
		/// Returns the options for the PreValue Editor
		/// </returns>
		public T GetPreValueOptions<T>()
		{
			var prevalues = PreValues.GetPreValues(this.m_DataType.DataTypeDefinitionId);
			if (prevalues.Count > 0)
			{
				var prevalue = (PreValue)prevalues[0];
				if (!String.IsNullOrEmpty(prevalue.Value))
				{
					try
					{
						// deserialize the options
						var serializer = new JavaScriptSerializer();

						// return the options
						return serializer.Deserialize<T>(prevalue.Value);
					}
					catch (Exception ex)
					{
						Log.Add(LogTypes.Error, this.m_DataType.DataTypeDefinitionId, string.Concat("MarkdownDeep Editor: Execption thrown: ", ex.Message));
					}
				}
			}

			// if all else fails, return default options
			return default(T);
		}

		/// <summary>
		/// Saves this instance.
		/// </summary>
		public void Save()
		{
			// set the options
			var options = new Options(true)
			{
				EnableHistory = this.EnableHistory.Checked,
				EnableWmd = this.EnableWmd.Checked,
				HelpUrl = this.TextBoxHelpUrl.Text,
				OutputFormat = (Options.OutputFormats)this.OutputFormats.SelectedIndex,
				SelectedPreview = this.PreviewOptions.SelectedValue
			};

			// parse the height
			int height, width;
			if (int.TryParse(this.TextBoxHeight.Text, out height))
			{
				if (height == 0)
				{
					height = 300;
				}

				options.Height = height;
			}

			// parse the width
			if (int.TryParse(this.TextBoxWidth.Text, out width))
			{
				if (width == 0)
				{
					width = 525;
				}

				options.Width = width;
			}

			// save the options as JSON
			this.SaveAsJson(options);
		}

		/// <summary>
		/// Saves the data-type PreValue options.
		/// </summary>
		/// <param name="options">The PreValue options.</param>
		public void SaveAsJson(object options)
		{
			// serialize the options into JSON
			var serializer = new JavaScriptSerializer();
			var json = serializer.Serialize(options);

			lock (m_Locker)
			{
				var prevalues = PreValues.GetPreValues(this.m_DataType.DataTypeDefinitionId);
				if (prevalues.Count > 0)
				{
					PreValue prevalue = (PreValue)prevalues[0];

					// update
					prevalue.Value = json;
					prevalue.Save();
				}
				else
				{
					// insert
					PreValue.MakeNew(this.m_DataType.DataTypeDefinitionId, json);
				}
			}
		}

		/// <summary>
		/// Renders the HTML opening tag of the control to the specified writer. This method is used primarily by control developers.
		/// </summary>
		/// <param name="writer">A <see cref="T:System.Web.UI.HtmlTextWriter"/> that represents the output stream to render HTML content on the client.</param>
		public override void RenderBeginTag(HtmlTextWriter writer)
		{
			writer.AddAttribute(HtmlTextWriterAttribute.Class, "settings");
			writer.RenderBeginTag(HtmlTextWriterTag.Div); //// start 'settings'

			base.RenderBeginTag(writer);
		}

		/// <summary>
		/// Renders the HTML closing tag of the control into the specified writer. This method is used primarily by control developers.
		/// </summary>
		/// <param name="writer">A <see cref="T:System.Web.UI.HtmlTextWriter"/> that represents the output stream to render HTML content on the client.</param>
		public override void RenderEndTag(HtmlTextWriter writer)
		{
			base.RenderEndTag(writer);

			writer.RenderEndTag(); //// end 'settings'
		}

		/// <summary>
		/// Raises the <see cref="E:System.Web.UI.Control.Init"/> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);

			this.EnsureChildControls();

			// Adds the client dependencies.
			this.AddResourceToClientDependency("Xilium.MarkdownDeepEditor4Umbraco.Resources.Styles.PrevalueEditor.css", ClientDependency.Core.ClientDependencyType.Css);
		}

		/// <summary>
		/// Creates child controls for this control
		/// </summary>
		protected override void CreateChildControls()
		{
			base.CreateChildControls();

			// set-up child controls
			this.EnableHistory = new CheckBox() { ID = "EnableHistory" };
			this.EnableWmd = new CheckBox() { ID = "EnableWmd" };
			this.PreviewOptions = new RadioButtonList() { ID = "PreviewOptions", RepeatDirection = RepeatDirection.Vertical, RepeatLayout = RepeatLayout.Flow };
			this.OutputFormats = new RadioButtonList() { ID = "OutputFormats", RepeatDirection = RepeatDirection.Vertical, RepeatLayout = RepeatLayout.Flow };
			this.TextBoxHeight = new TextBox() { ID = "Height", CssClass = "guiInputText" };
			this.TextBoxHelpUrl = new TextBox() { ID = "TextBoxHelpUrl", CssClass = "guiInputText umbEditorTextField" };
			this.TextBoxWidth = new TextBox() { ID = "Width", CssClass = "guiInputText" };

			// populate the controls
			var items = new ListItem[]
			{
				new ListItem("Disabled - No preview will appear", string.Empty),
				new ListItem("Toolbar - Adds Compose/Preview toggle buttons to the toolbar (default)", "toolbar"),
				new ListItem("Above - Auto-updated preview to appear above the editor", "above"),
				new ListItem("Below - Auto-updated preview to appear below the editor", "below")
			};
			this.PreviewOptions.Items.AddRange(items);

			items = new ListItem[]
			{
				new ListItem("HTML - Renders the Markdown text as HTML, stored as CDATA text.", "0"),
				new ListItem("XML - Renders the Markdown text as HTML, stored as raw XML.", "1"),
				new ListItem("Markdown - Outputs the raw Markdown, stored as CDATA text.", "2")
			};
			this.OutputFormats.Items.AddRange(items);

			// add the child controls
			this.Controls.Add(this.EnableHistory);
			this.Controls.Add(this.EnableWmd);
			this.Controls.Add(this.PreviewOptions);
			this.Controls.Add(this.OutputFormats);
			this.Controls.Add(this.TextBoxHeight);
			this.Controls.Add(this.TextBoxHelpUrl);
			this.Controls.Add(this.TextBoxWidth);
		}

		/// <summary>
		/// Raises the <see cref="E:System.Web.UI.Control.Load"/> event.
		/// </summary>
		/// <param name="e">The <see cref="T:System.EventArgs"/> object that contains the event data.</param>
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			// get PreValues, load them into the controls.
			var options = this.GetPreValueOptions<Options>();

			// no options? use the default ones.
			if (options == null)
			{
				options = new Options(true);
			}

			// set the values
			this.EnableHistory.Checked = options.EnableHistory;
			this.EnableWmd.Checked = options.EnableWmd;
			this.PreviewOptions.SelectedValue = options.SelectedPreview;
			this.OutputFormats.SelectedIndex = options.SaveAsXml ? 1 : (int)options.OutputFormat;
			this.TextBoxHeight.Text = options.Height.ToString();
			this.TextBoxHelpUrl.Text = options.HelpUrl;
			this.TextBoxWidth.Text = options.Width.ToString();
		}

		/// <summary>
		/// Renders the contents of the control to the specified writer. This method is used primarily by control developers.
		/// </summary>
		/// <param name="writer">A <see cref="T:System.Web.UI.HtmlTextWriter"/> that represents the output stream to render HTML content on the client.</param>
		protected override void RenderContents(HtmlTextWriter writer)
		{
			// add property fields
			writer.AddPrevalueRow("Height:", this.TextBoxHeight);
			writer.AddPrevalueRow("Width:", this.TextBoxWidth);
			writer.AddPrevalueRow("Output Format:", this.OutputFormats);
			writer.AddPrevalueRow("Enable WMD editor:", "The WMD editor is powered by <a href='http://tstone.github.com/jquery-markedit/' target='_blank'>MarkEdit</a> by Titus Stone.", this.EnableWmd);

			// configuration options - http://wiki.github.com/tstone/jquery-markedit/configuration-options
			writer.AddPrevalueRow(string.Empty, "The following options are only applicable when the WMD editor is enabled.");
			writer.AddPrevalueRow("Preview:", this.PreviewOptions);
			writer.AddPrevalueRow("Enable history:", "This enables/disables the undo/redo editing history for the WMD editor.", this.EnableHistory);
			writer.AddPrevalueRow("Help URL:", "Override the help URL for the WMD editor.", this.TextBoxHelpUrl);
		}
	}
}
