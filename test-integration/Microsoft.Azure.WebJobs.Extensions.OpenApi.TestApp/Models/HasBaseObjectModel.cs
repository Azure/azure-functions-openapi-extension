namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.TestApp.Models
{
    public class HasBaseObjectSubModel
    {
        public object SubObjectValue{ get; set; }
    }

    public class HasBaseObjectModel
    {
        public object ObjectValue { get; set; }
        public int NonObjectValue { get; set; }
        public HasBaseObjectSubModel SubModel { get; set; }
    }
}
