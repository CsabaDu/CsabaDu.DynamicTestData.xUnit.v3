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
public interface ITheoryTestDataRow : ITheoryDataRow
{
    /// <summary>
    /// Gets the test data instance for this test row.
    /// </summary>
    /// <value>The <see cref="ITestData"/> instance (never null).</value>
    [NotNull]
    ITestData TestData { get; }

    /// <summary>
    /// Gets the code specifying how the test data should be converted to arguments.
    /// </summary>
    /// <value>An <see cref="DynamicTestData.DynamicDataSources.ArgsCode"/> value indicating the argument conversion method.</value>
    ArgsCode ArgsCode { get; }

    /// <summary>
    /// Sets the display name for the test row based on the test method name.
    /// </summary>
    /// <param name="testMethodName">The name of the test method (optional). If null, the default display name will be used.</param>
    /// <returns>The configured test data row instance.</returns>
    ITheoryTestDataRow SetTestDisplayName(string? testMethodName);
}