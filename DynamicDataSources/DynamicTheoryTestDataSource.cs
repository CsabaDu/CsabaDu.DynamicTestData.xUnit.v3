// SPDX-License-Identifier: MIT
// Copyright (c) 2025. Csaba Dudas (CsabaDu)

using CsabaDu.DynamicTestData.xUnit.v3.TestDataHolders;
using CsabaDu.DynamicTestData.xUnit.v3.TheoryTestDataHolders;

namespace CsabaDu.DynamicTestData.xUnit.v3.DynamicDataSources;

public abstract class DynamicTheoryTestDataHolder(ArgsCode argsCode)
: DynamicNamedDataSource<ITheoryTestDataRow>(argsCode, typeof(IExpected))
{
    protected override void Add<TTestData>(TTestData testData)
    {
        var success = TryGetTestDataRow(
            testData,
            out ITestDataRow<TTestData, ITheoryTestDataRow>? testDataRow);

        if (success && DataRowHolder is TheoryTestData<TTestData> theoryTestData)
        {
            theoryTestData.Add(testDataRow!.TestData);
        }
    }

    protected override ITestDataRow<TTestData, ITheoryTestDataRow> CreateTestDataRow<TTestData>(TTestData testData)
    => new TheoryTestDataRow<TTestData>(
        testData,
        this);

    protected override void InitDataRowHolder<TTestData>(TTestData testData)
    => DataRowHolder = new TheoryTestData<TTestData>
        (testData,
        this);
}
