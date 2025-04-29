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
namespace CsabaDu.DynamicTestData.xUnit.v3.DynamicDataSources;

/// <summary>
/// Abstract base class for providing dynamic theory test data sources with type-safe argument handling.
/// </summary>
/// <remarks>
/// <para>
/// This class serves as a foundation for creating strongly-typed test data sources
/// that can be used with xUnit theory tests. It maintains type consistency across
/// all added test data and provides various methods for adding different kinds of
/// test cases (normal, return value, and exception cases).
/// </para>
/// <para>
/// The class ensures all test data added to a single instance maintains consistent
/// generic type parameters through runtime checks.
/// </para>
/// </remarks>
/// <param name="argsCode">The strategy for converting test data to method arguments</param>
public abstract class DynamicTheoryTestDataSource(ArgsCode argsCode) : DynamicDataSource(argsCode)
{
    #region Properties
    /// <summary>
    /// Gets the underlying theory test data collection.
    /// </summary>
    /// <value>
    /// A <see cref="TheoryTestDataTypes.TheoryTestData"/> instance that contains all added test cases.
    /// </value>
    protected TheoryTestData? TheoryTestData { get; private set; } = null;
    #endregion

    #region Methods
    #region ResetTheoryTestData
    /// <summary>
    /// Resets the underlying theory test data collection to an empty 'TheoryTestData' instance.
    /// </summary>
    public void ResetTheoryTestData()
    {
        TheoryTestData = new(ArgsCode);
    }
    #endregion

    #region AddOptionalToTheoryTestData
    /// <summary>
    /// Adds optional test data to the collection with a specific argument conversion strategy.
    /// </summary>
    /// <param name="addTestDataToTheoryTestData">Action that adds the test data</param>
    /// <param name="argsCode">The argument conversion strategy to use</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="addTestDataToTheoryTestData"/> is null
    /// </exception>
    /// <remarks>
    /// This method allows temporarily changing the <see cref="ArgsCode"/> for a specific set of test data.
    /// </remarks>
    public void AddOptionalToTheoryTestData(Action addTestDataToTheoryTestData, ArgsCode? argsCode)
    {
        Guard.ArgumentNotNull(addTestDataToTheoryTestData, nameof(addTestDataToTheoryTestData));
        WithOptionalArgsCode(this, addTestDataToTheoryTestData, argsCode);
    }
    #endregion

    #region AddTestDataToTheoryTestData
    /// <summary>
    /// Adds a test case to the theory test data.
    /// </summary>
    /// <typeparam name="T1">The type of the first argument</typeparam>
    /// <param name="definition">Description of the test case</param>
    /// <param name="expected">Expected result or outcome description</param>
    /// <param name="arg1">First argument value</param>
    public void AddTestDataToTheoryTestData<T1>(string definition, string expected, T1? arg1)
    {
        var testData = new TestData<T1>(definition, expected, arg1);
        AddToTheoryTestData(testData);
    }

    /// <inheritdoc cref="AddTestDataToTheoryTestData{T1}" />
    /// <typeparam name="T2">The type of the second argument</typeparam>
    /// <param name="arg2">Second argument value</param>
    public void AddTestDataToTheoryTestData<T1, T2>(string definition, string expected, T1? arg1, T2? arg2)
    {
        var testData = new TestData<T1, T2>(definition, expected, arg1, arg2);
        AddToTheoryTestData(testData);
    }

    /// <inheritdoc cref="AddTestDataToTheoryTestData{T1, T2}" />
    /// <typeparam name="T3">The type of the second argument</typeparam>
    /// <param name="arg3">Second argument value</param>
    public void AddTestDataToTheoryTestData<T1, T2, T3>(string definition, string expected, T1? arg1, T2? arg2, T3? arg3)
    {
        var testData = new TestData<T1, T2, T3>(definition, expected, arg1, arg2, arg3);
        AddToTheoryTestData(testData);
    }

    /// <inheritdoc cref="AddTestDataToTheoryTestData{T1, T2, T3}" />
    /// <typeparam name="T4">The type of the second argument</typeparam>
    /// <param name="arg4">Second argument value</param>
    public void AddTestDataToTheoryTestData<T1, T2, T3, T4>(string definition, string expected, T1? arg1, T2? arg2, T3? arg3, T4? arg4)
    {
        var testData = new TestData<T1, T2, T3, T4>(definition, expected, arg1, arg2, arg3, arg4);
        AddToTheoryTestData(testData);
    }

    /// <inheritdoc cref="AddTestDataToTheoryTestData{T1, T2, T3, T4}" />
    /// <typeparam name="T5">The type of the second argument</typeparam>
    /// <param name="arg5">Second argument value</param>
    public void AddTestDataToTheoryTestData<T1, T2, T3, T4, T5>(string definition, string expected, T1? arg1, T2? arg2, T3? arg3, T4? arg4, T5? arg5)
    {
        var testData = new TestData<T1, T2, T3, T4, T5>(definition, expected, arg1, arg2, arg3, arg4, arg5);
        AddToTheoryTestData(testData);
    }

    /// <inheritdoc cref="AddTestDataToTheoryTestData{T1, T2, T3, T4, T5}" />
    /// <typeparam name="T6">The type of the second argument</typeparam>
    /// <param name="arg6">Second argument value</param>
    public void AddTestDataToTheoryTestData<T1, T2, T3, T4, T5, T6>(string definition, string expected, T1? arg1, T2? arg2, T3? arg3, T4? arg4, T5? arg5, T6? arg6)
    {
        var testData = new TestData<T1, T2, T3, T4, T5, T6>(definition, expected, arg1, arg2, arg3, arg4, arg5, arg6);
        AddToTheoryTestData(testData);
    }

    /// <inheritdoc cref="AddTestDataToTheoryTestData{T1, T2, T3, T4, T5, T6}" />
    /// <typeparam name="T7">The type of the second argument</typeparam>
    /// <param name="arg7">Second argument value</param>
    public void AddTestDataToTheoryTestData<T1, T2, T3, T4, T5, T6, T7>(string definition, string expected, T1? arg1, T2? arg2, T3? arg3, T4? arg4, T5? arg5, T6? arg6, T7? arg7)
    {
        var testData = new TestData<T1, T2, T3, T4, T5, T6, T7>(definition, expected, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        AddToTheoryTestData(testData);
    }

    /// <inheritdoc cref="AddTestDataToTheoryTestData{T1, T2, T3, T4, T5, T6, T7}" />
    /// <typeparam name="T8">The type of the second argument</typeparam>
    /// <param name="arg8">Second argument value</param>
    public void AddTestDataToTheoryTestData<T1, T2, T3, T4, T5, T6, T7, T8>(string definition, string expected, T1? arg1, T2? arg2, T3? arg3, T4? arg4, T5? arg5, T6? arg6, T7? arg7, T8? arg8)
    {
        var testData = new TestData<T1, T2, T3, T4, T5, T6, T7, T8>(definition, expected, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
        AddToTheoryTestData(testData);
    }

    /// <inheritdoc cref="AddTestDataToTheoryTestData{T1, T2, T3, T4, T5, T6, T7, T8}" />
    /// <typeparam name="T9">The type of the second argument</typeparam>
    /// <param name="arg9">Second argument value</param>
    public void AddTestDataToTheoryTestData<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string definition, string expected, T1? arg1, T2? arg2, T3? arg3, T4? arg4, T5? arg5, T6? arg6, T7? arg7, T8? arg8, T9? arg9)
    {
        var testData = new TestData<T1, T2, T3, T4, T5, T6, T7, T8, T9>(definition, expected, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
        AddToTheoryTestData(testData);
    }
    #endregion

    #region AddTestDataReturnsToTheoryTestData
    /// <summary>
    /// Adds a test case that returns a value type.
    /// </summary>
    /// <typeparam name="TStruct">The return value type (must be a value type)</typeparam>
    /// <typeparam name="T1">The type of the first argument</typeparam>
    /// <param name="definition">Description of the test case</param>
    /// <param name="expected">Expected return value</param>
    /// <param name="arg1">First argument value</param>
    public void AddTestDataReturnsToTheoryTestData<TStruct, T1>(string definition, TStruct expected, T1? arg1)
    where TStruct : struct
    {
        var testData = new TestDataReturns<TStruct, T1>(definition, expected, arg1);
        AddToTheoryTestData(testData);
    }

    /// <inheritdoc cref="AddTestDataReturnsToTheoryTestData{TStruct, T1}" />
    /// <typeparam name="T2">The type of the second argument</typeparam>
    /// <param name="arg2">Second argument value</param>
    public void AddTestDataReturnsToTheoryTestData<TStruct, T1, T2>(string definition, TStruct expected, T1? arg1, T2? arg2)
    where TStruct : struct
    {
        var testData = new TestDataReturns<TStruct, T1, T2>(definition, expected, arg1, arg2);
        AddToTheoryTestData(testData);
    }

    /// <inheritdoc cref="AddTestDataReturnsToTheoryTestData{TStruct, T1, T2}" />
    /// <typeparam name="T3">The type of the second argument</typeparam>
    /// <param name="arg3">Second argument value</param>
    public void AddTestDataReturnsToTheoryTestData<TStruct, T1, T2, T3>(string definition, TStruct expected, T1? arg1, T2? arg2, T3? arg3)
    where TStruct : struct
    {
        var testData = new TestDataReturns<TStruct, T1, T2, T3>(definition, expected, arg1, arg2, arg3);
        AddToTheoryTestData(testData);
    }

    /// <inheritdoc cref="AddTestDataReturnsToTheoryTestData{TStruct, T1, T2, T3}" />
    /// <typeparam name="T4">The type of the second argument</typeparam>
    /// <param name="arg4">Second argument value</param>
    public void AddTestDataReturnsToTheoryTestData<TStruct, T1, T2, T3, T4>(string definition, TStruct expected, T1? arg1, T2? arg2, T3? arg3, T4? arg4)
    where TStruct : struct
    {
        var testData = new TestDataReturns<TStruct, T1, T2, T3, T4>(definition, expected, arg1, arg2, arg3, arg4);
        AddToTheoryTestData(testData);
    }

    /// <inheritdoc cref="AddTestDataReturnsToTheoryTestData{TStruct, T1, T2, T3, T4}" />
    /// <typeparam name="T5">The type of the second argument</typeparam>
    /// <param name="arg5">Second argument value</param>
    public void AddTestDataReturnsToTheoryTestData<TStruct, T1, T2, T3, T4, T5>(string definition, TStruct expected, T1? arg1, T2? arg2, T3? arg3, T4? arg4, T5? arg5)
    where TStruct : struct
    {
        var testData = new TestDataReturns<TStruct, T1, T2, T3, T4, T5>(definition, expected, arg1, arg2, arg3, arg4, arg5);
        AddToTheoryTestData(testData);
    }

    /// <inheritdoc cref="AddTestDataReturnsToTheoryTestData{TStruct, T1, T2, T3, T4, T5}" />
    /// <typeparam name="T6">The type of the second argument</typeparam>
    /// <param name="arg6">Second argument value</param>
    public void AddTestDataReturnsToTheoryTestData<TStruct, T1, T2, T3, T4, T5, T6>(string definition, TStruct expected, T1? arg1, T2? arg2, T3? arg3, T4? arg4, T5? arg5, T6? arg6)
    where TStruct : struct
    {
        var testData = new TestDataReturns<TStruct, T1, T2, T3, T4, T5, T6>(definition, expected, arg1, arg2, arg3, arg4, arg5, arg6);
        AddToTheoryTestData(testData);
    }

    /// <inheritdoc cref="AddTestDataReturnsToTheoryTestData{TStruct, T1, T2, T3, T4, T5, T6}" />
    /// <typeparam name="T7">The type of the second argument</typeparam>
    /// <param name="arg7">Second argument value</param>
    public void AddTestDataReturnsToTheoryTestData<TStruct, T1, T2, T3, T4, T5, T6, T7>(string definition, TStruct expected, T1? arg1, T2? arg2, T3? arg3, T4? arg4, T5? arg5, T6? arg6, T7? arg7)
    where TStruct : struct
    {
        var testData = new TestDataReturns<TStruct, T1, T2, T3, T4, T5, T6, T7>(definition, expected, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        AddToTheoryTestData(testData);
    }

    /// <inheritdoc cref="AddTestDataReturnsToTheoryTestData{TStruct, T1, T2, T3, T4, T5, T6, T7}" />
    /// <typeparam name="T8">The type of the second argument</typeparam>
    /// <param name="arg8">Second argument value</param>
    public void AddTestDataReturnsToTheoryTestData<TStruct, T1, T2, T3, T4, T5, T6, T7, T8>(string definition, TStruct expected, T1? arg1, T2? arg2, T3? arg3, T4? arg4, T5? arg5, T6? arg6, T7? arg7, T8? arg8)
    where TStruct : struct
    {
        var testData = new TestDataReturns<TStruct, T1, T2, T3, T4, T5, T6, T7, T8>(definition, expected, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
        AddToTheoryTestData(testData);
    }

    /// <inheritdoc cref="AddTestDataReturnsToTheoryTestData{TStruct, T1, T2, T3, T4, T5, T6, T7, T8}" />
    /// <typeparam name="T9">The type of the second argument</typeparam>
    /// <param name="arg9">Second argument value</param>
    public void AddTestDataReturnsToTheoryTestData<TStruct, T1, T2, T3, T4, T5, T6, T7, T8, T9>(string definition, TStruct expected, T1? arg1, T2? arg2, T3? arg3, T4? arg4, T5? arg5, T6? arg6, T7? arg7, T8? arg8, T9? arg9)
    where TStruct : struct
    {
        var testData = new TestDataReturns<TStruct, T1, T2, T3, T4, T5, T6, T7, T8, T9>(definition, expected, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
        AddToTheoryTestData(testData);
    }
    #endregion

    #region AddTestDataThrowsToTheoryTestData
    /// <summary>
    /// Adds a test case that throws an exception.
    /// </summary>
    /// <typeparam name="TException">The expected exception type</typeparam>
    /// <typeparam name="T1">The type of the first argument</typeparam>
    /// <param name="definition">Description of the test case</param>
    /// <param name="expected">Expected exception instance</param>
    /// <param name="arg1">First argument value</param>
    public void AddTestDataThrowsToTheoryTestData<TException, T1>(string definition, TException expected, T1? arg1)
    where TException : Exception
    {
        var testData = new TestDataThrows<TException, T1>(definition, expected, arg1);
        AddToTheoryTestData(testData);
    }

    /// <inheritdoc cref="AddTestDataThrowsToTheoryTestData{TException, T1}" />
    /// <typeparam name="T2">The type of the second argument</typeparam>
    /// <param name="arg2">Second argument value</param>
    public void AddTestDataThrowsToTheoryTestData<TException, T1, T2>(string definition, TException expected, T1? arg1, T2? arg2)
    where TException : Exception
    {
        var testData = new TestDataThrows<TException, T1, T2>(definition, expected, arg1, arg2);
        AddToTheoryTestData(testData);
    }

    /// <inheritdoc cref="AddTestDataThrowsToTheoryTestData{TException, T1, T2}" />
    /// <typeparam name="T3">The type of the second argument</typeparam>
    /// <param name="arg3">Second argument value</param>
    public void AddTestDataThrowsToTheoryTestData<TException, T1, T2, T3>(string definition, TException expected, T1? arg1, T2? arg2, T3? arg3)
    where TException : Exception
    {
        var testData = new TestDataThrows<TException, T1, T2, T3>(definition, expected, arg1, arg2, arg3);
        AddToTheoryTestData(testData);
    }

    /// <inheritdoc cref="AddTestDataThrowsToTheoryTestData{TException, T1, T2, T3}" />
    /// <typeparam name="T4">The type of the second argument</typeparam>
    /// <param name="arg4">Second argument value</param>
    public void AddTestDataThrowsToTheoryTestData<TException, T1, T2, T3, T4>(string definition, TException expected, T1? arg1, T2? arg2, T3? arg3, T4? arg4)
    where TException : Exception
    {
        var testData = new TestDataThrows<TException, T1, T2, T3, T4>(definition, expected, arg1, arg2, arg3, arg4);
        AddToTheoryTestData(testData);
    }

    /// <inheritdoc cref="AddTestDataThrowsToTheoryTestData{TException, T1, T2, T3, T4}" />
    /// <typeparam name="T5">The type of the second argument</typeparam>
    /// <param name="arg5">Second argument value</param>
    public void AddTestDataThrowsToTheoryTestData<TException, T1, T2, T3, T4, T5>(string definition, TException expected, T1? arg1, T2? arg2, T3? arg3, T4? arg4, T5? arg5)
    where TException : Exception
    {
        var testData = new TestDataThrows<TException, T1, T2, T3, T4, T5>(definition, expected, arg1, arg2, arg3, arg4, arg5);
        AddToTheoryTestData(testData);
    }

    /// <inheritdoc cref="AddTestDataThrowsToTheoryTestData{TException, T1, T2, T3, T4, T5}" />
    /// <typeparam name="T6">The type of the second argument</typeparam>
    /// <param name="arg6">Second argument value</param>
    public void AddTestDataThrowsToTheoryTestData<TException, T1, T2, T3, T4, T5, T6>(string definition, TException expected, T1? arg1, T2? arg2, T3? arg3, T4? arg4, T5? arg5, T6? arg6)
    where TException : Exception
    {
        var testData = new TestDataThrows<TException, T1, T2, T3, T4, T5, T6>(definition, expected, arg1, arg2, arg3, arg4, arg5, arg6);
        AddToTheoryTestData(testData);
    }

    /// <inheritdoc cref="AddTestDataThrowsToTheoryTestData{TException, T1, T2, T3, T4, T5, T6}" />
    /// <typeparam name="T7">The type of the second argument</typeparam>
    /// <param name="arg7">Second argument value</param>
    public void AddTestDataThrowsToTheoryTestData<TException, T1, T2, T3, T4, T5, T6, T7>(string definition, TException expected, T1? arg1, T2? arg2, T3? arg3, T4? arg4, T5? arg5, T6? arg6, T7? arg7)
    where TException : Exception
    {
        var testData = new TestDataThrows<TException, T1, T2, T3, T4, T5, T6, T7>(definition, expected, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        AddToTheoryTestData(testData);
    }

    /// <inheritdoc cref="AddTestDataThrowsToTheoryTestData{TException, T1, T2, T3, T4, T5, T6, T7}" />
    /// <typeparam name="T8">The type of the second argument</typeparam>
    /// <param name="arg8">Second argument value</param>
    public void AddTestDataThrowsToTheoryTestData<TException, T1, T2, T3, T4, T5, T6, T7, T8>(string definition, TException expected, T1? arg1, T2? arg2, T3? arg3, T4? arg4, T5? arg5, T6? arg6, T7? arg7, T8? arg8)
    where TException : Exception
    {
        var testData = new TestDataThrows<TException, T1, T2, T3, T4, T5, T6, T7, T8>(definition, expected, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
        AddToTheoryTestData(testData);
    }

    /// <inheritdoc cref="AddTestDataThrowsToTheoryTestData{TException, T1, T2, T3, T4, T5, T6, T7, T8}" />
    /// <typeparam name="T9">The type of the second argument</typeparam>
    /// <param name="arg9">Second argument value</param>
    public void AddTestDataThrowsToTheoryTestData<TException, T1, T2, T3, T4, T5, T6, T7, T8, T9>(string definition, TException expected, T1? arg1, T2? arg2, T3? arg3, T4? arg4, T5? arg5, T6? arg6, T7? arg7, T8? arg8, T9? arg9)
    where TException : Exception
    {
        var testData = new TestDataThrows<TException, T1, T2, T3, T4, T5, T6, T7, T8, T9>(definition, expected, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
        AddToTheoryTestData(testData);
    }
    #endregion

    #region AddToTheoryTestData
    /// <summary>
    /// Adds test data to the collection, ensuring type consistency.
    /// </summary>
    /// <param name="testData">The test data to add</param>
    /// <remarks>
    /// All test data added to a single instance must have different <see cref="TestData.TestCase"/> property.
    /// </remarks>
    private void AddToTheoryTestData(TestData testData)
    {
        TheoryTestDataRow? firstRow = TheoryTestData?.FirstOrDefault();

        if (TheoryTestData is null
            || TheoryTestData.Any(t => t.TestData.TestCase == testData.TestCase))
        {
            ResetTheoryTestData();
        }

        TheoryTestData!.Add(testData);
    }
    #endregion
    #endregion
}
