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
/// </remarks>
/// <param name="argsCode">The strategy for converting test data to method arguments.</param>
public sealed class TheoryTestData(ArgsCode argsCode) : TheoryDataBase<ITheoryTestDataRow, ITestData>, ITheoryTestData
{
    /// <summary>
    /// Gets the strategy for converting test data to method arguments.
    /// </summary>
    /// <value>
    /// An <see cref="DynamicTestData.DynamicDataSources.ArgsCode"/> value that determines how test data should be
    /// converted to test method arguments. The value is validated to be a defined enum value.
    /// </value>
    public ArgsCode ArgsCode => argsCode.Defined(nameof(argsCode));

    /// <summary>
    /// Converts test data into a theory test data row.
    /// </summary>
    /// <param name="testData">The test data to convert.</param>
    /// <returns>
    /// A new <see cref="TheoryTestDataRow"/> instance configured with the test data,
    /// and argument conversion strategy.
    /// </returns>
    protected override ITheoryTestDataRow Convert(ITestData testData)
    => new TheoryTestDataRow(testData, ArgsCode);
}