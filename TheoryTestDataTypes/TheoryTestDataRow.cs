/*
 * MIT License
 * 
 * Copyright (c) 2025. Csaba Dudas (CsabaDu)
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */
namespace CsabaDu.DynamicTestData.xUnit.v3.TheoryTestDataTypes;

/// <summary>
/// Represents a row of test data for xUnit.net theory tests with additional configuration options.
/// </summary>
/// <remarks>
/// This record implements both <see cref="ITheoryTestDataRow"/> and <see cref="ISetTheoryDataRow{T}"/> interfaces,
/// providing a fluent API for configuring test data rows.
/// </remarks>
/// <param name="TestData">The test data instance</param>
/// <param name="ArgsCode">Specifies how the test data should be converted to arguments</param>
/// <param name="TestDisplayName">Optional display name for the test</param>
public sealed record class TheoryTestDataRow(
    TestData TestData,
    ArgsCode ArgsCode)
: ITheoryTestDataRow, ISetTheoryDataRow<TheoryTestDataRow>
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
    public TestData TestData { get; init; } = Guard.ArgumentNotNull(TestData, nameof(TestData));

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
    internal static string? GetTestDisplayName(string? testMethodName, [NotNull] TestData testData)
    {
        return testMethodName is not null ?
            GetDisplayName(testMethodName, testData)
            : null;
    }

    /// <summary>
    /// Sets the test display name based on the test method name.
    /// </summary>
    /// <param name="testMethodName">The name of the test method</param>
    /// <returns>A new instance with the updated display name</returns>
    public TheoryTestDataRow SetTestDisplayName(string? testMethodName)
    => this with { TestDisplayName = GetTestDisplayName(testMethodName, TestData) };

    /// <summary>
    /// Sets whether the test should be marked as explicit.
    /// </summary>
    /// <param name="explicitValue">The explicit flag value</param>
    /// <returns>A new instance with the updated explicit value</returns>
    public TheoryTestDataRow SetExplicit(bool? explicitValue)
    => this with { Explicit = explicitValue };

    /// <summary>
    /// Sets the skip reason for the test.
    /// </summary>
    /// <param name="skipValue">The skip reason (null means the test won't be skipped)</param>
    /// <returns>A new instance with the updated skip reason</returns>
    public TheoryTestDataRow SetSkip(string? skipValue)
    => this with { Skip = skipValue };

    /// <summary>
    /// Sets the timeout for the test in milliseconds.
    /// </summary>
    /// <param name="timeoutValue">The timeout value in milliseconds</param>
    /// <returns>A new instance with the updated timeout</returns>
    public TheoryTestDataRow SetTimeout(int? timeoutValue)
    => this with { Timeout = timeoutValue };

    /// <summary>
    /// Adds or updates a trait for the test.
    /// </summary>
    /// <param name="traitName">The name of the trait (cannot be null or empty)</param>
    /// <param name="traitValue">The value of the trait (cannot be null or empty)</param>
    /// <returns>A new instance with the updated traits</returns>
    /// <exception cref="ArgumentException">Thrown when traitName or traitValue is null or empty</exception>
    public TheoryTestDataRow SetTraits(string traitName, string traitValue)
    {
        Guard.ArgumentNotNullOrEmpty(nameof(traitName), traitName);
        Guard.ArgumentNotNullOrEmpty(nameof(traitValue), traitValue);

        if (Traits == null)
        {
            var traits = new Dictionary<string, HashSet<string>>()
            {
                { traitName, [traitValue] }
            };

            return this with { Traits = traits };
        }

        if (Traits!.TryGetValue(traitName, out HashSet<string>? traitvalues))
        {
            _ = traitvalues.Add(traitValue);
        }
        else
        {
            Traits.Add(traitName, [traitValue]);
        }

        return this;
    }

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
        ArgsCode.Properties => string.IsNullOrEmpty(TestData.ExitMode) ?
            TestDataPropertiesToArgs(2)
            : TestDataPropertiesToArgs(1),
        _ => throw new InvalidOperationException(ArgsCodePropertyHasInvalidValue_ + (int)ArgsCode)
    };

    /// <summary>
    /// Converts test data properties to arguments starting from the specified index.
    /// </summary>
    /// <param name="startIndex">The starting index for the arguments</param>
    /// <returns>An array of arguments</returns>
    private object?[] TestDataPropertiesToArgs(int startIndex)
    => TestData.ToArgs(ArgsCode.Properties)[startIndex..];
    #endregion
}