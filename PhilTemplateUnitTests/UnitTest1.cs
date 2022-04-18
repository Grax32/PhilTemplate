using PhilTemplate;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace PhilTemplateUnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var data = Enumerable.Range(1, 10).Select(key => new { key = $"key:{key}", value = $"{key}:value" }).ToDictionary(item => item.key, item => item.value);

            var templatesAndResults = new[]
            {
                new { template = "", expected = "" },
                new { template = "z{{}}}}z", expected = "z{{}}z" },
                new { template = "z{{{{}}}}z", expected = "z{{}}z" },
                new { template = "{{key:4}}", expected = "4:value" },
                new { template = "zz{{key:4}}", expected = "zz4:value" },
                new { template = "{{key:4}}ee", expected = "4:valueee" },
                new { template = "aa{{key:4}}bb", expected = "aa4:valuebb" },
                new { template = "{{key:4}}{{key:4}}{{key:4}}{{key:4}}{{key:4}}88{{key:4}}{{key:4}}99", expected = "4:value4:value4:value4:value4:value884:value4:value99" },
            }.ToList();

            void ThrowIfNoMatch(string expected, string actual)
            {
                if (actual != expected)
                {
                    throw new InvalidOperationException(@"Output did not match 
expected: " + expected +
@" for 
  actual: " + actual);
                }

            }

            void RequireCompiledOutputEqualExpected(string template, string expected)
            {
                var actual = TemplateCompiler.CompileFuncStringTemplate(template)(key => data![key]);
                ThrowIfNoMatch(expected, actual);
            }

            templatesAndResults.ForEach(x => RequireCompiledOutputEqualExpected(x.template, x.expected));



            void RequireOnDemandOutputEqualExpected(string template, string expected)
            {
                var actual = TemplateOnDemand.ApplyStringTemplate(template, key => data![key]);
                ThrowIfNoMatch(expected, actual);
            }

            templatesAndResults.ForEach(x => RequireOnDemandOutputEqualExpected(x.template, x.expected));
        }
    }
}