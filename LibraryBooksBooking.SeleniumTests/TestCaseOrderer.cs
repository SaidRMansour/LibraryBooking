using System;
using System.Collections.Generic;
using Xunit.Abstractions;
using Xunit.Sdk;

public class TestCaseOrderer : ITestCaseOrderer
{
    public IEnumerable<TTestCase> OrderTestCases<TTestCase>(IEnumerable<TTestCase> testCases)
        where TTestCase : ITestCase
    {
        var sortedMethods = new SortedDictionary<int, TTestCase>();

        foreach (var testCase in testCases)
        {
            var orderAttribute = testCase.TestMethod.Method.GetCustomAttributes((typeof(TestOrderAttribute).AssemblyQualifiedName)).FirstOrDefault();
            var order = orderAttribute.GetNamedArgument<int>("Order");
            sortedMethods.Add(order, testCase);
        }

        return sortedMethods.Values;
    }
}

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class TestOrderAttribute : Attribute
{
    public int Order { get; private set; }

    public TestOrderAttribute(int order)
    {
        Order = order;
    }
}
