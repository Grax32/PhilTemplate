using PhilTemplate;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Collections.Generic;

namespace PhilTemplateUnitTests
{
    [TestClass]
    public class TemplateTest
    {
        private class TestDataItem
        {
            public string? Template { get; set; }
            public string? Expected { get; set; }
        }

        private static List<TestDataItem> GetTestTemplates()
        {
            return new TestDataItem[]
            {
                new (){ Template = "", Expected = "" },
                new (){ Template = "z{{}}}}z", Expected = "z{{}}z" },
                new (){ Template = "z{{{{}}}}z", Expected = "z{{}}z" },
                new (){ Template = "{{testKey}}", Expected = "my-test-value" },
                new (){ Template = "zz{{testKey}}", Expected = "zzmy-test-value" },
                new (){ Template = "{{testKey}}ee", Expected = "my-test-valueee" },
                new (){ Template = "aa{{testKey}}bb", Expected = "aamy-test-valuebb" },
                new (){ Template = "{{testKey}}{{testKey}}{{testKey}}{{testKey}}{{testKey}}88{{testKey}}{{testKey}}99", Expected = "my-test-valuemy-test-valuemy-test-valuemy-test-valuemy-test-value88my-test-valuemy-test-value99" },
            }.ToList();
        }

        private static Dictionary<string, string> GetReplacementData()
        {
            return new Dictionary<string, string>
            {
                { "testKey", "my-test-value" }
            };
        }

        private static void ThrowIfNoMatch(string? expected, string? actual)
        {
            if (actual != expected)
            {
                throw new InvalidOperationException(@"Output did not match 
expected: " + expected +
@" for 
  actual: " + actual);
            }

        }


        [TestMethod]
        public void TestCompiledTemplates()
        {
            var data = GetReplacementData();
            var templatesAndResults = GetTestTemplates();

            templatesAndResults
                .ForEach(templateItem =>
                {
                    var actual = TemplateCompiler.CompileFuncStringTemplate(templateItem.Template)(key => data![key]);
                    ThrowIfNoMatch(templateItem.Expected, actual);
                });
        }

        [TestMethod]
        public void TestOnDemandTemplates()
        {
            var data = GetReplacementData();
            var templatesAndResults = GetTestTemplates();

            templatesAndResults
                 .ForEach(templateItem =>
                 {
                     var actual = TemplateOnDemand.ApplyStringTemplate(templateItem.Template, key => data![key]);
                     ThrowIfNoMatch(templateItem.Expected, actual);
                 });
        }

#if DEBUG
        [TestMethod]
        public void TestOnDemandTemplates2()
        {
            var data = GetReplacementData();
            var templatesAndResults = GetTestTemplates();

            Enumerable.Range(0, 10000)
                .Select(templateItem => new { templatesAndResults })
                .SelectMany(item => item.templatesAndResults)
                .ToList()
                 .ForEach(templateItem =>
                 {
                     var actual = TemplateOnDemand2.ApplyStringTemplate(templateItem.Template, key => data![key]);
                     ThrowIfNoMatch(templateItem.Expected, actual);
                 });
        }
#endif

        [TestMethod]
        public void TestOnDemandTemplates1()
        {
            var data = GetReplacementData();
            var templatesAndResults = GetTestTemplates();

            Enumerable.Range(0, 10000)
                .Select(templateItem => new { templatesAndResults })
                .SelectMany(item => item.templatesAndResults)
                .ToList()
                 .ForEach(templateItem =>
                 {
                     var actual = TemplateOnDemand.ApplyStringTemplate(templateItem.Template, key => data![key]);
                     ThrowIfNoMatch(templateItem.Expected, actual);
                 });
        }
    }
}