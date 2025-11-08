// SPDX-License-Identifier: MIT
// Copyright (c) 2025. Csaba Dudas (CsabaDu)

namespace CsabaDu.DynamicTestData.xUnit.v3.TestDataRows;

/// <summary>
/// Represents a row of test data for xUnit.net theory tests with additional configuration options.
/// </summary>
/// <param name="testData">The test data instance</param>
/// <param name="argsCode">Specifies how the test data should be converted to arguments</param>
public sealed class TheoryTestDataRow<TTestData>(
    TTestData testData,
    ArgsCode argsCode)
: NamedTestDataRow<ITheoryTestDataRow, TTestData>(testData),
ITheoryTestDataRow
where TTestData : notnull, ITestData
{
    #region Constructors
    internal TheoryTestDataRow(
        TTestData testData,
        ArgsCode argsCode,
        string? testMethodName)
    : this(
        testData,
        argsCode)
    => TestDisplayName = GetTestDisplayName(
        testMethodName,
        testData);

    private TheoryTestDataRow(
        TheoryTestDataRow<TTestData> other,
        string? testMethodName)
    : this(
        other.TestData,
        other.ArgsCode)
    => SetTheoryDataRow(
        other,
        other.ArgsCode,
        testMethodName);

    internal TheoryTestDataRow(
        TheoryTestDataRow<TTestData> other,
        ArgsCode argsCode,
        string? testMethodName)
    : this(
        other.TestData,
        argsCode)
    => SetTheoryDataRow(
        other,
        argsCode,
        testMethodName);
    #endregion

    #region Properties
    public ArgsCode ArgsCode { get; private set; } =
        argsCode.Defined(nameof(argsCode));
    #endregion

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

    /// <inheritdoc/>
    public string? Label { get; set; }

    /// <inheritdoc/>
    public Type? SkipType { get; set; }

    /// <inheritdoc/>
    public string? SkipUnless { get; set; }

    /// <inheritdoc/>
    public string? SkipWhen { get; set; }

    public object?[] GetData()
    => GetParams(GetDataStrategy());
    #endregion

    #region TestDataRow members
    public override ITheoryTestDataRow Convert(IDataStrategy dataStrategy)
    {
        ArgsCode argsCode = dataStrategy.ArgsCode;

        return argsCode == ArgsCode ?
            this
            : new TheoryTestDataRow<TTestData>(
                this,
                argsCode,
                null);
    }
    #endregion

    #region ITheoryTestDataRow members
    public override ITheoryTestDataRow Convert(IDataStrategy dataStrategy, string? testMethodName)
    => string.IsNullOrEmpty(testMethodName) ?
        this
        : new TheoryTestDataRow<TTestData>(
            this,
            testMethodName);

    public IDataStrategy GetDataStrategy()
    => GetStoredDataStrategy(ArgsCode, default);
    #endregion

    #region Private methods
    private void SetTheoryDataRow(
        TheoryTestDataRow<TTestData> other,
        ArgsCode argsCode,
        string? testMethodName)
    {
        ArgsCode = argsCode;

        Explicit = other.Explicit;
        Skip = other.Skip;
        Label = other.Label;
        SkipType = other.SkipType;
        SkipUnless = other.SkipUnless;
        SkipWhen = other.SkipWhen;
        TestDisplayName = GetTestDisplayName(
            testMethodName,
            other)
            ?? other.TestDisplayName;
        Timeout = other.Timeout;
        Traits = other.Traits ?? [];
    }

    private string? GetTestDisplayName(
        string? testMethodName,
        INamedTestCase namedTestCase)
    => ArgsCode == ArgsCode.Properties ?
        GetDisplayName(
            testMethodName,
            namedTestCase.GetTestCaseName())
        : testMethodName;
    #endregion
}
