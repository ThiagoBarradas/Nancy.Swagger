using System.Collections.Generic;

namespace Nancy.Swagger.Demo.Models
{
    public class Address
    {
        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string Town { get; set; }

        public string County { get; set; }

        public string PostCode { get; set; }

        public IEnumerable<Test> Testtt { get; set; }

        public List<Test2> Testtt2 { get; set; }

        public Dictionary<string, Test3> Testtt3 { get; set; }

        public HttpStatusCode StatusCode { get; set; }
    }
}