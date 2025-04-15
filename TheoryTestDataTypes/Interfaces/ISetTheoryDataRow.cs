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
namespace CsabaDu.DynamicTestData.xUnit.v3.TheoryTestDataTypes.Interfaces;

/// <summary>
/// Provides a fluent interface for configuring theory test data rows.
/// </summary>
/// <typeparam name="TTheoryDataRow">The type of theory data row being configured, which must implement <see cref="ITheoryDataRow"/>.</typeparam>
/// <remarks>
/// This interface defines a builder pattern for configuring test data rows with optional settings
/// like display name, explicit marking, skip reason, timeout, and traits.
/// </remarks>
public interface ISetTheoryDataRow<TTheoryDataRow> where TTheoryDataRow : ITheoryDataRow
{
    /// <summary>
    /// Sets the display name for the test row based on the test method name.
    /// </summary>
    /// <param name="testMethodName">The name of the test method (optional). If null, the default display name will be used.</param>
    /// <returns>The configured test data row instance.</returns>
    TTheoryDataRow SetTestDisplayName(string? testMethodName);

    /// <summary>
    /// Marks the test as explicit or clears the explicit marking.
    /// </summary>
    /// <param name="explicitValue">
    /// True to mark the test as explicit, false to mark as non-explicit, 
    /// or null to clear any explicit marking (default behavior).
    /// </param>
    /// <returns>The configured test data row instance.</returns>
    TTheoryDataRow SetExplicit(bool? explicitValue);

    /// <summary>
    /// Sets the skip reason for the test or clears the skip setting.
    /// </summary>
    /// <param name="skipValue">
    /// The reason to skip the test, or null to indicate the test should run.
    /// </param>
    /// <returns>The configured test data row instance.</returns>
    TTheoryDataRow SetSkip(string? skipValue);

    /// <summary>
    /// Sets the timeout for the test execution in milliseconds or clears the timeout setting.
    /// </summary>
    /// <param name="timeOutValue">
    /// The maximum execution time in milliseconds, or null to indicate no timeout.
    /// </param>
    /// <returns>The configured test data row instance.</returns>
    TTheoryDataRow SetTimeout(int? timeOutValue);

    /// <summary>
    /// Adds a trait to the test with the specified name and value.
    /// </summary>
    /// <param name="traitName">The name of the trait (must not be null or empty).</param>
    /// <param name="traitValue">The value of the trait (must not be null or empty).</param>
    /// <returns>The configured test data row instance.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when either <paramref name="traitName"/> or <paramref name="traitValue"/> is null or empty.
    /// </exception>
    TTheoryDataRow SetTraits(string traitName, string traitValue);
}