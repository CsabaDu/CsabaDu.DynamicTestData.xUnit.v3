// SPDX-License-Identifier: MIT
// Copyright (c) 2025. Csaba Dudas (CsabaDu)

namespace CsabaDu.DynamicTestData.xUnit.v3.TheoryTestDataTypes;

/// <summary>
/// Represents a collection of theory test data with configurable argument conversion strategy.
/// </summary>
/// <remarks>
/// <para>
/// This sealed class provides a strongly-typed container for theory test data rows,
/// inheriting from <see cref="TheoryDataBase{TTheoryDataRow, TTestData}"/> and implementing
/// <see cref="ITheoryTestData"/>.
/// </para>
/// </remarks>
/// <param name="argsCode">The strategy for converting test data to method arguments.</param>
public sealed class TheoryTestData(ArgsCode argsCode)
: TheoryDataBase<ITheoryTestDataRow, ITestData>, ITheoryTestData
{
    /// <summary>
    /// Gets the strategy for converting test data to method arguments.
    /// </summary>
    /// <value>
    /// An <see cref="DynamicTestData.DynamicDataSources.ArgsCode"/> value that determines how test data should be
    /// converted to test method arguments. The value is validated to be a defined enum value.
    /// </value>
    public ArgsCode ArgsCode => argsCode.Defined(nameof(argsCode));

    /// <summary>
    /// Converts test data into a theory test data row.
    /// </summary>
    /// <param name="testData">The test data to convert.</param>
    /// <returns>
    /// A new <see cref="TheoryTestDataRow"/> instance configured with the test data,
    /// and argument conversion strategy.
    /// </returns>
    protected override ITheoryTestDataRow Convert(ITestData testData)
    => new TheoryTestDataRow(testData, ArgsCode);
}