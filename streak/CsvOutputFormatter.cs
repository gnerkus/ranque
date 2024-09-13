using System.Text;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using Shared;

namespace streak
{
    public class CsvOutputFormatter : TextOutputFormatter
    {
        public CsvOutputFormatter()
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/csv"));
            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);
        }

        protected override bool CanWriteType(Type? type)
        {
            if (typeof(OrganizationDto).IsAssignableFrom(type) ||
                typeof(IEnumerable<OrganizationDto>).IsAssignableFrom(type))
                return base.CanWriteType(type);
            return false;
        }

        public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context,
            Encoding selectedEncoding)
        {
            var response = context.HttpContext.Response;
            var buffer = new StringBuilder();
            if (context.Object is IEnumerable<OrganizationDto> dtos)
                foreach (var company in dtos)
                    FormatCsv(buffer, company);
            else
                FormatCsv(buffer, (OrganizationDto)context.Object);

            await response.WriteAsync(buffer.ToString());
        }

        private static void FormatCsv(StringBuilder buffer, OrganizationDto org)
        {
            buffer.AppendLine($"{org.Id},\"{org.Name},\"{org.FullAddress}\"");
        }
    }
}