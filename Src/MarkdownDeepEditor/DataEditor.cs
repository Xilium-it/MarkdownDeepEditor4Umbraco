using System;
using Xilium.MarkdownDeepEditor4Umbraco.TextFormatter;
using umbraco.cms.businesslogic.datatype;
using umbraco.interfaces;

namespace Xilium.MarkdownDeepEditor4Umbraco {
	/// <summary>
	/// The DataEditor for the MarkdownDeep Editor data-type.
	/// </summary>
	public sealed class DataEditor : AbstractDataEditor {
		/// <summary>
		/// The WMD/Markdown control.
		/// </summary>
		private EditorUIControl m_Control = new EditorUIControl();

		/// <summary>
		/// The Data object for the data-type.
		/// </summary>
		private IData m_Data;

		/// <summary>
		/// The options from the prevalue editor.
		/// </summary>
		private Options m_Options;

		/// <summary>
		/// The PreValue Editor for the data-type.
		/// </summary>
		private PrevalueEditor m_PreValueEditor;

		/// <summary>
		/// The TextFormatter object that will be the text transform to HTML.
		/// </summary>
		private TextFormatter.TextFormatterBase m_TextFormatter;

		/// <summary>
		/// Initializes a new instance of the <see cref="DataEditor"/> class.
		/// </summary>
		public DataEditor()
			: base() {
			// set the render control as the placeholder
			this.RenderControl = this.m_Control;

			// assign the initialise event for the placeholder
			this.m_Control.Init += new EventHandler(this.m_Control_Init);

			// assign the save event for the data-type/editor
			this.DataEditorControl.OnSave += new AbstractDataEditorControl.SaveEventHandler(this.DataEditorControl_OnSave);

			this.m_TextFormatter = null;
		}

		/// <summary>
		/// Gets the id of the data-type.
		/// </summary>
		/// <value>The id of the data-type.</value>
		public override Guid Id {
			get { return new Guid("86C1F03D-0AD8-464B-9CFD-B360839CA184"); }
		}

		/// <summary>
		/// Gets the name of the data type.
		/// </summary>
		/// <value>The name of the data type.</value>
		public override string DataTypeName {
			get { return "MarkdownDeep Editor"; }
		}

		/// <summary>
		/// Gets the data for the data-type.
		/// </summary>
		/// <value>The data for the data-type.</value>
		public override IData Data {
			get {
				if (this.m_Data == null) {
					switch (this.Options.OutputFormat) {
						case Options.OutputFormats.XML:
							this.m_Data = new XmlData(this);
							break;

						case Options.OutputFormats.Markdown:
							this.m_Data = new DefaultData(this);
							break;

						case Options.OutputFormats.HTML:
						default:
							this.m_Data = new TextData(this);
							break;
					}
				}

				return this.m_Data;
			}
		}

		/// <summary>
		/// Gets the options.
		/// </summary>
		/// <value>The options.</value>
		public Options Options {
			get {
				if (this.m_Options == null) {
					this.m_Options = ((PrevalueEditor)this.PrevalueEditor).GetPreValueOptions<Options>();
				}

				return this.m_Options;
			}
		}

		/// <summary>
		/// Gets the TextFormatter object
		/// </summary>
		public TextFormatter.TextFormatterBase TextFormatter {
			get {
				if (this.m_TextFormatter == null) {
					this.m_TextFormatter = TextFormatterDriver.CreateDefaultTextFormatterInstance(this, this.Options);
				}
				return this.m_TextFormatter;
			}
		}

		/// <summary>
		/// Gets the prevalue editor.
		/// </summary>
		/// <value>The prevalue editor.</value>
		public override IDataPrevalue PrevalueEditor {
			get {
				if (this.m_PreValueEditor == null) {
					this.m_PreValueEditor = new PrevalueEditor(this, DBTypes.Ntext);
				}

				return this.m_PreValueEditor;
			}
		}

		/// <summary>
		/// Handles the Init event of the m_Placeholder control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		private void m_Control_Init(object sender, EventArgs e) {
			// get the options for the data-type.
			this.m_Control.Options = this.Options;

			// set the value of the control
			this.m_Control.Text = (this.Data.Value != null ? this.Data.Value.ToString() : String.Empty);
		}

		/// <summary>
		/// Saves the editor control value.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		private void DataEditorControl_OnSave(EventArgs e) {
			// save the value of the control
			this.Data.Value = this.m_Control.Text;
		}
	}
}