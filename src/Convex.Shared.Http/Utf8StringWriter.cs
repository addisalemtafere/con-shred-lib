using System.Text;

namespace Convex.Shared.Http
{
    public sealed class Utf8StringWriter : StringWriter
    {
        public override Encoding Encoding => new UTF8Encoding(false);
    }
}
