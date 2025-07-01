// SPDX-License-Identifier: MIT
// Copyright (c) 2025. Csaba Dudas (CsabaDu)

namespace CsabaDu.DynamicTestData.xUnit.v3.Attributes;

/// <summary>
/// Provides a data source for a theory test, with the data coming from a dataSource of the test class.
/// Extends <see cref="MemberDataAttributeBase"/> with additional functionality.
/// </summary>
public abstract class MemberTestDataAttributeBase
: MemberDataAttributeBase
{
    /// <summary>
    /// Initializes a new dataSource of the <see cref="MemberTestDataAttribute"/> class.
    /// <remarks>
    /// Constructor extension to set <see cref="MemberDataAttributeBase.DisableDiscoveryEnumeration"/> to true.
    /// </remarks>
    /// </summary>
    /// <param name="memberName">The name of the public dataSource that will provide the test data.</param>
    /// <param name="arguments">The arguments to be passed to the dataSource (only supported for static members).</param>
    private protected MemberTestDataAttributeBase(string memberName, params object[] arguments)
    : base(memberName, arguments)
    => DisableDiscoveryEnumeration = true;

    /// <summary>
    /// Retrieves the data to be used to test the theory.
    /// <remarks>
    /// For data rows implementing <see cref="ITheoryTestDataRow"/> where <see cref="ITheoryTestDataRow.ArgsCode"/>
    /// equals <see cref="ArgsCode.Properties"/>, this method sets the <see cref="ITheoryDataRow.TestDisplayName"/>
    /// using the test method name and the <see cref="ITheoryTestDataRow.TestData.TestCase"/> property.
    /// </remarks>
    /// </summary>
    /// <param name="testMethod">The test method decorated with the <see cref="TheoryAttribute"/>.</param>
    /// <param name="disposalTracker">The tracker for disposable objects created during test data generation.</param>
    /// <returns>A collection of test data rows to be used for the theory test.</returns>
    public override async ValueTask<IReadOnlyCollection<ITheoryDataRow>> GetData(
        MethodInfo testMethod,
        DisposalTracker disposalTracker)
    {
        var testMethodName = testMethod.Name;
        var theoryDataRowCollection =
            await base.GetData(testMethod, disposalTracker)
            .ConfigureAwait(false);

        if (testMethodName == null)
        {
            return theoryDataRowCollection;
        }

        var runtimeGenericType = theoryDataRowCollection
            !.GetType()
            .GetGenericArguments()[0];

        if (!typeof(ITheoryTestDataRow).IsAssignableFrom(runtimeGenericType))
        {
            return theoryDataRowCollection;
        }

        var ttdrList = new List<ITheoryTestDataRow>();

        foreach (var item in theoryDataRowCollection!)
        {
            var ttdr = item as ITheoryTestDataRow;

            if (ttdr!.TestDisplayName == null)
            {
                ttdr = ttdr.Convert(
                    ttdr.GetDataStrategy(),
                    testMethodName);
            }

            ttdrList.Add(ttdr);
        }

        return ttdrList.CastOrToReadOnlyCollection();
    }

    /// <summary>
    /// Returns <paramref name="dataRow"/> if it is an <see cref="ITheoryTestDataRow"/>,
    /// otherwise calls the base method.
    /// </summary>
    /// <inheritdoc cref="DataAttribute.ConvertDataRow(object)"/>
    protected override ITheoryDataRow ConvertDataRow(object dataRow)
    {
        if (dataRow is ITheoryTestDataRow theoryTestDataRow)
        {
            return theoryTestDataRow;
        }

        if (dataRow is not ITestData testData)
        {
            if (dataRow is not ITestDataRow testDataRow)
            {
                return base.ConvertDataRow(dataRow);
            }

            testData = testDataRow.GetTestData();
        }

        return new TheoryTestDataRow<ITestData>(
            testData,
            getArgsCodeFromDataSourceMember());

        #region Local methods
        ArgsCode getArgsCodeFromDataSourceMember()
        {
            try
            {
                Type declaringType = MemberType
                    ?? throw new InvalidOperationException(
                        "Data source member declaring type is null");

                object dataSource = getDataSourceMemberValue(
                    declaringType,
                    BindingFlags.Static |
                    BindingFlags.Public |
                    BindingFlags.NonPublic);

                return dataSource is IArgsCode dataStrategyBase ?
                    dataStrategyBase.ArgsCode
                    : ArgsCode.Instance;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    "Failed to retrieve 'ITestData' type data rows from " +
                    $"{MemberType?.Name ?? "(unknown type)"}.{MemberName}",
                    ex is TargetInvocationException tiex ?
                    tiex.InnerException
                    : ex);
            }
        }

        object getDataSourceMemberValue(
            Type declaringType,
            BindingFlags flags)
        {
            Guard.ArgumentNotNull(declaringType, nameof(declaringType));

            // Property
            if (declaringType.GetProperty(MemberName, flags) is { } property
                && property.GetValue(null) is object propertyValue)
            {
                return propertyValue;
            }

            // Method
            if (declaringType.GetMethod(MemberName, flags,
                null, EmptyTypeArray, null) is { } method
                && method.Invoke(null, null) is object methodValue)
            {
                return methodValue;
            }

            // Field
            if (declaringType.GetField(MemberName, flags) is { } field
                && field.GetValue(null) is object fieldValue)
            {
                return fieldValue;
            }

            throw new InvalidOperationException(
                "Static data source member " +
                $"'{MemberName}' not found in " +
                $"{declaringType.Name}");
        }
        #endregion
    }

    private static readonly Type[] EmptyTypeArray = Type.EmptyTypes;
}
