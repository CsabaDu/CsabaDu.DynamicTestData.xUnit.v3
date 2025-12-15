// SPDX-License-Identifier: MIT
// Copyright (c) 2025. Csaba Dudas (CsabaDu)

using CsabaDu.DynamicTestData.xUnit.v3.TestDataRows.Converters;
using static CsabaDu.DynamicTestData.xUnit.v3.TestDataRows.Converters.TestDataConverter;

namespace CsabaDu.DynamicTestData.xUnit.v3.TestDataRows.Converters;

public static class CollectionConverter
{
    public static IEnumerable<TheoryTestDataRow<TTestData>> ToToTheoryTestDataRowCollection<TTestData>(
        this IEnumerable<TTestData> testDataCollection,
        ArgsCode argsCode,
        string? testMethodName = null)
    where TTestData : notnull, ITestData
    {
        return testDataCollection.Convert(
            TestDataConverter.ToTheoryTestDataRow,
            nameof(TestDataConverter.ToTheoryTestDataRow),
            argsCode,
            testMethodName);
    }
}