// SPDX-License-Identifier: MIT
// Copyright (c) 2025. Csaba Dudas (CsabaDu)

namespace CsabaDu.DynamicTestData.xUnit.v3.TestDataRows.Interfaces;

/// <summary>
/// Represents a row of test data for xUnit.net theory tests with strongly-typed test data.
/// </summary>
/// <remarks>
/// Extends <see cref="ITheoryDataRow"/> to provide access to the <see cref="TestData"/> instance
/// and specifies how the test data should be converted to arguments via <see cref="DynamicTestData.DynamicDataSources.ArgsCode"/>.
/// </remarks>
public interface ITheoryTestDataRow
: ITheoryDataRow,
INamedTestDataRow<ITheoryTestDataRow>
{
    IDataStrategy GetDataStrategy();
}
