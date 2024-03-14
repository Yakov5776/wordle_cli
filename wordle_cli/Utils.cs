using System.Reflection;

namespace wordle_cli
{
    internal class Utils
    {
        public static string ReadResource<TSource>(string embeddedFileName) where TSource : class
        {
            var assembly = typeof(TSource).GetTypeInfo().Assembly;
            var resourceName = assembly.GetManifestResourceNames().First(s => s.EndsWith(embeddedFileName, StringComparison.CurrentCultureIgnoreCase));

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null) throw new InvalidOperationException();
                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }

    public static class InputExtensions
    {
        public static int Clamp(
            this int value, int Min, int Max)
        {
            if (value < Min) { return Min; }
            if (value > Max) { return Max; }
            return value;
        }
    }
}
