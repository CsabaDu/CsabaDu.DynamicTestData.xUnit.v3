// SPDX-License-Identifier: MIT
// Copyright (c) 2025. Csaba Dudas (CsabaDu)

namespace CsabaDu.DynamicTestData.xUnit.v3.TestDataHolders;

public class TheoryTestData<TTestData>(IDataStrategy? dataStrategy)
: TheoryDataBase<TheoryTestDataRow<TTestData>, TTestData>,
INamedDataRowHolder<ITheoryTestDataRow>
where TTestData : notnull, ITestData
{
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

    public Type TestDataType => typeof(TTestData);

    public IDataStrategy DataStrategy { get; init; } = dataStrategy
        ?? throw new ArgumentNullException(nameof(dataStrategy));

    public IDataRowHolder<ITheoryTestDataRow> GetDataRowHolder(
        IDataStrategy dataStrategy)
    => new TheoryTestData<TTestData>(
        this,
        dataStrategy);

    public IEnumerable<ITheoryTestDataRow>? GetRows()
    => (GetDataRowHolder(DataStrategy) as IEnumerable<ITestDataRow>)
        ?.Select(tdr => (tdr as ITestDataRow<ITheoryTestDataRow>)
        !.Convert(DataStrategy));

    public IEnumerable<ITheoryTestDataRow>? GetRows(ArgsCode? argsCode)
    => argsCode.HasValue ?
        GetDataRowHolder(argsCode.Value)
        .GetRows()
        : GetRows();

    public IEnumerable<ITheoryTestDataRow>? GetNamedRows(string? testMethodName)
    => (GetDataRowHolder(DataStrategy) as IEnumerable<ITestDataRow>)
        ?.Select(tdr => (tdr as INamedTestDataRow<ITheoryTestDataRow>)
        !.Convert(DataStrategy, testMethodName));

    public IEnumerable<ITheoryTestDataRow>? GetNamedRows(string? testMethodName, ArgsCode? argsCode)
    => argsCode.HasValue ?
        (GetDataRowHolder(argsCode.Value)
            as INamedDataRowHolder<ITheoryTestDataRow>)
        !.GetNamedRows(testMethodName)
            : GetNamedRows(testMethodName);

    protected override TheoryTestDataRow<TTestData> Convert(TTestData testData)
    => new TheoryTestDataRow<TTestData>(
        testData,
        DataStrategy.ArgsCode);

    private IDataStrategy GetDataStrategy(ArgsCode argsCode)
    => argsCode == DataStrategy.ArgsCode ?
        DataStrategy
        : new DataStrategy(
            argsCode,
            DataStrategy.WithExpected);

    private IDataRowHolder<ITheoryTestDataRow> GetDataRowHolder(ArgsCode argsCode)
    => GetDataRowHolder(GetDataStrategy(argsCode));
}
