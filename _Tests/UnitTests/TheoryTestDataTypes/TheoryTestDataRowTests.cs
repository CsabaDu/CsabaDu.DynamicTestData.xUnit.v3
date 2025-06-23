// SPDX-License-Identifier: MIT
// Copyright (c) 2025. Csaba Dudas (CsabaDu)

using CsabaDu.DynamicTestData.TestDataTypes;
using CsabaDu.DynamicTestData.TestHelpers.TestDoubles.TestDataTypes;
using CsabaDu.DynamicTestData.xUnit.v3.TheoryTestDataHolders;

namespace CsabaDu.DynamicTestData.xUnit.v3.Tests.UnitTests.TheoryTestDataTypes;

[TestClass]
public sealed class TheoryTestDataRowTests
{
    private readonly ITestData testData = new TestDataChild(ActualDefinition, ActualExitMode, ActualResult);
    private TheoryTestDataRow _sut;

    private static DynamicArgsCodeDataSource DataSource = new();
    private const string DisplayName = nameof(GetDisplayName);

    private static IEnumerable<object[]> ArgsCodeDataSource
    => DataSource.ArgsCodeDataSource();

    public static string GetDisplayName(MethodInfo testMethod, object[] args)
    => DynamicDataSource.GetDisplayName(testMethod.Name, args);

    #region Constructors tests
    [TestMethod, DynamicData(nameof(ArgsCodeDataSource), DynamicDataDisplayName = DisplayName)]
    public void TheoryTestDataRow_validArgs_creates(ArgsCode argsCode)
    {
        // Arrange
        // Act
        _sut = new TheoryTestDataRow(testData, argsCode);

        // Assert
        Assert.IsNotNull(_sut);
    }

    [TestMethod]
    public void TheoryTestDataRow_nullTestData_throwsArgumentNullException()
    {
        // Arrange
        // Act
        void attempt() => _ = new TheoryTestDataRow(null, ArgsCode.Instance);

        // Assert
        var exception = Assert.ThrowsException<ArgumentNullException>(attempt);
        Assert.AreEqual("TestData", exception.ParamName);
    }

    [TestMethod]
    public void TheoryTestDataRow_invalidArgsCode_throwsInvalidOperationException()
    {
        // Arrange
        // Act
        void attempt() => _ = new TheoryTestDataRow(testData, InvalidArgsCode);
        // Assert
        var exception = Assert.ThrowsException<InvalidEnumArgumentException>(attempt);
        Assert.AreEqual("ArgsCode", exception.ParamName);
    }
    #endregion


}
