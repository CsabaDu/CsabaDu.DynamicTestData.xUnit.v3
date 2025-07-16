// SPDX-License-Identifier: MIT
// Copyright (c) 2025. Csaba Dudas (CsabaDu)

namespace CsabaDu.DynamicTestData.xUnit.v3.Statics;

public static class Extensions
{
    #region ToTheoryTestDataRow
    public static TheoryTestDataRow<TTestData> ToTheoryTestDataRow<TTestData>(
        this TTestData testData,
        ArgsCode argsCode,
        string? testMethodName)
    where TTestData : notnull, ITestData
    => new(
        testData,
        argsCode,
        testMethodName);
    #endregion

    #region ToTheoryDataRow
    public static ITheoryDataRow ToTheoryDataRow<TResult, T1>(
        this ITestData<TResult, T1> testData,
        ArgsCode argsCode,
        string? testMethodName)
    where TResult : notnull
    {
        var instanceRow =
            InstanceToTheoryDataRowOrNull(
                testData,
                argsCode,
                testMethodName,
                out string? testDisplayName);

        return instanceRow is not null ?
            instanceRow
            : testData is IExpected ?
                new TheoryDataRow<TResult, T1?>(
                    testData.Expected,
                    testData.Arg1)
                {
                    TestDisplayName = testDisplayName
                }
                : new TheoryDataRow<T1?>(
                    testData.Arg1)
                {
                    TestDisplayName = testDisplayName
                };
    }

    public static ITheoryDataRow ToTheoryDataRow<TResult, T1, T2>(
        this ITestData<TResult, T1, T2> testData,
        ArgsCode argsCode,
        string? testMethodName)
    where TResult : notnull
    {
        var instanceRow =
            InstanceToTheoryDataRowOrNull(
                testData,
                argsCode,
                testMethodName,
                out string? testDisplayName);

        return instanceRow is not null ?
            instanceRow
            : testData is IExpected ?
                new TheoryDataRow<TResult, T1?, T2?>(
                    testData.Expected,
                    testData.Arg1,
                    testData.Arg2)
                {
                    TestDisplayName = testDisplayName
                }
                : new TheoryDataRow<T1?, T2?>(
                    testData.Arg1,
                    testData.Arg2)
                {
                    TestDisplayName = testDisplayName
                };
    }

    public static ITheoryDataRow ToTheoryDataRow< TResult, T1, T2, T3>(
        this ITestData<TResult, T1, T2, T3> testData,
        ArgsCode argsCode,
        string? testMethodName)
    where TResult : notnull
    {
        var instanceRow =
            InstanceToTheoryDataRowOrNull(
                testData,
                argsCode,
                testMethodName,
                out string? testDisplayName);

        return instanceRow is not null ?
            instanceRow
            : testData is IExpected ?
                new TheoryDataRow<TResult, T1?, T2?, T3?>(
                    testData.Expected,
                    testData.Arg1,
                    testData.Arg2,
                    testData.Arg3)
                {
                    TestDisplayName = testDisplayName
                }
                : new TheoryDataRow<T1?, T2?, T3?>(
                    testData.Arg1,
                    testData.Arg2,
                    testData.Arg3)
                {
                    TestDisplayName = testDisplayName
                };
    }

    public static ITheoryDataRow ToTheoryDataRow<TResult, T1, T2, T3, T4>(
        this ITestData<TResult, T1, T2, T3, T4> testData,
        ArgsCode argsCode,
        string? testMethodName)
    where TResult : notnull
    {
        var instanceRow =
            InstanceToTheoryDataRowOrNull(
                testData,
                argsCode,
                testMethodName,
                out string? testDisplayName);

        return instanceRow is not null ?
            instanceRow
            : testData is IExpected ?
                new TheoryDataRow<TResult, T1?, T2?, T3?, T4?>(
                    testData.Expected,
                    testData.Arg1,
                    testData.Arg2,
                    testData.Arg3,
                    testData.Arg4)
                {
                    TestDisplayName = testDisplayName
                }
                : new TheoryDataRow<T1?, T2?, T3?, T4?>(
                    testData.Arg1,
                    testData.Arg2,
                    testData.Arg3,
                    testData.Arg4)
                {
                    TestDisplayName = testDisplayName
                };
    }

    public static ITheoryDataRow ToTheoryDataRow<TResult, T1, T2, T3, T4, T5>(
        this ITestData<TResult, T1, T2, T3, T4, T5> testData,
        ArgsCode argsCode,
        string? testMethodName)
    where TResult : notnull
    {
        var instanceRow =
            InstanceToTheoryDataRowOrNull(
                testData,
                argsCode,
                testMethodName,
                out string? testDisplayName);

        return instanceRow is not null ?
            instanceRow
            : testData is IExpected ?
                new TheoryDataRow<TResult, T1?, T2?, T3?, T4?, T5?>(
                    testData.Expected,
                    testData.Arg1,
                    testData.Arg2,
                    testData.Arg3,
                    testData.Arg4,
                    testData.Arg5)
                {
                    TestDisplayName = testDisplayName
                }
                : new TheoryDataRow<T1?, T2?, T3?, T4?, T5?>(
                    testData.Arg1,
                    testData.Arg2,
                    testData.Arg3,
                    testData.Arg4,
                    testData.Arg5)
                {
                    TestDisplayName = testDisplayName
                };
    }

    public static ITheoryDataRow ToTheoryDataRow<TResult, T1, T2, T3, T4, T5, T6>(
        this ITestData<TResult, T1, T2, T3, T4, T5, T6> testData,
        ArgsCode argsCode,
        string? testMethodName)
    where TResult : notnull
    {
        var instanceRow =
            InstanceToTheoryDataRowOrNull(
                testData,
                argsCode,
                testMethodName,
                out string? testDisplayName);

        return instanceRow is not null ?
            instanceRow
            : testData is IExpected ?
                new TheoryDataRow<TResult, T1?, T2?, T3?, T4?, T5?, T6?>(
                    testData.Expected,
                    testData.Arg1,
                    testData.Arg2,
                    testData.Arg3,
                    testData.Arg4,
                    testData.Arg5,
                    testData.Arg6)
                {
                    TestDisplayName = testDisplayName
                }
                : new TheoryDataRow<T1?, T2?, T3?, T4?, T5?, T6?>(
                    testData.Arg1,
                    testData.Arg2,
                    testData.Arg3,
                    testData.Arg4,
                    testData.Arg5,
                    testData.Arg6)
                {
                    TestDisplayName = testDisplayName
                };
    }

    public static ITheoryDataRow ToTheoryDataRow<TResult, T1, T2, T3, T4, T5, T6, T7>(
        this ITestData<TResult, T1, T2, T3, T4, T5, T6, T7> testData,
        ArgsCode argsCode,
        string? testMethodName)
    where TResult : notnull
    {
        var instanceRow =
            InstanceToTheoryDataRowOrNull(
                testData,
                argsCode,
                testMethodName,
                out string? testDisplayName);

        return instanceRow is not null ?
            instanceRow
            : testData is IExpected ?
                new TheoryDataRow<TResult, T1?, T2?, T3?, T4?, T5?, T6?, T7?>(
                    testData.Expected,
                    testData.Arg1,
                    testData.Arg2,
                    testData.Arg3,
                    testData.Arg4,
                    testData.Arg5,
                    testData.Arg6,
                    testData.Arg7)
                {
                    TestDisplayName = testDisplayName
                }
                : new TheoryDataRow<T1?, T2?, T3?, T4?, T5?, T6?, T7?>(
                    testData.Arg1,
                    testData.Arg2,
                    testData.Arg3,
                    testData.Arg4,
                    testData.Arg5,
                    testData.Arg6,
                    testData.Arg7)
                {
                    TestDisplayName = testDisplayName
                };
    }

    public static ITheoryDataRow ToTheoryDataRow<TResult, T1, T2, T3, T4, T5, T6, T7, T8>(
        this ITestData<TResult, T1, T2, T3, T4, T5, T6, T7, T8> testData,
        ArgsCode argsCode,
        string? testMethodName)
    where TResult : notnull
    {
        var instanceRow =
            InstanceToTheoryDataRowOrNull(
                testData,
                argsCode,
                testMethodName,
                out string? testDisplayName);

        return instanceRow is not null ?
            instanceRow
            : testData is IExpected ?
                new TheoryDataRow<TResult, T1?, T2?, T3?, T4?, T5?, T6?, T7?, T8?>(
                    testData.Expected,
                    testData.Arg1,
                    testData.Arg2,
                    testData.Arg3,
                    testData.Arg4,
                    testData.Arg5,
                    testData.Arg6,
                    testData.Arg7,
                    testData.Arg8)
                {
                    TestDisplayName = testDisplayName
                }
                : new TheoryDataRow<T1?, T2?, T3?, T4?, T5?, T6?, T7?, T8?>(
                    testData.Arg1,
                    testData.Arg2,
                    testData.Arg3,
                    testData.Arg4,
                    testData.Arg5,
                    testData.Arg6,
                    testData.Arg7,
                    testData.Arg8)
                {
                    TestDisplayName = testDisplayName
                };
    }

    public static ITheoryDataRow ToTheoryDataRow<TResult, T1, T2, T3, T4, T5, T6, T7, T8, T9>(
        this ITestData<TResult, T1, T2, T3, T4, T5, T6, T7, T8, T9> testData,
        ArgsCode argsCode,
        string? testMethodName)
    where TResult : notnull
    {
        var instanceRow =
            InstanceToTheoryDataRowOrNull(
                testData,
                argsCode,
                testMethodName,
                out string? testDisplayName);

        return instanceRow is not null ?
            instanceRow
            : testData is IExpected ?
                new TheoryDataRow<TResult, T1?, T2?, T3?, T4?, T5?, T6?, T7?, T8?, T9?>(
                    testData.Expected,
                    testData.Arg1,
                    testData.Arg2,
                    testData.Arg3,
                    testData.Arg4,
                    testData.Arg5,
                    testData.Arg6,
                    testData.Arg7,
                    testData.Arg8,
                    testData.Arg9)
                {
                    TestDisplayName = testDisplayName
                }
                : new TheoryDataRow<T1?, T2?, T3?, T4?, T5?, T6?, T7?, T8?, T9?>(
                    testData.Arg1,
                    testData.Arg2,
                    testData.Arg3,
                    testData.Arg4,
                    testData.Arg5,
                    testData.Arg6,
                    testData.Arg7,
                    testData.Arg8,
                    testData.Arg9)
                {
                    TestDisplayName = testDisplayName
                };
    }
    #endregion

    #region Private InstanceToTheoryDataRowOrNull
    private static TheoryDataRow<TTestData>? InstanceToTheoryDataRowOrNull<TTestData>(
        TTestData testData,
        ArgsCode argsCode,
        string? testMethodName,
        out string? testDisplayName)
    where TTestData : notnull, ITestData
    {
        var testCaseName = (testData
            ?? throw new ArgumentNullException(nameof(testData)))
            .GetTestCaseName();

        testDisplayName = GetDisplayName(
            testMethodName,
            testCaseName);

        return argsCode switch
        {
            ArgsCode.Instance =>
            new TheoryDataRow<TTestData>(testData)
            {
                TestDisplayName = testDisplayName,
            },
            ArgsCode.Properties =>
            null,
            _ =>
            throw argsCode.GetInvalidEnumArgumentException(nameof(argsCode)),
        };
    }
    #endregion
}
