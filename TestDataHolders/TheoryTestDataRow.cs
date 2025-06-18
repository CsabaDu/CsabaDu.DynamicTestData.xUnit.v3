// SPDX-License-Identifier: MIT
// Copyright (c) 2025. Csaba Dudas (CsabaDu)

namespace CsabaDu.DynamicTestData.xUnit.v3.TheoryTestDataHolders;

/// <summary>
/// Represents a row of test data for xUnit.net theory tests with additional configuration options.
/// </summary>
/// <param name="TestData">The test data instance</param>
/// <param name="ArgsCode">Specifies how the test data should be converted to arguments</param>
public sealed class TheoryTestDataRow<TTestData>(
    TTestData testData,
    IDataStrategy dataStrategy)
: TestDataRow<TTestData, ITheoryTestDataRow>(
    testData,
    dataStrategy),
ITheoryTestDataRow<TTestData>
where TTestData : notnull, ITestData
{
    private TheoryTestDataRow(
        TheoryTestDataRow<TTestData> other,
        string? testMethodName)
    : this(
        other.TestData,
        other.DataStrategy)
    => SetTheoryDataRow(
        other,
        other.DataStrategy.ArgsCode,
        testMethodName);

    internal TheoryTestDataRow(
        TheoryTestDataRow<TTestData> other,
        ArgsCode argsCode,
        string? testMethodName)
    : this(
        other.TestData,
        new DataStrategy(
            argsCode,
            other.DataStrategy.WithExpected))
    => SetTheoryDataRow(
        other,
        argsCode,
        testMethodName);

    internal void SetTheoryDataRow(
        TheoryTestDataRow<TTestData> other,
        ArgsCode argsCode,
        string? testMethodName)
    {
        Explicit = other.Explicit;
        Skip = other.Skip;
        TestDisplayName =
            (argsCode == ArgsCode.Properties ?
                GetDisplayName(testMethodName, other.TestCaseName)
                : testMethodName)
            ?? other.TestDisplayName;
        Timeout = other.Timeout;
        Traits = other.Traits ?? [];
    }

    #region ITheoryDataRow members
    Dictionary<string, HashSet<string>> traits = [];

    /// <inheritdoc/>
    public bool? Explicit { get; set; }

    /// <inheritdoc/>
    public string? Skip { get; set; }

    /// <inheritdoc/>
    public string? TestDisplayName { get; set; }

    /// <inheritdoc/>
    public int? Timeout { get; set; }

    /// <inheritdoc/>
    public Dictionary<string, HashSet<string>> Traits
    {
        get => traits;
        set => traits = Guard.ArgumentNotNull(value, nameof(Traits));
    }

    public object?[] GetData()
    => Params;
    #endregion

    public override ITheoryTestDataRow Convert()
    => this;

    public override ITestDataRow<TTestData, ITheoryTestDataRow> CreateTestDataRow(
        TTestData testData,
        IDataStrategy dataStrategy)
    => new TheoryTestDataRow<TTestData>(
        testData,
        dataStrategy);

    public ITheoryTestDataRow Convert(string? testMethodName)
    => (string.IsNullOrEmpty(testMethodName) ?
        this
        : new TheoryTestDataRow<TTestData>(
            this,
            testMethodName));
}
