using System.IO;
using System.Reflection;
using System.Threading.Tasks;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.FunctionApp.V3Static.Configurations
{
    public class CustomUiProvider : DefaultUiCustomizationProvider
    {
        public override async Task<string> GetStylesheetAsync()
        {
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"{typeof(CustomUiProvider).Assembly.GetName().Name}.dist.custom.css"))
            using (var reader = new StreamReader(stream))
            {
                var customCss = await reader.ReadToEndAsync().ConfigureAwait(false);
                return customCss;
            }
        }
    }
}
