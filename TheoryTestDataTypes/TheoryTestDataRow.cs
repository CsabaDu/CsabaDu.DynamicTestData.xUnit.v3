// SPDX-License-Identifier: MIT
// Copyright (c) 2025. Csaba Dudas (CsabaDu)

namespace CsabaDu.DynamicTestData.xUnit.v3.TheoryTestDataTypes;

/// <summary>
/// Represents a row of test data for xUnit.net theory tests with additional configuration options.
/// </summary>
/// <param name="TestData">The test data instance</param>
/// <param name="ArgsCode">Specifies how the test data should be converted to arguments</param>
public sealed record class TheoryTestDataRow(ITestData TestData, ArgsCode ArgsCode)
: ITheoryTestDataRow
{
    #region Constants
    /// <summary>
    /// Error message for invalid ArgsCode property value
    /// </summary>
    internal const string ArgsCodePropertyHasInvalidValue_ = "ArgsCode property has invalid value: ";
    #endregion

    #region Properties
    /// <summary>
    /// Gets the test data instance. This property cannot be null.
    /// </summary>
    [NotNull]
    public ITestData TestData { get; init; } = Guard.ArgumentNotNull(TestData, nameof(TestData));

    /// <summary>
    /// Gets the code specifying how the test data should be converted to arguments.
    /// </summary>
    public ArgsCode ArgsCode { get; init; } = ArgsCode.Defined(nameof(ArgsCode));

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
    public Dictionary<string, HashSet<string>>? Traits { get; init; } = [];
    #endregion

    #region Methods
    /// <summary>
    /// Gets the test display name based on the test method name and test data.
    /// </summary>
    /// <param name="testMethodName">The name of the test method</param>
    /// <param name="testData">The test data instance (cannot be null)</param>
    /// <returns>The formatted display name or null if testMethodName is null</returns>
    /// <exception cref="ArgumentNullException">Thrown when testData is null</exception>
    internal static string? GetTestDisplayName(string? testMethodName, [NotNull] ITestData testData)
    => testMethodName is not null ?
        GetDisplayName(testMethodName, testData)
        : null;

    /// <summary>
    /// Sets the test display name based on the test method name.
    /// </summary>
    /// <param name="testMethodName">The name of the test method</param>
    /// <returns>A new instance with the updated display name</returns>
    public ITheoryTestDataRow SetTestDisplayName(string? testMethodName)
    => this with { TestDisplayName = GetTestDisplayName(testMethodName, TestData) };

    /// <summary>
    /// Gets the test data as an array of arguments based on the ArgsCode.
    /// </summary>
    /// <returns>An array of test arguments</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when ArgsCode has an invalid value
    /// </exception>
    public object?[] GetData() => ArgsCode switch
    {
        ArgsCode.Instance => [TestData],
        ArgsCode.Properties => TestDataPropertiesToArgs(),
        _ => throw new InvalidOperationException(ArgsCodePropertyHasInvalidValue_ + (int)ArgsCode)
    };

    /// <summary>
    /// Converts test data properties to arguments starting from the specified index.
    /// </summary>
    /// <returns>An array of arguments</returns>
    private object?[] TestDataPropertiesToArgs()
    {
        return TestData is ITestDataReturns or ITestDataThrows ?
            testDataPropertiesToArgs(1)
            : testDataPropertiesToArgs(2);

        #region Local methods
        object?[] testDataPropertiesToArgs(int startIndex)
        => TestData.ToArgs(ArgsCode.Properties)[startIndex..];
        #endregion
    }
    #endregion
}