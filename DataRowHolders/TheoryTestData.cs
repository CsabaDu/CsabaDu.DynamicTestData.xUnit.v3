// SPDX-License-Identifier: MIT
// Copyright (c) 2025. Csaba Dudas (CsabaDu)

using CsabaDu.DynamicTestData.xUnit.v3.TestDataRows;
using CsabaDu.DynamicTestData.xUnit.v3.TestDataRows.Converters;
using CsabaDu.DynamicTestData.xUnit.v3.TestDataRows.Interfaces;
using static CsabaDu.DynamicTestData.xUnit.v3.TestDataRows.Converters.TestDataConverter;

namespace CsabaDu.DynamicTestData.xUnit.v3.DataRowHolders;

public class TheoryTestData<TTestData>
: TheoryDataBase<TheoryTestDataRow<TTestData>, TTestData>,
INamedDataRowHolder<ITheoryTestDataRow>
where TTestData : notnull, ITestData
{
    #region Constructors
    private TheoryTestData(IDataStrategy? dataStrategy)
    {
        DataStrategy = Guard.ArgumentNotNull(
            dataStrategy,
            nameof(dataStrategy));
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
        PropsCode? propsCode)
    => GetRows(
        null,
        argsCode,
        propsCode);

    public IEnumerable<ITheoryTestDataRow>? GetRows(
        string? testMethodName,
        ArgsCode? argsCode,
        PropsCode? propsCode)
    => NamedDataRowHolder<ITheoryTestDataRow, TTestData>.GetRows(
        this,
        testMethodName,
        GetDataStrategy(
            argsCode,
            propsCode));

    public override void Add(TheoryTestDataRow<TTestData> row)
    {
        if (row?.ContainedBy(this) != true)
        {
            base.Add(row!);
        }
    }

    public new void Add(TTestData testData)
    {
        if (!testData.ContainedBy(this))
        {
            base.Add(testData);
        }
    }

    public new void AddRange(IEnumerable<TTestData> rows)
    {
        foreach (var row in Guard.ArgumentNotNull(rows))
        {
            Add(row);
        }
    }

    public new void AddRange(params TTestData[] rows)
    {
        foreach (var row in Guard.ArgumentNotNull(rows))
        {
            Add(row);
        }
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
    => GetStoredDataStrategy(
        argsCode,
        DataStrategy);

    public IDataStrategy GetDataStrategy(
        ArgsCode? argsCode,
        PropsCode? propsCode)
    => GetStoredDataStrategy(
        argsCode ?? DataStrategy.ArgsCode,
        propsCode ?? DataStrategy.PropsCode);
    #endregion
}
