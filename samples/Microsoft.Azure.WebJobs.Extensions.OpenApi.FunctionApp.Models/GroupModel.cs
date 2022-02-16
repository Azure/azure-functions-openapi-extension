using System;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.FunctionApp.Models
{
    public class GroupModel
    {
        public GroupModel ChildGroup { get; set; }
        public UserModel Owner { get; set; }
        public UserModel CoOwner { get; set; }
    }

    public class UserModel
    {
        public Guid Id { get; set; }
    }
}
