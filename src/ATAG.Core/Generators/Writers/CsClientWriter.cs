using ATAG.Core.Models;
using ATAG.Core.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATAG.Core.Generators.Writers
{
	public class CsClientWriter : BaseWriter
	{

		public CsClientWriter(string @namespace) : base(@namespace)
		{ }

		public override string GenerateContent(object entity)
		{
			var controllers = (List<ControllerModel>)entity;

			StringBuilder sb = new StringBuilder();
			int tabLevel = 0;

			sb.AppendLine("using System;");
			sb.AppendLine("using System.Collections.Generic;");
			sb.AppendLine("using System.Text;");
			sb.AppendLine("using System.Net.Http;");
			sb.AppendLine("using System.Text.Json;");
			sb.AppendLine("using System.Linq;");
			sb.AppendLine("using System.Threading.Tasks;");
			sb.AppendLine();

			sb.AppendLine($"namespace {_namespace}");
			sb.AppendLine("{");

			tabLevel++;

			sb.AppendLine($"{Tabs(tabLevel)}public class ApiClient");
			sb.AppendLine($"{Tabs(tabLevel)}{{");
			sb.AppendLine();

			tabLevel++;

			sb.AppendLine($"{Tabs(tabLevel)}private readonly HttpClient _httpClient;");
			sb.AppendLine($"{Tabs(tabLevel)}private readonly string _hostUrl;");

			sb.AppendLine();

			sb.AppendLine($"{Tabs(tabLevel)}public ApiClient(HttpClient httpClient, string hostUrl)");
			sb.AppendLine($"{Tabs(tabLevel)}{{");
			tabLevel++;
			sb.AppendLine($"{Tabs(tabLevel)}_httpClient = httpClient;");
			sb.AppendLine($"{Tabs(tabLevel)}_hostUrl = hostUrl;");
			tabLevel--;
			sb.AppendLine($"{Tabs(tabLevel)}}}");
			sb.AppendLine();

			foreach(var controller in controllers)
			{
				sb.AppendLine($"{Tabs(tabLevel)}#region {controller.Name}");
				sb.AppendLine();

				foreach(var method in controller.Methods)
				{
					sb.Append($"{Tabs(tabLevel)}public async Task<{method.ReturnedType}> {method.Name}(");

					bool hasBodyParameter = !string.IsNullOrEmpty(method.Parameters.BodyParameter.Key);

					if (hasBodyParameter)
					{
						var bp = method.Parameters.BodyParameter;
						sb.Append($"{bp.Key} {bp.Value}");
					}

					if (method.Parameters.QueryParameters.Count > 0)
					{
						if (hasBodyParameter)
							sb.Append(", ");

						foreach (var qp in method.Parameters.QueryParameters)
						{
							sb.Append($"{qp.Key} {qp.Value}, ");
						}
						sb.Length -= 2;
					}
					sb.Append(")");
					sb.AppendLine();

					sb.AppendLine($"{Tabs(tabLevel)}{{");

					tabLevel++;

					var controllerName = controller.Name.EndsWith("Controller") ?
						controller.Name.Substring(0, controller.Name.Length - 1 - "Controller".Length)
						: controller.Name;

					StringBuilder url = new StringBuilder($"_hostUrl + \"/api/{controllerName}");
					if (method.Attributes.TryGetValue("Route", out string route))
					{
						url.Append("/" + route);
					}
					url.Append("\"");
					if (method.Parameters.QueryParameters.Count > 0)
					{
						url.Append("+\"?");
						foreach (var qp in method.Parameters.QueryParameters)
						{
							url.Append($"{qp.Value}=\"+{qp.Value}.ToString()+\"&");
						}
						url.Length -= 3;
						url.Append(";");
					}

					sb.AppendLine($"{Tabs(tabLevel)}var url = {url}");
					sb.AppendLine();

					sb.Append($"{Tabs(tabLevel)}var result = await {_verbsMethods[method.Verb]}<{method.ReturnedType}>(url");

					if (hasBodyParameter)
					{
						sb.Append($", {method.Parameters.BodyParameter.Value}");
					}
					sb.Append(");");
                    sb.AppendLine();
					sb.AppendLine($"{Tabs(tabLevel)}return result");
					tabLevel--;
					sb.AppendLine($"{Tabs(tabLevel)}}}");
				}
				sb.AppendLine();
				sb.AppendLine($"{Tabs(tabLevel)}#endregion");
			}

			GeneratePrivateGeneralMethods(sb, tabLevel);
            sb.AppendLine();

			tabLevel--;
			sb.AppendLine($"{Tabs(tabLevel)}}}");
			tabLevel--;
			sb.AppendLine($"{Tabs(tabLevel)}}}");

			return sb.ToString();
		}

		private void GeneratePrivateGeneralMethods(StringBuilder sb, int tabLevel)
		{
			var getMethod = @"		
		private async Task<T> GetPrivateMethod<T>(string url)
		{
			var response = await _client.GetAsync(url);
			response.EnsureSuccessStatusCode();

			var result = JsonSerializer.Deserialize<T>(response.Content.ReadAsStringAsync().Result);

			return result;
		}";
			sb.Append(getMethod);

			var postMethod = @"
		private async Task<T> PostPrivateMethod<T>(string url, object data = null)
		{
			HttpContent content = new StringContent(JsonSerializer.Serialize(data ?? """"), Encoding.UTF8, ""application/json"");

			var response = await _client.PostAsync(url, content);
			response.EnsureSuccessStatusCode();

			var result = JsonSerializer.Deserialize<T>(response.Content.ReadAsStringAsync().Result);
			return result;
		}";
			sb.Append(postMethod);

			var putMethod = @"
		private async Task<T> PutPrivateMethod<T>(string url, object data = null)
		{
			HttpContent content = new StringContent(JsonSerializer.Serialize(data ?? """"), Encoding.UTF8, ""application/json"");

			var response = await _client.PutAsync(url, content);
			response.EnsureSuccessStatusCode();

			var result = JsonSerializer.Deserialize<T>(response.Content.ReadAsStringAsync().Result);
			return result;
		}";
			sb.Append(putMethod);

			var deleteMethod = @"		
		private async Task<T> DeletePrivateMethod<T>(string url)
		{
			var response = await _client.DeleteAsync(url);
			response.EnsureSuccessStatusCode();

			var result = JsonSerializer.Deserialize<T>(response.Content.ReadAsStringAsync().Result);

			return result;
		}";
			sb.Append(deleteMethod);
		}

		private Dictionary<HttpVerb, string> _verbsMethods =>
			new Dictionary<HttpVerb, string>
			{
				[HttpVerb.get] = "GetPrivateMethod",
				[HttpVerb.post] = "PostPrivateMethod",
				[HttpVerb.put] = "PutPrivateMethod",
				[HttpVerb.delete] = "DeletePrivateMethod",
			};
	}
}
