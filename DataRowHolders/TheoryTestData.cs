// SPDX-License-Identifier: MIT
// Copyright (c) 2025. Csaba Dudas (CsabaDu)

using CsabaDu.DynamicTestData.xUnit.v3.Statics;

namespace CsabaDu.DynamicTestData.xUnit.v3.DataRowHolders;

public class TheoryTestData<TTestData>
: TheoryDataBase<TheoryTestDataRow<TTestData>, TTestData>,
INamedDataRowHolder<ITheoryTestDataRow>
where TTestData : notnull, ITestData
{
    #region Constructors
    protected TheoryTestData(IDataStrategy? dataStrategy)
    {
        DataStrategy = dataStrategy
            ?? throw new ArgumentNullException(nameof(dataStrategy));
    }

    internal TheoryTestData(
        IEnumerable<ITestDataRow> testDataRows,
        IDataStrategy dataStrategy,
        string? testMethodName)
    : this(dataStrategy)
    {
        Guard.ArgumentNotNull(
            testDataRows,
            nameof(testDataRows));

        if (testDataRows.Any(row => row.GetTestData() is not TTestData))
        {
            throw new ArgumentException(
                "'GetTestData()' method of the provided test data rows" +
                $" must return objects of type {typeof(TTestData).Name}.",
                nameof(testDataRows));
        }

        if (testMethodName is null)
        {
            AddRange(testDataRows.Select(getTestData));
        }

        else
        {
            AddRange(
                testDataRows.Select(
                    row => getTestData(row).ToTheoryTestDataRow(
                        dataStrategy.ArgsCode,
                        testMethodName)));
        }

        #region Local methods
        static TTestData getTestData(ITestDataRow row)
        => (TTestData)row.GetTestData();
        #endregion
    }

    public TheoryTestData(
        IEnumerable<ITestDataRow<TheoryTestDataRow<TTestData>, TTestData>> testDataRows,
        IDataStrategy dataStrategy,
        string? testMethodName)
    : this(dataStrategy)
    {
        AddRange(testDataRows.Select(
            row => row.TestData.ToTheoryTestDataRow(
                dataStrategy.ArgsCode,
                testMethodName)));
    }

    public TheoryTestData(
        TheoryTestData<TTestData> other,
        IDataStrategy dataStrategy,
        string? testMethodName)
    : this(dataStrategy)
    {
        AddRange(other.Select(
            row => new TheoryTestDataRow<TTestData>(
                row,
                dataStrategy.ArgsCode,
                testMethodName)));
    }

    public TheoryTestData(
        TTestData testData,
        IDataStrategy dataStrategy,
        string? testMethodName)
    : this(dataStrategy)
    {
        if (testMethodName is null)
        {
            Add(testData);
        }
        else
        {
            Add(testData.ToTheoryTestDataRow(
                dataStrategy.ArgsCode,
                testMethodName));
        }
    }
    #endregion

    public Type TestDataType
        => typeof(TTestData);

    public IDataStrategy DataStrategy { get; init; }

    public IDataRowHolder<ITheoryTestDataRow> GetDataRowHolder(IDataStrategy dataStrategy)
    => dataStrategy.ArgsCode == DataStrategy.ArgsCode ?
        this
        : new TheoryTestData<TTestData>(
            this,
            dataStrategy,
            null);

    public IEnumerable<ITheoryTestDataRow>? GetRows(ArgsCode? argsCode)
    => GetRows(null, argsCode);

    public IEnumerable<ITheoryTestDataRow>? GetRows(string? testMethodName, ArgsCode? argsCode)
    {
        var testDataRows = GetTestDataRows();
        var dataStrategy = GetDataStrategy(argsCode);

        return testDataRows?.Select(
            tdr => (tdr as INamedTestDataRow<ITheoryTestDataRow>)
            !.Convert(dataStrategy, testMethodName));
    }

    protected override TheoryTestDataRow<TTestData> Convert(TTestData testData)
    => new(testData, DataStrategy.ArgsCode);

    public IEnumerable<ITestDataRow>? GetTestDataRows()
    {
        var dataStrategy = GetDataStrategy(ArgsCode.Instance);

        return GetDataRowHolder(dataStrategy)
            as IEnumerable<TheoryTestDataRow<TTestData>>;
    }

    public IDataStrategy GetDataStrategy(ArgsCode? argsCode)
    {
        argsCode ??= DataStrategy.ArgsCode;

        return argsCode == DataStrategy.ArgsCode ?
            DataStrategy
            : GetStoredDataStrategy(
                argsCode.Value,
                DataStrategy.WithExpected);
    }
}
