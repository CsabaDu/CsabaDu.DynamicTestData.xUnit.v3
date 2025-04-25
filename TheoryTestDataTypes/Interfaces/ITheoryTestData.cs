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
using System.Collections.Immutable;

namespace CsabaDu.DynamicTestData.xUnit.v3.TheoryTestDataTypes.Interfaces;

/// <summary>
/// Represents a container for theory test data with initialization capabilities.
/// </summary>
/// <remarks>
/// This interface provides access to the argument conversion strategy (<see cref="DynamicTestData.DynamicDataSources.ArgsCode"/>)
/// and allows initialization with a test method name.
/// </remarks>
public interface ITheoryTestData
{
    /// <summary>
    /// Gets the strategy for converting test data to method arguments.
    /// </summary>
    /// <value>
    /// An <see cref="DynamicTestData.DynamicDataSources.ArgsCode"/> enum value that determines how test data should be
    /// converted to test method arguments.
    /// </value>
    ArgsCode ArgsCode { get; }

    ///// <summary>
    ///// Initializes the test data with the specified test method name.
    ///// </summary>
    ///// <param name="testMethodName">The name of the test method to associate with this test data.</param>
    ///// <remarks>
    ///// This method should typically be called once before the test data is used,
    ///// to properly set up test-specific information.
    ///// </remarks>
    //void InitTestMethodName(string? testMethodName);
}