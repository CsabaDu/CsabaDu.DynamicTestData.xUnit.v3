// SPDX-License-Identifier: MIT
// Copyright (c) 2025. Csaba Dudas (CsabaDu)

namespace CsabaDu.DynamicTestData.xUnit.v3.TheoryTestDataTypes;

/// <summary>
/// Represents a collection of theory test data with configurable argument conversion strategy.
/// </summary>
/// <remarks>
/// <para>
/// This sealed class provides a strongly-typed container for theory test data rows,
/// inheriting from <see cref="TheoryDataBase{ITheoryTestDataRow, ITestData}"/> and implementing
/// <see cref="ITheoryTestData"/>.
/// </para>
/// </remarks>
/// <param name="argsCode">The strategy for converting test data to method arguments.</param>
public class TheoryTestData(ArgsCode argsCode)
: TheoryDataBase<ITheoryTestDataRow, ITestData>
{
    /// <inheritdoc cref="ITheoryTestData.ArgsCode"/>
    /// <remarks>The value is validated to be a defined enum value.</remarks>
    public ArgsCode ArgsCode { get; init; } = argsCode.Defined(nameof(argsCode));

    /// <inheritdoc cref="ITheoryTestData.TestDataType"/>
    public Type? TestDataType { get; protected set; }

    /// <summary>
    /// Determines whether the specified <see cref="Type"/> is equal to the current test data type.
    /// </summary>
    /// <param name="testDataType">The <see cref="Type"/> to compare with the current test data type.</param>
    /// <returns><see langword="true"/> if the specified <see cref="Type"/> is not <see langword="null"/>  and is equal to the
    /// current test data type; otherwise, <see langword="false"/>.</returns>
    public bool Equals(Type? testDataType)
    => testDataType is not null
        && testDataType == TestDataType;

    /// <summary>
    /// Converts test data into a theory test data row.
    /// </summary>
    /// <param name="testData">The test data to convert.</param>
    /// <returns>
    /// A new <see cref="TheoryTestDataRow"/> instance configured with the test data,
    /// and argument conversion strategy.
    /// </returns>
    /// <exception cref="ArgumentException">Thrown if the type of <paramref name="testData"/>
    /// does not match with the other elements of the collection. </exception>
    protected override ITheoryTestDataRow Convert(ITestData testData)
    {
        return Equals(testData.GetType()) ?
            new TheoryTestDataRow(testData, ArgsCode)
            : throw new ArgumentException("Type does not match.", nameof(testData));
    }
}

/// <summary>
/// Represents a strongly-typed container for test data used in theory-based unit tests.
/// </summary>
/// <remarks>This class is designed to encapsulate test data of a specific type and associate it with an <see
/// cref="ArgsCode"/>. It is typically used in scenarios where parameterized unit tests require structured test
/// data.</remarks>
/// <typeparam name="T">The type of test data contained in this instance. Must implement <see cref="ITestData"/>.</typeparam>
public sealed class TheoryTestData<T>
: TheoryTestData
where T : ITestData
{
    /// <summary>
    /// Represents test data for a theory test, including its type and associated arguments.
    /// </summary>
    /// <remarks>This constructor initializes the test data and its type, and adds the provided test data to
    /// the collection. The type of the test data is determined using the generic type parameter.</remarks>
    /// <param name="argsCode">The arguments code associated with the test data.</param>
    /// <param name="TestData">The test data to be added.</param>
    internal TheoryTestData(ArgsCode argsCode, T TestData)
    : base(argsCode)
    {
        TestDataType = typeof(T);
        Add(TestData);
    }
}