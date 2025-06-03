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
    /// <summary>
    /// Initializes a new instance of the <see cref="TheoryTestDataRow"/> class with the specified test data and
    /// argument code.
    /// </summary>
    /// <remarks>The constructor processes the provided <paramref name="testData"/> and <paramref
    /// name="argsCode"/> to generate the test case parameters and determine the test case identifier. If <paramref
    /// name="testData"/> implements <see cref="IExpected"/>, additional processing is applied.</remarks>
    /// <param name="testData">The test data to be converted into parameters for the test case. Must implement <see cref="ITestData"/>.</param>
    /// <param name="argsCode">The argument code associated with the test case. Used to define additional context or behavior.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="testData"/> parameter is null.</exception>
    /// <exception cref="InvalidEnumArgumentException">Thrown is <paramref name="argsCode"/> parameter has invalid value.</exception>
    public TheoryTestDataRow(
        ITestData testData,
        ArgsCode argsCode)
    {
        Params = TestDataToParams(
            testData,
            argsCode,
            out string testCase);
        ArgsCode = argsCode;
        TestCase = testCase;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TheoryTestDataRow"/> class by copying data from another instance
    /// and optionally specifying a test method name.
    /// </summary>
    /// <remarks>This constructor copies all relevant properties from the provided <paramref name="other"/>
    /// instance, including test data, arguments, and traits. If <paramref name="testMethodName"/> is provided, it may
    /// be used to generate a custom display name for the test case.</remarks>
    /// <param name="other">The <see cref="ITheoryTestDataRow"/> instance to copy data from. Cannot be <see langword="null"/>.</param>
    /// <param name="testMethodName">An optional test method name to use for generating the display name. If <see langword="null"/>, the display name
    /// from <paramref name="other"/> will be used.</param>
    private TheoryTestDataRow(
        ITheoryTestDataRow other,
        string? testMethodName)
    {
        Params = other.Params;
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

    #region Properties
    /// <inheritdoc cref="ITestDataRow.Data"/>/>
    public object?[] Params { get; init;}

    /// <inheritdoc cref="IArgsCode.ArgsCode"/>
    public ArgsCode ArgsCode { get; init; }

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
    => string.IsNullOrEmpty(testMethodName) ?
        this
        : new TheoryTestDataRow(this, testMethodName);

    /// <summary>
    /// Determines whether the current instance is equal to the specified <see
    /// cref="ITestCaseName"/>.
    /// </summary>
    /// <param name="other">The <see cref="ITestCaseName"/> to compare with the current instance.</param>
    /// <returns><see langword="true"/> if the specified <see cref="ITestCaseName"/> is not <see
    /// langword="null"/>  and its <c>TestCase</c> property is equal to the <c>TestCase</c> property of the current
    /// instance; otherwise, <see langword="false"/>.</returns>
    public bool Equals(ITestCaseName? other)
    => other is not null
        && other.TestCase == TestCase;

    /// <summary>
    /// Gets the test data as an array of arguments based on the ArgsCode.
    /// </summary>
    /// <returns>An array of test arguments</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when ArgsCode has an invalid value
    /// or when the test data conversion fails.
    /// </exception>
    protected override object?[] GetData()
    => Params;
    #endregion
}