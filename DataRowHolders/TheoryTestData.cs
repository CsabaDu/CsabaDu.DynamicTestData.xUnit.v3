// SPDX-License-Identifier: MIT
// Copyright (c) 2025. Csaba Dudas (CsabaDu)

using CsabaDu.DynamicTestData.DataRowHolders;

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
            AddRange(testDataRows.Select(getTheoryTestDataRow));
        }

        #region Local methods
        static TTestData getTestData(ITestDataRow row)
        => (TTestData)row.GetTestData();

        TheoryTestDataRow<TTestData> getTheoryTestDataRow(ITestDataRow row)
        => getTestData(row).ToTheoryTestDataRow(
            dataStrategy.ArgsCode,
            testMethodName);
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

    #region Properties
    public IDataStrategy DataStrategy { get; init; }
    #endregion

    #region Methods
    public IDataRowHolder<ITheoryTestDataRow> GetDataRowHolder(IDataStrategy dataStrategy)
    => dataStrategy.ArgsCode == DataStrategy.ArgsCode ?
        this
        : new TheoryTestData<TTestData>(
            this,
            dataStrategy,
            null);

    public IEnumerable<ITheoryTestDataRow>? GetRows(ArgsCode? argsCode)
    => GetRows(
        null,
        argsCode);

    public IEnumerable<ITheoryTestDataRow>? GetRows(
        string? testMethodName,
        ArgsCode? argsCode)
    => NamedDataRowHolder<ITheoryTestDataRow, TTestData>.GetRows(
        this,
        testMethodName,
        GetDataStrategy(argsCode));

    public IEnumerable<ITheoryTestDataRow>? GetRows(
        ArgsCode? argsCode,
        PropertyCode? propertyCode)
    => GetRows(
        null,
        argsCode,
        propertyCode);

    public IEnumerable<ITheoryTestDataRow>? GetRows(
        string? testMethodName,
        ArgsCode? argsCode,
        PropertyCode? propertyCode)
    => NamedDataRowHolder<ITheoryTestDataRow, TTestData>.GetRows(
        this,
        testMethodName,
        GetDataStrategy(
            argsCode,
            propertyCode));

    protected override TheoryTestDataRow<TTestData> Convert(TTestData testData)
    => new(testData, DataStrategy.ArgsCode);

    public IEnumerable<ITestDataRow>? GetTestDataRows()
    {
        var dataStrategy = GetDataStrategy(ArgsCode.Instance);

        return GetDataRowHolder(dataStrategy)
            as IEnumerable<TheoryTestDataRow<TTestData>>;
    }

    public IDataStrategy GetDataStrategy(ArgsCode? argsCode)
    => GetStoredDataStrategy(
        argsCode,
        DataStrategy);

    public IDataStrategy GetDataStrategy(
        ArgsCode? argsCode,
        PropertyCode? propertyCode)
    => GetStoredDataStrategy(
        argsCode ?? DataStrategy.ArgsCode,
        propertyCode ?? DataStrategy.PropertyCode);
    #endregion
}
