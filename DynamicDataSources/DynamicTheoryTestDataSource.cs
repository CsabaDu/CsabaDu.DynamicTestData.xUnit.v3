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
/// generic type parameters through runtime checks.
/// </para>
/// </remarks>
/// <param name="argsCode">The strategy for converting test data to method arguments</param>
public abstract class DynamicTheoryTestDataHolder(ArgsCode argsCode)
: DynamicNamedDataSource<ITheoryTestDataRow>(argsCode, typeof(IExpected))
{
    protected override void Add<TTestData>(TTestData testData)
    {
        bool rowCreated = TryGetTestDataRow<TTestData, TheoryTestDataRow<TTestData>>(
            testData,
            out ITestDataRow<TTestData, ITheoryTestDataRow>? testDataRow);

        if (rowCreated && DataRowHolder is TheoryTestData<TTestData> theoryTestData)
        {
            theoryTestData.Add(testDataRow!.TestData);
        }
    }

    public TheoryTestData<TTestData>? GetTheoryTestData<TTestData>(
        string? testMethodName,
        ArgsCode? argsCode)
    where TTestData : notnull, ITestData
    {
        if (DataRowHolder is null)
        {
            return null;
        }

        var dataStrategy = GetDataStrategy(argsCode);

        if (DataRowHolder is TheoryTestData<TTestData> theoryTestData)
        {
            return dataStrategy.ArgsCode == DataRowHolder?.DataStrategy.ArgsCode
                && testMethodName == null ?
                theoryTestData
                : new TheoryTestData<TTestData>(
                    theoryTestData,
                    dataStrategy,
                    testMethodName);
        }

        if (DataRowHolder is IEnumerable<ITestDataRow<TTestData, TheoryTestDataRow<TTestData>>> testDataRows)
        {
            return new TheoryTestData<TTestData>(
                testDataRows,
                dataStrategy,
                testMethodName);
        }

        return null;
    }

    protected override ITestDataRow<TTestData, ITheoryTestDataRow> CreateTestDataRow<TTestData>(TTestData testData)
    => new TheoryTestDataRow<TTestData>(
        testData,
        ArgsCode);

    protected override void InitDataRowHolder<TTestData>(TTestData testData)
    => DataRowHolder = new TheoryTestData<TTestData>
        (testData,
        this);
}
