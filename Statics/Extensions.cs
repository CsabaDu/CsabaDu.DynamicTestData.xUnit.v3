// SPDX-License-Identifier: MIT
// Copyright (c) 2025. Csaba Dudas (CsabaDu)

namespace CsabaDu.DynamicTestData.xUnit.v3.Statics;

public static class Extensions
{
    public static ITheoryDataRow ToTheoryDataRow<TTestData>(
        this TTestData testData,
        ArgsCode argsCode,
        string? testMethodName)
    where TTestData : notnull, ITestData
    {
        string? testDisplayName = GetDisplayName(
            testMethodName,
            testData.GetTestCaseName());

        return argsCode switch
        {
            ArgsCode.Instance =>
            new TheoryDataRow<TTestData>(testData)
            {
                TestDisplayName = testDisplayName,
            },
            ArgsCode.Properties =>
            new TheoryDataRow(testData.ToParams(
                argsCode,
                testData is IExpected))
            {
                TestDisplayName = testDisplayName,
            },
            _ =>
            throw argsCode.GetInvalidEnumArgumentException(
                nameof(argsCode)),
        };
    }

    public static TheoryTestDataRow<TTestData> ToTheoryTestDataRow<TTestData>(
        this TTestData testData,
        ArgsCode argsCode,
        string? testMethodName)
    where TTestData : notnull, ITestData
    => new TheoryTestDataRow<TTestData>(
        testData,
        argsCode,
        testMethodName);
}
