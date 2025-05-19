// SPDX-License-Identifier: MIT
// Copyright (c) 2025. Csaba Dudas (CsabaDu)

using System.ComponentModel;

namespace CsabaDu.DynamicTestData.xUnit.v3.TheoryTestDataTypes;

/// <summary>
/// Represents a row of test data for xUnit.net theory tests with additional configuration options.
/// </summary>
/// <param name="testData">The test data instance</param>
/// <param name="argsCode">Specifies how the test data should be converted to arguments</param>
public sealed record class TheoryTestDataRow(ITestData testData, ArgsCode argsCode)
: ITheoryTestDataRow
{
    #region Properties
    /// <summary>
    /// Gets the test data instance. This property cannot be null.
    /// </summary>
    [NotNull]
    public ITestData TestData { get; init; } = Guard.ArgumentNotNull(testData, nameof(testData));

    /// <summary>
    /// Gets the code specifying how the test data should be converted to arguments.
    /// </summary>
    public ArgsCode ArgsCode { get; init; } = argsCode.Defined(nameof(argsCode));

    /// <summary>
    /// Gets or sets whether the test should be marked as explicit.
    /// </summary>
    public bool? Explicit { get; init; } = null;

    /// <summary>
    /// Gets or sets the skip reason for the test (null means the test won't be skipped).
    /// </summary>
    public string? Skip { get; init; } = null;

    /// <summary>
    /// Gets or sets the display name for the test.
    /// </summary>
    public string? TestDisplayName { get; init; } = null;

    /// <summary>
    /// Gets or sets the timeout in milliseconds for the test.
    /// </summary>
    public int? Timeout { get; init; } = null;

    /// <summary>
    /// Gets or sets the traits associated with the test.
    /// </summary>
    public Dictionary<string, HashSet<string>>? Traits { get; init; } = null;

    /// <summary>
    /// Gets the error message for invalid argsCode property value
    /// </summary>
    internal string ArgsCodePropertyHasInvalidValueMessage
    => $"ArgsCode property has invalid value: {(int)ArgsCode}";
    #endregion

    #region Methods
    /// <summary>
    /// Sets the test display name based on the test method name.
    /// </summary>
    /// <param name="testMethodName">The name of the test method</param>
    /// <returns>A new instance with the updated display name</returns>
    public ITheoryTestDataRow SetTestDisplayName(string? testMethodName)
    {
        if (string.IsNullOrEmpty(testMethodName)) return this;

        return this with
        {
            TestDisplayName = GetDisplayName(testMethodName, TestData)
        };
    }

    /// <summary>
    /// Gets the test data as an array of arguments based on the argsCode.
    /// </summary>
    /// <returns>An array of test arguments</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when argsCode has an invalid value
    /// or when the test data conversion fails.
    /// </exception>
    public object?[] GetData()
    {
        try
        {
            return TestData.ToParams(ArgsCode, TestData is IExpected);
        }
        catch (InvalidEnumArgumentException)
        {
            throw new InvalidOperationException(
                ArgsCodePropertyHasInvalidValueMessage);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(
                "'TestData.ToParams' conversion failed.",
                ex);
        }
    }
    #endregion
}