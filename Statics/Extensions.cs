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
        var testDisplayName = CreateDisplayName(
            testData,
            testMethodName);

        return argsCode switch
        {
            ArgsCode.Instance =>
            new TheoryDataRow<ITestData<TResult, T1?>>(testData)
            {
                TestDisplayName = testDisplayName,
            },
            ArgsCode.Properties =>
            testData is IExpected ?
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
                },
            _ =>
            throw argsCode.GetInvalidEnumArgumentException(nameof(argsCode)),
        };
    }

    public static ITheoryDataRow ToTheoryDataRow<TResult, T1, T2>(
        this ITestData<TResult, T1, T2> testData,
        ArgsCode argsCode,
        string? testMethodName)
    where TResult : notnull
    {
        var testDisplayName = CreateDisplayName(
            testData,
            testMethodName);

        return argsCode switch
        {
            ArgsCode.Instance =>
            new TheoryDataRow<ITestData<TResult, T1?, T2?>>(testData)
            {
                TestDisplayName = testDisplayName,
            },
            ArgsCode.Properties =>
            testData is IExpected ?
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
                },
            _ =>
            throw argsCode.GetInvalidEnumArgumentException(nameof(argsCode)),
        };
    }

    public static ITheoryDataRow ToTheoryDataRow<TResult, T1, T2, T3>(
        this ITestData<TResult, T1, T2, T3> testData,
        ArgsCode argsCode,
        string? testMethodName)
    where TResult : notnull
    {
        var testDisplayName = CreateDisplayName(
            testData,
            testMethodName);

        return argsCode switch
        {
            ArgsCode.Instance =>
            new TheoryDataRow<ITestData<TResult, T1?, T2?, T3?>>(testData)
            {
                TestDisplayName = testDisplayName,
            },
            ArgsCode.Properties =>
            testData is IExpected ?
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
                },
            _ =>
            throw argsCode.GetInvalidEnumArgumentException(nameof(argsCode)),
        };
    }

    public static ITheoryDataRow ToTheoryDataRow<TResult, T1, T2, T3, T4>(
        this ITestData<TResult, T1, T2, T3, T4> testData,
        ArgsCode argsCode,
        string? testMethodName)
    where TResult : notnull
    {
        var testDisplayName = CreateDisplayName(
            testData,
            testMethodName);

        return argsCode switch
        {
            ArgsCode.Instance =>
            new TheoryDataRow<ITestData<TResult, T1?, T2?, T3?, T4?>>(testData)
            {
                TestDisplayName = testDisplayName,
            },
            ArgsCode.Properties =>
            testData is IExpected ?
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
                },
            _ =>
            throw argsCode.GetInvalidEnumArgumentException(nameof(argsCode)),
        };
    }

    public static ITheoryDataRow ToTheoryDataRow<TResult, T1, T2, T3, T4, T5>(
        this ITestData<TResult, T1, T2, T3, T4, T5> testData,
        ArgsCode argsCode,
        string? testMethodName)
    where TResult : notnull
    {
        var testDisplayName = CreateDisplayName(
            testData,
            testMethodName);

        return argsCode switch
        {
            ArgsCode.Instance =>
            new TheoryDataRow<ITestData<TResult, T1?, T2?, T3?, T4?, T5?>>(testData)
            {
                TestDisplayName = testDisplayName,
            },
            ArgsCode.Properties =>
            testData is IExpected ?
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
                },
            _ =>
            throw argsCode.GetInvalidEnumArgumentException(nameof(argsCode)),
        };
    }

    public static ITheoryDataRow ToTheoryDataRow<TResult, T1, T2, T3, T4, T5, T6>(
        this ITestData<TResult, T1, T2, T3, T4, T5, T6> testData,
        ArgsCode argsCode,
        string? testMethodName)
    where TResult : notnull
    {
        var testDisplayName = CreateDisplayName(
            testData,
            testMethodName);

        return argsCode switch
        {
            ArgsCode.Instance =>
            new TheoryDataRow<ITestData<TResult, T1?, T2?, T3?, T4?, T5?, T6?>>(testData)
            {
                TestDisplayName = testDisplayName,
            },
            ArgsCode.Properties =>
            testData is IExpected ?
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
                },
            _ =>
            throw argsCode.GetInvalidEnumArgumentException(nameof(argsCode)),
        };
    }

    public static ITheoryDataRow ToTheoryDataRow<TResult, T1, T2, T3, T4, T5, T6, T7>(
        this ITestData<TResult, T1, T2, T3, T4, T5, T6, T7> testData,
        ArgsCode argsCode,
        string? testMethodName)
    where TResult : notnull
    {
        var testDisplayName = CreateDisplayName(
            testData,
            testMethodName);

        return argsCode switch
        {
            ArgsCode.Instance =>
            new TheoryDataRow<ITestData<TResult, T1?, T2?, T3?, T4?, T5?, T6?, T7?>>(testData)
            {
                TestDisplayName = testDisplayName,
            },
            ArgsCode.Properties =>
            testData is IExpected ?
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
                },
            _ =>
            throw argsCode.GetInvalidEnumArgumentException(nameof(argsCode)),
        };
    }

    public static ITheoryDataRow ToTheoryDataRow<TResult, T1, T2, T3, T4, T5, T6, T7, T8>(
        this ITestData<TResult, T1, T2, T3, T4, T5, T6, T7, T8> testData,
        ArgsCode argsCode,
        string? testMethodName)
    where TResult : notnull
    {
        var testDisplayName = CreateDisplayName(
            testData,
            testMethodName);

        return argsCode switch
        {
            ArgsCode.Instance =>
            new TheoryDataRow<ITestData<TResult, T1?, T2?, T3?, T4?, T5?, T6?, T7?, T8?>>(testData)
            {
                TestDisplayName = testDisplayName,
            },
            ArgsCode.Properties =>
            testData is IExpected ?
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
                },
            _ =>
            throw argsCode.GetInvalidEnumArgumentException(nameof(argsCode)),
        };
    }

    public static ITheoryDataRow ToTheoryDataRow<TResult, T1, T2, T3, T4, T5, T6, T7, T8, T9>(
        this ITestData<TResult, T1, T2, T3, T4, T5, T6, T7, T8, T9> testData,
        ArgsCode argsCode,
        string? testMethodName)
    where TResult : notnull
    {
        var testDisplayName = CreateDisplayName(
            testData,
            testMethodName);

        return argsCode switch
        {
            ArgsCode.Instance =>
            new TheoryDataRow<ITestData<TResult, T1?, T2?, T3?, T4?, T5?, T6?, T7?, T8?, T9>>(testData)
            {
                TestDisplayName = testDisplayName,
            },
            ArgsCode.Properties =>
            testData is IExpected ?
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
                },
            _ =>
            throw argsCode.GetInvalidEnumArgumentException(nameof(argsCode)),
        };
    }
    #endregion

    #region Private CreateDisplayName

    private static string? CreateDisplayName(
        ITestData testData,
        string? testMethodName)
    {
        var testCaseName = (testData
            ?? throw new ArgumentNullException(nameof(testData)))
            .GetTestCaseName();

       return GetDisplayName(
            testMethodName,
            testCaseName);
    }
    #endregion
}
