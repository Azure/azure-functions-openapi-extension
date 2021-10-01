using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.FunctionApp.Models
{
    public class DummyListModel
    {
        public List<DummyStringModel> ListValues1 { get; set; }

        public HashSet<int> ListValues2 { get; set; }

        public ISet<DummyStringModel> ListValues3 { get; set; }

        public ValidationProblemDetails ValidationProblemDetails { get; set; }
    }
}
