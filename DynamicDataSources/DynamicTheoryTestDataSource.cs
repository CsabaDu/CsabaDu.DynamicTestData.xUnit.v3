// SPDX-License-Identifier: MIT
// Copyright (c) 2025. Csaba Dudas (CsabaDu)

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
    => TheoryTestData = new(ArgsCode);
    #endregion

    #region AddOptional
    /// <summary>
    /// Adds optional test data to the collection with a specific argument conversion strategy.
    /// <remarks>
    /// This method allows temporarily changing the <see cref="ArgsCode"/> for a specific set of test data.
    /// </remarks>
    /// </summary>
    /// <param name="add">Action that adds the test data</param>
    /// <param name="argsCode">The argument conversion strategy to use</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="add"/> is null
    /// </exception>
    public void AddOptional(Action add, ArgsCode? argsCode)
    {
        Guard.ArgumentNotNull(add, nameof(add));
        WithOptionalArgsCode(this, add, argsCode);
    }
    #endregion


    #region Private methods
    #region Add
    /// <summary>
    /// Adds test data to the collection, ensuring type consistency.
    /// <remarks>
    /// All test data added to a single instance must have different <see cref="TestData.TestCase"/> property.
    /// </remarks>
    /// </summary>
    /// <param name="testData">The test data to add</param>
    private void Add(ITestData testData)
    {
        if (TheoryTestData is null)
        {
            SetTheoryTestData(testData);
            return;
        }

        if (TheoryTestData!.Any(testData.Equals))
        {
            return;
        }

        if (TheoryTestData!.Equals(testData))
        {
            TheoryTestData!.Add(testData);
            return;
        }

        SetTheoryTestData(testData);
    }
    #endregion

    #region SetTheoryTestData
    private void SetTheoryTestData(ITestData testData)
    {
        TheoryTestData =
            TypedTheoryTestData(ArgsCode, testData);
    }
    #endregion
    #endregion

    /// <summary>
    /// Adds a test case to the theory test data.
    /// </summary>
    /// <typeparam name="T1">The type of the first argument</typeparam>
    /// <param name="definition">Description of the test case</param>
    /// <param name="expected">Expected result or outcome description</param>
    /// <param name="arg1">First argument value</param>
    public void Add<T1>(
        string definition,
        string expected,
        T1? arg1)
    => Add(new TestData<T1>(
        definition,
        expected,
        arg1));

    /// <inheritdoc cref="Add{T1}" />
    /// <typeparam name="T2">The type of the second argument</typeparam>
    /// <param name="arg2">Second argument value</param>
    public void Add<T1, T2>(
        string definition,
        string expected,
        T1? arg1, T2? arg2)
    => Add(new TestData<T1, T2>(
        definition,
        expected,
        arg1, arg2));

    /// <inheritdoc cref="Add{T1, T2}" />
    /// <typeparam name="T3">The type of the second argument</typeparam>
    /// <param name="arg3">Second argument value</param>
    public void Add<T1, T2, T3>(
        string definition,
        string expected,
        T1? arg1, T2? arg2, T3? arg3)
    => Add(new TestData<T1, T2, T3>(
        definition,
        expected,
        arg1, arg2, arg3));

    /// <inheritdoc cref="Add{T1, T2, T3}" />
    /// <typeparam name="T4">The type of the second argument</typeparam>
    /// <param name="arg4">Second argument value</param>
    public void Add<T1, T2, T3, T4>(
        string definition,
        string expected,
        T1? arg1, T2? arg2, T3? arg3, T4? arg4)
    => Add(new TestData<T1, T2, T3, T4>(
        definition,
        expected,
        arg1, arg2, arg3, arg4));

    /// <inheritdoc cref="Add{T1, T2, T3, T4}" />
    /// <typeparam name="T5">The type of the second argument</typeparam>
    /// <param name="arg5">Second argument value</param>
    public void Add<T1, T2, T3, T4, T5>(
        string definition,
        string expected,
        T1? arg1, T2? arg2, T3? arg3, T4? arg4, T5? arg5)
    => Add(new TestData<T1, T2, T3, T4, T5>(
        definition,
        expected,
        arg1, arg2, arg3, arg4, arg5));

    /// <inheritdoc cref="Add{T1, T2, T3, T4, T5}" />
    /// <typeparam name="T6">The type of the second argument</typeparam>
    /// <param name="arg6">Second argument value</param>
    public void Add<T1, T2, T3, T4, T5, T6>(
        string definition,
        string expected,
        T1? arg1, T2? arg2, T3? arg3, T4? arg4, T5? arg5, T6? arg6)
    => Add(new TestData<T1, T2, T3, T4, T5, T6>(
        definition,
        expected,
        arg1, arg2, arg3, arg4, arg5, arg6));

    /// <inheritdoc cref="Add{T1, T2, T3, T4, T5, T6}" />
    /// <typeparam name="T7">The type of the second argument</typeparam>
    /// <param name="arg7">Second argument value</param>
    public void Add<T1, T2, T3, T4, T5, T6, T7>(
        string definition,
        string expected,
        T1? arg1, T2? arg2, T3? arg3, T4? arg4, T5? arg5, T6? arg6, T7? arg7)
    => Add(new TestData<T1, T2, T3, T4, T5, T6, T7>(
        definition,
        expected,
        arg1, arg2, arg3, arg4, arg5, arg6, arg7));

    /// <inheritdoc cref="Add{T1, T2, T3, T4, T5, T6, T7}" />
    /// <typeparam name="T8">The type of the second argument</typeparam>
    /// <param name="arg8">Second argument value</param>
    public void Add<T1, T2, T3, T4, T5, T6, T7, T8>(
        string definition,
        string expected,
        T1? arg1, T2? arg2, T3? arg3, T4? arg4, T5? arg5, T6? arg6, T7? arg7, T8? arg8)
    => Add(new TestData<T1, T2, T3, T4, T5, T6, T7, T8>(
        definition,
        expected,
        arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8));

    /// <inheritdoc cref="Add{T1, T2, T3, T4, T5, T6, T7, T8}" />
    /// <typeparam name="T9">The type of the second argument</typeparam>
    /// <param name="arg9">Second argument value</param>
    public void Add<T1, T2, T3, T4, T5, T6, T7, T8, T9>(
        string definition,
        string expected,
        T1? arg1, T2? arg2, T3? arg3, T4? arg4, T5? arg5, T6? arg6, T7? arg7, T8? arg8, T9? arg9)
    => Add(new TestData<T1, T2, T3, T4, T5, T6, T7, T8, T9>(
        definition,
        expected,
        arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9));
    #endregion

    #region AddReturns
    /// <summary>
    /// Adds a test case that returns a value type.
    /// </summary>
    /// <typeparam name="TStruct">The return value type (must be a value type)</typeparam>
    /// <typeparam name="T1">The type of the first argument</typeparam>
    /// <param name="definition">Description of the test case</param>
    /// <param name="expected">Expected return value</param>
    /// <param name="arg1">First argument value</param>
    public void AddReturns<TStruct, T1>(
        string definition,
        TStruct expected,
        T1? arg1)
    where TStruct : struct
    => Add(new TestDataReturns<TStruct, T1>(
        definition,
        expected,
        arg1));

    /// <inheritdoc cref="AddReturns{TStruct, T1}" />
    /// <typeparam name="T2">The type of the second argument</typeparam>
    /// <param name="arg2">Second argument value</param>
    public void AddReturns<TStruct, T1, T2>(
        string definition,
        TStruct expected,
        T1? arg1, T2? arg2)
    where TStruct : struct
    => Add(new TestDataReturns<TStruct, T1, T2>(
        definition,
        expected,
        arg1, arg2));

    /// <inheritdoc cref="AddReturns{TStruct, T1, T2}" />
    /// <typeparam name="T3">The type of the second argument</typeparam>
    /// <param name="arg3">Second argument value</param>
    public void AddReturns<TStruct, T1, T2, T3>(
        string definition,
        TStruct expected,
        T1? arg1, T2? arg2, T3? arg3)
    where TStruct : struct
    => Add(new TestDataReturns<TStruct, T1, T2, T3>(
        definition,
        expected,
        arg1, arg2, arg3));

    /// <inheritdoc cref="AddReturns{TStruct, T1, T2, T3}" />
    /// <typeparam name="T4">The type of the second argument</typeparam>
    /// <param name="arg4">Second argument value</param>
    public void AddReturns<TStruct, T1, T2, T3, T4>(
        string definition,
        TStruct expected,
        T1? arg1, T2? arg2, T3? arg3, T4? arg4)
    where TStruct : struct
    => Add(new TestDataReturns<TStruct, T1, T2, T3, T4>(
        definition,
        expected,
        arg1, arg2, arg3, arg4));

    /// <inheritdoc cref="AddReturns{TStruct, T1, T2, T3, T4}" />
    /// <typeparam name="T5">The type of the second argument</typeparam>
    /// <param name="arg5">Second argument value</param>
    public void AddReturns<TStruct, T1, T2, T3, T4, T5>(
        string definition,
        TStruct expected,
        T1? arg1, T2? arg2, T3? arg3, T4? arg4, T5? arg5)
    where TStruct : struct
    => Add(new TestDataReturns<TStruct, T1, T2, T3, T4, T5>(
        definition,
        expected,
        arg1, arg2, arg3, arg4, arg5));

    /// <inheritdoc cref="AddReturns{TStruct, T1, T2, T3, T4, T5}" />
    /// <typeparam name="T6">The type of the second argument</typeparam>
    /// <param name="arg6">Second argument value</param>
    public void AddReturns<TStruct, T1, T2, T3, T4, T5, T6>(
        string definition,
        TStruct expected,
        T1? arg1, T2? arg2, T3? arg3, T4? arg4, T5? arg5, T6? arg6)
    where TStruct : struct
    => Add(new TestDataReturns<TStruct, T1, T2, T3, T4, T5, T6>(
        definition,
        expected,
        arg1, arg2, arg3, arg4, arg5, arg6));

    /// <inheritdoc cref="AddReturns{TStruct, T1, T2, T3, T4, T5, T6}" />
    /// <typeparam name="T7">The type of the second argument</typeparam>
    /// <param name="arg7">Second argument value</param>
    public void AddReturns<TStruct, T1, T2, T3, T4, T5, T6, T7>(
        string definition,
        TStruct expected,
        T1? arg1, T2? arg2, T3? arg3, T4? arg4, T5? arg5, T6? arg6, T7? arg7)
    where TStruct : struct
    => Add(new TestDataReturns<TStruct, T1, T2, T3, T4, T5, T6, T7>(
        definition,
        expected,
        arg1, arg2, arg3, arg4, arg5, arg6, arg7));

    /// <inheritdoc cref="AddReturns{TStruct, T1, T2, T3, T4, T5, T6, T7}" />
    /// <typeparam name="T8">The type of the second argument</typeparam>
    /// <param name="arg8">Second argument value</param>
    public void AddReturns<TStruct, T1, T2, T3, T4, T5, T6, T7, T8>(
        string definition,
        TStruct expected,
        T1? arg1, T2? arg2, T3? arg3, T4? arg4, T5? arg5, T6? arg6, T7? arg7, T8? arg8)
    where TStruct : struct
    => Add(new TestDataReturns<TStruct, T1, T2, T3, T4, T5, T6, T7, T8>(
        definition,
        expected,
        arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8));

    /// <inheritdoc cref="AddReturns{TStruct, T1, T2, T3, T4, T5, T6, T7, T8}" />
    /// <typeparam name="T9">The type of the second argument</typeparam>
    /// <param name="arg9">Second argument value</param>
    public void AddReturns<TStruct, T1, T2, T3, T4, T5, T6, T7, T8, T9>(
        string definition,
        TStruct expected,
        T1? arg1, T2? arg2, T3? arg3, T4? arg4, T5? arg5, T6? arg6, T7? arg7, T8? arg8, T9? arg9)
    where TStruct : struct
    => Add(new TestDataReturns<TStruct, T1, T2, T3, T4, T5, T6, T7, T8, T9>(
        definition,
        expected,
        arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9));
    #endregion

    #region AddThrows
    /// <summary>
    /// Adds a test case that throws an exception.
    /// </summary>
    /// <typeparam name="TException">The expected exception type</typeparam>
    /// <typeparam name="T1">The type of the first argument</typeparam>
    /// <param name="definition">Description of the test case</param>
    /// <param name="expected">Expected exception instance</param>
    /// <param name="arg1">First argument value</param>
    public void AddThrows<TException, T1>(
        string definition,
        TException expected,
        T1? arg1)
    where TException : Exception
    => Add(new TestDataThrows<TException, T1>(
        definition,
        expected,
        arg1));

    /// <inheritdoc cref="AddThrows{TException, T1}" />
    /// <typeparam name="T2">The type of the second argument</typeparam>
    /// <param name="arg2">Second argument value</param>
    public void AddThrows<TException, T1, T2>(
        string definition,
        TException expected,
        T1? arg1, T2? arg2)
    where TException : Exception
    => Add(new TestDataThrows<TException, T1, T2>(
        definition,
        expected,
        arg1, arg2));

    /// <inheritdoc cref="AddThrows{TException, T1, T2}" />
    /// <typeparam name="T3">The type of the second argument</typeparam>
    /// <param name="arg3">Second argument value</param>
    public void AddThrows<TException, T1, T2, T3>(
        string definition,
        TException expected,
        T1? arg1, T2? arg2, T3? arg3)
    where TException : Exception
    => Add(new TestDataThrows<TException, T1, T2, T3>(
        definition,
        expected,
        arg1, arg2, arg3));

    /// <inheritdoc cref="AddThrows{TException, T1, T2, T3}" />
    /// <typeparam name="T4">The type of the second argument</typeparam>
    /// <param name="arg4">Second argument value</param>
    public void AddThrows<TException, T1, T2, T3, T4>(
        string definition,
        TException expected,
        T1? arg1, T2? arg2, T3? arg3, T4? arg4)
    where TException : Exception
    => Add(new TestDataThrows<TException, T1, T2, T3, T4>(
        definition,
        expected,
        arg1, arg2, arg3, arg4));

    /// <inheritdoc cref="AddThrows{TException, T1, T2, T3, T4}" />
    /// <typeparam name="T5">The type of the second argument</typeparam>
    /// <param name="arg5">Second argument value</param>
    public void AddThrows<TException, T1, T2, T3, T4, T5>(
        string definition,
        TException expected,
        T1? arg1, T2? arg2, T3? arg3, T4? arg4, T5? arg5)
    where TException : Exception
    => Add(new TestDataThrows<TException, T1, T2, T3, T4, T5>(
        definition,
        expected,
        arg1, arg2, arg3, arg4, arg5));

    /// <inheritdoc cref="AddThrows{TException, T1, T2, T3, T4, T5}" />
    /// <typeparam name="T6">The type of the second argument</typeparam>
    /// <param name="arg6">Second argument value</param>
    public void AddThrows<TException, T1, T2, T3, T4, T5, T6>(
        string definition,
        TException expected,
        T1? arg1, T2? arg2, T3? arg3, T4? arg4, T5? arg5, T6? arg6)
    where TException : Exception
    => Add(new TestDataThrows<TException, T1, T2, T3, T4, T5, T6>(
        definition,
        expected,
        arg1, arg2, arg3, arg4, arg5, arg6));

    /// <inheritdoc cref="AddThrows{TException, T1, T2, T3, T4, T5, T6}" />
    /// <typeparam name="T7">The type of the second argument</typeparam>
    /// <param name="arg7">Second argument value</param>
    public void AddThrows<TException, T1, T2, T3, T4, T5, T6, T7>(
        string definition,
        TException expected,
        T1? arg1, T2? arg2, T3? arg3, T4? arg4, T5? arg5, T6? arg6, T7? arg7)
    where TException : Exception
    => Add(new TestDataThrows<TException, T1, T2, T3, T4, T5, T6, T7>(
        definition,
        expected,
        arg1, arg2, arg3, arg4, arg5, arg6, arg7));

    /// <inheritdoc cref="AddThrows{TException, T1, T2, T3, T4, T5, T6, T7}" />
    /// <typeparam name="T8">The type of the second argument</typeparam>
    /// <param name="arg8">Second argument value</param>
    public void AddThrows<TException, T1, T2, T3, T4, T5, T6, T7, T8>(
        string definition,
        TException expected,
        T1? arg1, T2? arg2, T3? arg3, T4? arg4, T5? arg5, T6? arg6, T7? arg7, T8? arg8)
    where TException : Exception
    => Add(new TestDataThrows<TException, T1, T2, T3, T4, T5, T6, T7, T8>(
        definition,
        expected,
        arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8));

    /// <inheritdoc cref="AddThrows{TException, T1, T2, T3, T4, T5, T6, T7, T8}" />
    /// <typeparam name="T9">The type of the second argument</typeparam>
    /// <param name="arg9">Second argument value</param>
    public void AddThrows<TException, T1, T2, T3, T4, T5, T6, T7, T8, T9>(
        string definition,
        TException expected,
        T1? arg1, T2? arg2, T3? arg3, T4? arg4, T5? arg5, T6? arg6, T7? arg7, T8? arg8, T9? arg9)
    where TException : Exception
    => Add(new TestDataThrows<TException, T1, T2, T3, T4, T5, T6, T7, T8, T9>(
        definition,
        expected,
        arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9));
    #endregion
}
