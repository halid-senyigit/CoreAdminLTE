using Microsoft.Extensions.Configuration;

namespace CoreAdminLTE.Services
{
    public class EmailModel
    {
        public string Subject { get; set; }

        public string To { get; set; }

        public string Body { get; set; }

    }
}