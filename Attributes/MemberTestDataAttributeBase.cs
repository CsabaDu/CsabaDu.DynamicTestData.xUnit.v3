// SPDX-License-Identifier: MIT
// Copyright (c) 2025. Csaba Dudas (CsabaDu)

namespace CsabaDu.DynamicTestData.xUnit.v3.Attributes;

/// <summary>
/// Provides a data source for a theory test, with the data coming from a member of the test class.
/// Extends <see cref="MemberDataAttributeBase"/> with additional functionality.
/// </summary>
public abstract class MemberTestDataAttributeBase
: MemberDataAttributeBase
{
    #region Constructors
    /// <summary>
    /// Initializes a new instance of the <see cref="MemberTestDataAttributeBase"/> class.
    /// </summary>
    /// <param name="memberName">The name of the public member that will provide the test data.</param>
    /// <param name="arguments">The arguments to be passed to the member (only supported for static members).</param>
    private protected MemberTestDataAttributeBase(
        string memberName,
        params object[] arguments)
    : base(memberName, arguments)
    => DisableDiscoveryEnumeration = true;
    #endregion

    #region Methodsű
    /// <inheritdoc/>
    public override async ValueTask<IReadOnlyCollection<ITheoryDataRow>> GetData(
        MethodInfo testMethod,
        DisposalTracker disposalTracker)
    {
        var testMethodName = testMethod.Name;
        var theoryDataRowCollection =
            await base.GetData(testMethod, disposalTracker)
            .ConfigureAwait(false);

        if (testMethodName is null || theoryDataRowCollection is null)
        {
            return theoryDataRowCollection ?? [];
        }

        var runtimeGenericType = theoryDataRowCollection
            .GetType()
            .GetGenericArguments()[0];

        if (!runtimeGenericType.IsAssignableTo(typeof(ITheoryTestDataRow)))
        {
            return theoryDataRowCollection;
        }

        List<ITheoryTestDataRow> ttdrList = new(theoryDataRowCollection.Count);

        foreach (var item in theoryDataRowCollection)
        {
            var ttdr = item as ITheoryTestDataRow;

            ttdrList.Add(ttdr!.TestDisplayName is null ?
                ttdr.Convert(
                    ttdr.GetDataStrategy(),
                    testMethodName)
                : ttdr);
        }

        return ttdrList.CastOrToReadOnlyCollection();
    }

    /// <inheritdoc/>
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
            ArgsCode.Instance);
    }
    #endregion
}
