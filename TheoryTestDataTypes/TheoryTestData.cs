// SPDX-License-Identifier: MIT
// Copyright (c) 2025. Csaba Dudas (CsabaDu)

namespace CsabaDu.DynamicTestData.xUnit.v3.TheoryTestDataTypes;

/// <summary>
/// Represents a strongly-typed container for test data used in theory-based unit tests.
/// </summary>
/// <remarks>This class is designed to encapsulate test data of a specific type and associate it with an <see
/// cref="ArgsCode"/>. It is typically used in scenarios where parameterized unit tests require structured test
/// data.</remarks>
/// <typeparam name="TTestData">The type of test data contained in this instance. Must implement <see cref="ITestData"/>.</typeparam>
public sealed class TheoryTestData<TTestData>(
    TTestData testData,
    IDataStrategy dataStrategy)
: DataRowHolder<TTestData, object?[]>(
    testData,
    dataStrategy),
ITheoryTestData
where TTestData : notnull, ITestData
{
    public override bool? WithExpected { get; } =
        DataStrategy<TTestData>.GetWithExpected(typeof(IExpected));

    public override ITestDataRow<TTestData, object?[]> CreateTestDataRow(
        TTestData testData,
        IDataStrategy dataStrategy)
    => new TheoryTestDataRow<TTestData>(
        testData,
        dataStrategy);
}
