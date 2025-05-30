// SPDX-License-Identifier: MIT
// Copyright (c) 2025. Csaba Dudas (CsabaDu)

namespace CsabaDu.DynamicTestData.xUnit.v3.TheoryTestDataTypes.Interfaces;

/// <summary>
/// Represents a row of test data for xUnit.net theory tests with strongly-typed test data.
/// </summary>
/// <remarks>
/// Extends <see cref="ITheoryDataRow"/> to provide access to the <see cref="TestDataTypes.TestData"/> instance
/// and specifies how the test data should be converted to arguments via <see cref="DynamicTestData.DynamicDataSources.ArgsCode"/>.
/// </remarks>
public interface ITheoryTestDataRow
: ITheoryDataRow,
ITestCaseName
{
    /// <summary>
    /// Gets an array of objects representing the data associated with the current context.
    /// </summary>
    object?[] Data { get; }

    /// <summary>
    /// Gets the <see cref="CsabaDu.DynamicTestData.DynamicDataSources.ArgsCode"/> enum value
    /// which determines the way of the <see cref="ITestData"/> istance conversion to test parameters.
    /// </summary>
    ArgsCode ArgsCode { get; }

    /// <summary>
    /// Sets the display name for the test row based on the test method name.
    /// </summary>
    /// <param name="testMethodName">The name of the test method (optional). If null, the default display name will be used.</param>
    /// <returns>The configured test data row instance.</returns>
    ITheoryTestDataRow SetName(string? testMethodName);
}