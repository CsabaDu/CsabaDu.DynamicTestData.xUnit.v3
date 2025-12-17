// SPDX-License-Identifier: MIT
// Copyright (c) 2025. Csaba Dudas (CsabaDu)

namespace CsabaDu.DynamicTestData.xUnit.v3.TestDataRows.Converters;

public static class TestDataConverter
{
    #region ToTheoryTestDataRow
    public static TheoryTestDataRow<TTestData> ToTheoryTestDataRow<TTestData>(
        this TTestData testData,
        ArgsCode argsCode,
        string? testMethodName)
    where TTestData : notnull, ITestData
    => new(testData, argsCode, testMethodName);
    #endregion
}
