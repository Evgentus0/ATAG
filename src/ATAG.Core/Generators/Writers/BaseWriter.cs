using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ATAG.Core.Generators.Writers
{
    public abstract class BaseWriter
    {
        protected readonly string _namespace;

        public BaseWriter(string @namespace)
        {
            _namespace = @namespace;
        }

        public static void AddSource(string fullPath, string content)
        {
            using var writer = File.CreateText(fullPath);
            writer.Write(content);
        }

        public abstract string GenerateContent(object entity);

        protected static string Tabs(int n)
        {
            return new string('\t', n);
        }
    }
}
