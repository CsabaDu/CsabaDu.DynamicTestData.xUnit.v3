// SPDX-License-Identifier: MIT
// Copyright (c) 2025. Csaba Dudas (CsabaDu)

namespace CsabaDu.DynamicTestData.xUnit.v3.TestDataRows.Converters;

public static class CollectionConverter
{
    public static IEnumerable<TheoryTestDataRow<TTestData>> ToTheoryTestDataRowCollection<TTestData>(
        this IEnumerable<TTestData> testDataCollection,
        ArgsCode argsCode,
        string? testMethodName = null)
    where TTestData : notnull, ITestData
    => testDataCollection.Convert(
        TestDataConverter.ToTheoryTestDataRow,
        argsCode,
        testMethodName);
}