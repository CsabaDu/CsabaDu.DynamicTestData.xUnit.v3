// SPDX-License-Identifier: MIT
// Copyright (c) 2025. Csaba Dudas (CsabaDu)

namespace CsabaDu.DynamicTestData.xUnit.v3.TheoryTestDataTypes;

/// <summary>
/// Represents a row of test data for xUnit.net theory tests with additional configuration options.
/// </summary>
/// <param name="TestData">The test data instance</param>
/// <param name="ArgsCode">Specifies how the test data should be converted to arguments</param>
public sealed class TheoryTestDataRow<TTestData>(
    TTestData testData,
    IDataStrategy? dataStrategy)
: TestDataRow<TTestData, object?[]>(
    testData,
    dataStrategy),
ITheoryTestDataRow
where TTestData : notnull, ITestData
{
    private TheoryTestDataRow(
        TheoryTestDataRow<TTestData> other,
        string? testMethodName)
    : this(
        other.TestData,
        other.DataStrategy)
    {
        Explicit = other.Explicit;
        Skip = other.Skip;
        TestDisplayName =
            other.DataStrategy.ArgsCode == ArgsCode.Properties ?
            GetDisplayName(testMethodName, other.TestCaseName)
            : testMethodName
            ?? other.TestDisplayName;
        Timeout = other.Timeout;
        Traits = other.Traits ?? [];
    }

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

    public override object?[] Convert()
    => Params;

    public override ITestDataRow<TTestData, object?[]> CreateTestDataRow(
        TTestData testData,
        IDataStrategy? dataStrategy)
    => new TheoryTestDataRow<TTestData>(
        testData,
        dataStrategy);

    public object?[] GetData()
    => Params;

    public ITheoryTestDataRow SetName(string? testMethodName)
    => string.IsNullOrEmpty(testMethodName) ?
        this
        : new TheoryTestDataRow<TTestData>(this, testMethodName);
}
