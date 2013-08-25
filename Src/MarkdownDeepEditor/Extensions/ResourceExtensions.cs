using System.Web.UI;
using ClientDependency.Core;

namespace Xilium.MarkdownDeepEditor4Umbraco.Extensions
{
	/// <summary>
	/// Extension methods for embedded resources.
	/// </summary>
	public static class ResourceExtensions
	{

		/// <summary>
		/// Gets a URL reference to a resource in an assembly.
		/// </summary>
		/// <param name="ctl">The control.</param>
		/// <param name="resourceName">Name of the resource.</param>
		/// <returns></returns>
		public static string GetWebResourceUrl(this Control ctl, string resourceName) {
			// get the urls for the embedded resources
			var resourceUrl = ctl.Page.ClientScript.GetWebResourceUrl(ctl.GetType(), resourceName);

			return resourceUrl;
		}

		/// <summary>
		/// Adds an embedded resource to the ClientDependency output by name
		/// </summary>
		/// <param name="ctl">The control.</param>
		/// <param name="resourceName">Name of the resource.</param>
		/// <param name="type">The type of resource.</param>
		public static void AddResourceToClientDependency(this Control ctl, string resourceName, ClientDependencyType type)
		{
			
			switch (type)
			{
				case ClientDependencyType.Css:
					// get the urls for the embedded resources
					var resourceUrl = GetWebResourceUrl(ctl, resourceName);
					ctl.Page.Header.Controls.Add(new LiteralControl("<link type='text/css' rel='stylesheet' href='" + resourceUrl + "'/>"));
					break;

				case ClientDependencyType.Javascript:
					ctl.Page.ClientScript.RegisterClientScriptResource(typeof(ResourceExtensions), resourceName);
					break;

				default:
					break;
			}
		}
	}
}
