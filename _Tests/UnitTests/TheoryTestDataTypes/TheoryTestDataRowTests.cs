// SPDX-License-Identifier: MIT
// Copyright (c) 2025. Csaba Dudas (CsabaDu)

using CsabaDu.DynamicTestData.TestDataTypes.Interfaces;
using CsabaDu.DynamicTestData.TestHelpers.TestDoubles;
using CsabaDu.DynamicTestData.xUnit.v3.TheoryTestDataTypes;
using Xunit;

namespace CsabaDu.DynamicTestData.xUnit.v3.Tests.UnitTests.TheoryTestDataTypes;

public sealed class TheoryTestDataRowTests
{
    private readonly ITestData testData = new TestDataChild(ActualDefinition, ActualExitMode, ActualResult);
    private TheoryTestDataRow _sut;

    [Fact]
    public void TheoryTestDataRow_Constructor_ValidInput()
    {
        // Arrange
        // Act
        _sut = new TheoryTestDataRow(testData, ArgsCode.Properties);
        // Assert

    }
}
