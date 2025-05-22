// SPDX-License-Identifier: MIT
// Copyright (c) 2025. Csaba Dudas (CsabaDu)

namespace CsabaDu.DynamicTestData.xUnit.v3.TheoryTestDataTypes;

/// <summary>
/// Represents a row of test data for xUnit.net theory tests with additional configuration options.
/// </summary>
/// <param name="TestData">The test data instance</param>
/// <param name="ArgsCode">Specifies how the test data should be converted to arguments</param>
public sealed class TheoryTestDataRow
: TheoryDataRowBase, ITheoryTestDataRow
{
    #region Constructors
    public TheoryTestDataRow(
        ITestData testData,
        ArgsCode argsCode)
    {
        Data = TestDataToParams(
            testData,
            argsCode,
            testData is IExpected,
            out string testCase);
        ArgsCode = argsCode.Defined(nameof(argsCode));
        TestCase = testCase;
    }

    private TheoryTestDataRow(
        ITheoryTestDataRow other,
        string? testMethodName)
    {
        Guard.ArgumentNotNull(other, nameof(other));

        Data = other.Data;
        ArgsCode = other.ArgsCode;
        TestCase = other.TestCase;
        Explicit = other.Explicit;
        Skip = other.Skip;
        TestDisplayName =
            other.ArgsCode == ArgsCode.Properties ?
            GetDisplayName(testMethodName, other.TestCase)
            : testMethodName
            ?? other.TestDisplayName;
        Timeout = other.Timeout;
        Traits = other.Traits ?? [];
    }
    #endregion

    public ArgsCode ArgsCode { get; init; }

    #region Properties
    /// <inheritdoc cref="ITheoryTestDataRow.Data"/>/>
    public object?[] Data { get; init;}

    /// <inheritdoc cref="ITheoryTestDataRow.TestCase"/>/>
    public string TestCase { get; init; }

    #endregion

    #region Methods
    /// <summary>
    /// Sets the test display name based on the test method name.
    /// </summary>
    /// <param name="testMethodName">The name of the test method</param>
    /// <returns>A new instance with the updated display name
    /// or the same instance if <paramref name="testMethodName"/> is null.</returns>
    public ITheoryTestDataRow SetName(string? testMethodName)
    {
        return !string.IsNullOrEmpty(testMethodName) ?
            new TheoryTestDataRow(this, testMethodName)
            : this;
    }

    /// <summary>
    /// Gets the test data as an array of arguments based on the ArgsCode.
    /// </summary>
    /// <returns>An array of test arguments</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when ArgsCode has an invalid value
    /// or when the test data conversion fails.
    /// </exception>
    protected override object?[] GetData()
    => Data;

    /// <summary>
    /// Returns a string representation of the current object.
    /// </summary>
    /// <returns>The value of the <see cref="TestCase"/> property.</returns>
    public override string ToString()
    => TestCase;

    public bool Equals(TestDataTypes.Interfaces.ITestCase? other)
    => other is not null
        && other.TestCase == TestCase;

    #endregion
}