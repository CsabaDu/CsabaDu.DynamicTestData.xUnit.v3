// SPDX-License-Identifier: MIT
// Copyright (c) 2025. Csaba Dudas (CsabaDu)

namespace CsabaDu.DynamicTestData.xUnit.v3.TheoryTestDataTypes.Interfaces;

/// <summary>
/// Represents a container for theory test data with initialization capabilities.
/// </summary>
/// <remarks>
/// This interface provides access to the argument conversion strategy (<see cref="DynamicTestData.DynamicDataSources.ArgsCode"/>)
/// and allows initialization with a test method name.
/// </remarks>
public interface ITheoryTestData
{
    /// <summary>
    /// Gets the strategy for converting test data to method arguments.
    /// </summary>
    /// <value>
    /// An <see cref="DynamicTestData.DynamicDataSources.ArgsCode"/> enum value that determines how test data should be
    /// converted to test method arguments.
    /// </value>
    ArgsCode ArgsCode { get; }
}