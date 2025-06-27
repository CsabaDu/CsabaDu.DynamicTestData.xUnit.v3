// SPDX-License-Identifier: MIT
// Copyright (c) 2025. Csaba Dudas (CsabaDu)

namespace CsabaDu.DynamicTestData.xUnit.v3.TestDataHolders;

public class TheoryTestData<TTestData>(IDataStrategy? dataStrategy)
: TheoryDataBase<TheoryTestDataRow<TTestData>, TTestData>,
INamedDataRowHolder<ITheoryTestDataRow>
where TTestData : notnull, ITestData
{
    public TheoryTestData(
        IEnumerable<ITestDataRow<TheoryTestDataRow<TTestData>, TTestData>> testDataRows,
        IDataStrategy dataStrategy,
        string? testMethodName)
        : this(dataStrategy)
    {
        foreach (var row in testDataRows)
        {
            Add(new TheoryTestDataRow<TTestData>(
                row.TestData,
                dataStrategy.ArgsCode,
                testMethodName));
        }
    }

    public TheoryTestData(
        TheoryTestData<TTestData> other,
        IDataStrategy dataStrategy,
        string? testMethodName)
    : this(dataStrategy)
    {
        foreach (var row in other)
        {
            Add(new TheoryTestDataRow<TTestData>(
                row,
                dataStrategy.ArgsCode,
                testMethodName));
        }
    }

    public TheoryTestData(
        TheoryTestData<TTestData> other,
        IDataStrategy dataStrategy)
    : this(other, dataStrategy, null)
    {
    }


    public TheoryTestData(
        TTestData testData,
        IDataStrategy dataStrategy)
    : this(dataStrategy)
    => Add(testData);

    public Type TestDataType => typeof(TTestData);

    public IDataStrategy DataStrategy { get; init; } = dataStrategy
        ?? throw new ArgumentNullException(nameof(dataStrategy));

    public IDataRowHolder<ITheoryTestDataRow> GetDataRowHolder(
        IDataStrategy dataStrategy)
    => dataStrategy.ArgsCode == DataStrategy.ArgsCode ?
        this
        : new TheoryTestData<TTestData>(
            this,
            dataStrategy);

    public IEnumerable<ITheoryTestDataRow>? GetRows(ArgsCode? argsCode)
    => GetRows(null, argsCode);

    public IEnumerable<ITheoryTestDataRow>? GetRows(string? testMethodName, ArgsCode? argsCode)
    {
        var testDataRows = GetTestDataRows(argsCode);
        var dataStrategy = GetDataStrategy(argsCode);

        return testDataRows?.Select(
            tdr => (tdr as INamedTestDataRow<ITheoryTestDataRow>)
            !.Convert(dataStrategy, testMethodName));
    }

    protected override TheoryTestDataRow<TTestData> Convert(TTestData testData)
    => new(testData, DataStrategy.ArgsCode);

    public IEnumerable<ITestDataRow> GetTestDataRows()
    => this;

    public IEnumerable<ITestDataRow>? GetTestDataRows(ArgsCode? argsCode)
    {
        var dataStrategy = GetDataStrategy(argsCode);

        return GetDataRowHolder(dataStrategy)
            as IEnumerable<TheoryTestDataRow<TTestData>>;
    }

    public IDataStrategy GetDataStrategy(ArgsCode? argsCode)
    {
        argsCode ??= DataStrategy.ArgsCode;

        return argsCode == DataStrategy.ArgsCode ?
                DataStrategy
                : new DataStrategy(
                    argsCode.Value,
                    DataStrategy.WithExpected);
    }
}
