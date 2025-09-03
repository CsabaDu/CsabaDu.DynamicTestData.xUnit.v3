// SPDX-License-Identifier: MIT
// Copyright (c) 2025. Csaba Dudas (CsabaDu)

namespace CsabaDu.DynamicTestData.xUnit.v3.DynamicDataSources;

/// <summary>
/// Abstract base class for providing dynamic theory test data sources with type-safe argument handling.
/// </summary>
/// <remarks>
/// <para>
/// This class serves as a foundation for creating strongly-typed test data sources
/// that can be used with xUnit theory tests. It maintains type consistency across
/// all added test data and provides various methods for adding different kinds of
/// test cases (normal, return value, and exception cases).
/// </para>
/// <para>
/// The class ensures all test data added to a single instance maintains consistent
/// generic type typeParams through runtime checks.
/// </para>
/// </remarks>
/// <param name="argsCode">The strategy for converting test data to method arguments</param>
public abstract class DynamicTheoryTestDataHolder(ArgsCode argsCode)
: DynamicNamedDataRowSource<ITheoryTestDataRow>(argsCode, PropsCode.Expected)
{
    protected override void Add<TTestData>(TTestData testData)
    {
        if (DataHolder is not TheoryTestData<TTestData> theoryTestData)
        {
            InitDataHolder(testData);
            return;
        }

        if (theoryTestData.Any(testData.Equals))
        {
            return;
        }

        theoryTestData.Add(testData);
    }

    public TheoryTestData<TTestData>? GetTheoryTestData<TTestData>(
        string? testMethodName,
        ArgsCode? argsCode)
    where TTestData : notnull, ITestData
    {
        if (DataHolder is null)
        {
            return null;
        }

        var dataStrategy = GetDataStrategy(argsCode);

        if (DataHolder is TheoryTestData<TTestData> theoryTestData)
        {
            return dataStrategy.ArgsCode == ArgsCode
                && testMethodName is null ?
                theoryTestData
                : new TheoryTestData<TTestData>(
                    theoryTestData,
                    dataStrategy,
                    testMethodName);
        }

        if (DataHolder is IEnumerable<ITestDataRow<TheoryTestDataRow<TTestData>, TTestData>> theoryTestDataRows)
        {
            return new TheoryTestData<TTestData>(
                theoryTestDataRows,
                dataStrategy,
                testMethodName);
        }

        if (DataHolder is IEnumerable<ITestDataRow> testDataRows
            && testDataRows.All(x => x.GetTestData() is TTestData))
        {
            return new TheoryTestData<TTestData>(
                testDataRows,
                dataStrategy,
                testMethodName);
        }

        return null;
    }

    protected override void InitDataHolder<TTestData>(TTestData testData)
    => DataHolder = new TheoryTestData<TTestData>(
        testData,
        this,
        null);
}
