using BenchmarkDotNet.Attributes;

using PhilTemplate;

namespace MyBenchmarks
{
    [MemoryDiagnoser]
    public class Benchmarks
    {
        private static readonly List<TestDataItem> _testDataItems = GetTestTemplates();
        private static readonly List<Func<Func<string, string>, string>> _compiledTemplates = GetCompiledTemplates();
        private static readonly Dictionary<string, string> _replacementData = GetReplacementData();

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

        private static List<Func<Func<string, string>, string>> GetCompiledTemplates()
        {
            return GetTestTemplates()
                .Select(item => TemplateCompiler.CompileFuncStringTemplate(item.Template))
                .ToList();
        }


        private static Dictionary<string, string> GetReplacementData()
        {
            return new Dictionary<string, string>
            {
                { "testKey", "my-test-value" }
            };
        }

        [Benchmark]
        public void TestCompiledTemplates()
        {
            _compiledTemplates.ForEach(template => template(key => _replacementData![key]));
        }

        [Benchmark]
        public void TestOnDemand1()
        {
            _testDataItems.ForEach(templateItem =>
            {
                var actual = TemplateOnDemand.ApplyStringTemplate(templateItem.Template, key => _replacementData![key]);

            });
        }


        [Benchmark]
        public void TestOnDemand2()
        {
            _testDataItems.ForEach(templateItem =>
            {
                var actual = TemplateOnDemand.ApplyStringTemplate(templateItem.Template, key => _replacementData![key]);

            });
        }
    }
}
