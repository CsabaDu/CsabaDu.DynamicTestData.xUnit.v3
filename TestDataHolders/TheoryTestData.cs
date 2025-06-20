// SPDX-License-Identifier: MIT
// Copyright (c) 2025. Csaba Dudas (CsabaDu)

namespace CsabaDu.DynamicTestData.xUnit.v3.TestDataHolders;

public class TheoryTestData<TTestData>
: TheoryDataBase<ITheoryTestDataRow<TTestData>, TTestData>,
ITheoryTestData
where TTestData : notnull, ITestData
{
    private TheoryTestData(
        IDataStrategy? dataStrategy)
    => DataStrategy = dataStrategy
        ?? throw new ArgumentNullException(nameof(dataStrategy));

    public TheoryTestData(
        TheoryTestData<TTestData> other,
        IDataStrategy dataStrategy)
    : this(dataStrategy)
    {
        foreach (var row in other)
        {
            Add(new TheoryTestDataRow<TTestData>(
                row.TestData,
                dataStrategy.ArgsCode));
        }
    }

    public TheoryTestData(
        TTestData testData,
        IDataStrategy dataStrategy)
    : this(dataStrategy)
    => Add(testData);

    public TheoryTestData(ITheoryTestDataRow<TTestData> row)
    : this(row?.GetDataStrategy())
    => Add(row!);

    public TheoryTestData(IEnumerable<TheoryTestDataRow<TTestData>> rows)
    : this(rows?.FirstOrDefault()?.GetDataStrategy())
    => AddRange(rows!);

    public Type TestDataType => typeof(TTestData);

    public IDataStrategy DataStrategy { get; init; }

    public IEnumerable<ITheoryTestDataRow>? GetNamedRows(string? testMethodName)
    //=> GetRows(testMethodName, null);
    => (GetDataRowHolder(DataStrategy) as IEnumerable<ITestDataRow>)
    ?.Select(tdr => (tdr as INamedTestDataRow<ITheoryTestDataRow>)
    !.Convert(DataStrategy, testMethodName));

    public IEnumerable<ITheoryTestDataRow>? GetNamedRows(string? testMethodName, ArgsCode? argsCode)
    {
        if (argsCode.HasValue)
        {
            var dataStrategy =
                GetDataStrategy(argsCode.Value);
            var dataRowHolder =
                (GetDataRowHolder(dataStrategy)
                    as INamedDataRowHolder<ITheoryTestDataRow>)!;

            return dataRowHolder.GetNamedRows(testMethodName);
        }

        return GetNamedRows(testMethodName);
    }

    private IDataStrategy GetDataStrategy(ArgsCode argsCode)
        => argsCode == DataStrategy.ArgsCode ?
            DataStrategy
            : new DataStrategy(
                argsCode,
                DataStrategy.WithExpected);

    public IEnumerable<ITheoryTestDataRow>? GetRows()
    => (GetDataRowHolder(DataStrategy) as IEnumerable<ITestDataRow>)
        ?.Select(tdr => (tdr as ITestDataRow<ITheoryTestDataRow>)
        !.Convert(DataStrategy));

    public IEnumerable<ITheoryTestDataRow>? GetRows(ArgsCode? argsCode)
    {
        if (argsCode.HasValue)
        {
            var dataStrategy =
                GetDataStrategy(argsCode.Value);
            var dataRowHolder =
                GetDataRowHolder(dataStrategy);

            return dataRowHolder.GetRows();
        }

        return GetRows();
    }

    protected override ITheoryTestDataRow<TTestData> Convert(TTestData testData)
    => new TheoryTestDataRow<TTestData>(
        testData,
        DataStrategy.ArgsCode);

    public IDataRowHolder<ITheoryTestDataRow> GetDataRowHolder(
        IDataStrategy dataStrategy)
    => new TheoryTestData<TTestData>(
        this,
        dataStrategy);
}
