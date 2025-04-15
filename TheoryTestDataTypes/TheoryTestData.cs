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
/// Represents a collection of theory test data with configurable argument conversion strategy.
/// </summary>
/// <remarks>
/// <para>
/// This sealed class provides a strongly-typed container for theory test data rows,
/// inheriting from <see cref="TheoryDataBase{TTheoryDataRow, TTestData}"/> and implementing
/// <see cref="ITheoryTestData"/>.
/// </para>
/// <para>
/// The test method name can be initialized once using <see cref="InitTestMethodName(string)"/>,
/// which is used to generate display names for test cases.
/// </para>
/// </remarks>
/// <param name="argsCode">The strategy for converting test data to method arguments.</param>
public sealed class TheoryTestData(ArgsCode argsCode) : TheoryDataBase<TheoryTestDataRow, TestData>, ITheoryTestData
{
    /// <summary>
    /// Error message used when attempting to reinitialize the test method name.
    /// </summary>
    internal const string TestMethodNameIsNotNullMessage = "Test method name is not null.";

    private string? _testMethodName = null;

    /// <summary>
    /// Gets the strategy for converting test data to method arguments.
    /// </summary>
    /// <value>
    /// An <see cref="ArgsCode"/> value that determines how test data should be
    /// converted to test method arguments. The value is validated to be a defined enum value.
    /// </value>
    public ArgsCode ArgsCode => argsCode.Defined(nameof(argsCode));

    /// <summary>
    /// Initializes the test method name for display purposes.
    /// </summary>
    /// <param name="testMethodName">The name of the test method.</param>
    /// <exception cref="InvalidOperationException">
    /// Thrown when attempting to initialize the test method name more than once.
    /// </exception>
    /// <remarks>
    /// <para>
    /// This method must be called exactly once before adding test data rows.
    /// The test method name is used to generate display names for test cases.
    /// </para>
    /// <para>
    /// Subsequent calls will throw an <see cref="InvalidOperationException"/>.
    /// </para>
    /// </remarks>
    public void InitTestMethodName(string testMethodName)
    {
        if (_testMethodName is null)
        {
            _testMethodName = testMethodName;
        }
        else
        {
            throw new InvalidOperationException(TestMethodNameIsNotNullMessage);
        }
    }

    /// <summary>
    /// Converts test data into a theory test data row.
    /// </summary>
    /// <param name="testData">The test data to convert.</param>
    /// <returns>
    /// A new <see cref="TheoryTestDataRow"/> instance configured with the test data,
    /// argument conversion strategy, and generated display name.
    /// </returns>
    /// <remarks>
    /// The display name is generated using the initialized test method name and test case information.
    /// </remarks>
    protected override TheoryTestDataRow Convert(TestData testData)
    => new(testData, ArgsCode, GetTestDisplayName(_testMethodName, testData));
}