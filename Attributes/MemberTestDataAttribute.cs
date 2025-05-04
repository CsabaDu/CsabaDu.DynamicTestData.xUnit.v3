// SPDX-License-Identifier: MIT
// Copyright (c) 2025. Csaba Dudas (CsabaDu)

namespace CsabaDu.DynamicTestData.xUnit.v3.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public sealed class MemberTestDataAttribute : MemberDataAttributeBase
    {
        public MemberTestDataAttribute(string memberName, params object[] arguments)
        : base(memberName, arguments)
        {
            DisableDiscoveryEnumeration = true;
        }

        /// <inheritdoc/>
        public override async ValueTask<IReadOnlyCollection<ITheoryDataRow>> GetData(
            MethodInfo testMethod,
            DisposalTracker disposalTracker)
        {
            var dataCollection = await base.GetData(testMethod, disposalTracker);

            if (dataCollection is IEnumerable<ITheoryTestDataRow> testDataCollection)
            {
                dataCollection =
                    testDataCollection
                    .Select(setTestDisplayNameIfArgsCodeProperties)
                    .CastOrToReadOnlyCollection();
            }

            return await new ValueTask<IReadOnlyCollection<ITheoryDataRow>>(dataCollection);

            #region Local methods
            ITheoryDataRow setTestDisplayNameIfArgsCodeProperties(ITheoryTestDataRow testDataRow)
            => testDataRow.ArgsCode == ArgsCode.Properties ?
                    testDataRow.SetTestDisplayName(testMethod.Name)
                    : testDataRow;
            #endregion
        }

        //[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
        //public sealed class MemberTestDataAttribute : MemberDataAttributeBase
        //{
        //    #region Modified constructor
        //    // Modified to set 'DisableDiscoveryEnumeration' to true.
        //    public MemberTestDataAttribute(string memberName, params object[] arguments)
        //    : base(memberName, arguments)
        //    => DisableDiscoveryEnumeration = true;
        //    #endregion Modified constructor

        //    #region Reused MemberDataAttributeBase codes
        //    #region Reused with changes
        //    // Modified to use SetTestDisplayName
        //    /// <inheritdoc/>
        //    public override ValueTask<IReadOnlyCollection<ITheoryDataRow>> GetData(
        //        MethodInfo testMethod,
        //        DisposalTracker disposalTracker)
        //    {
        //        MemberType ??= testMethod.DeclaringType
        //            ?? throw new InvalidOperationException("Test method declaring type is null.");

        //        var accessor =
        //            GetPropertyAccessor(MemberType)
        //                ?? GetFieldAccessor(MemberType)
        //                ?? GetMethodAccessor(MemberType)
        //                ?? throw new ArgumentException(
        //                    string.Format(
        //                        CultureInfo.CurrentCulture,
        //                        "Could not find public static member (property, field, or method) named '{0}' on '{1}'{2}",
        //                        MemberName,
        //                        MemberType.SafeName(),
        //                        Arguments.Length > 0 ? string.Format(CultureInfo.CurrentCulture, " with parameter types: {0}", string.Join(", ", Arguments.Select(p => p?.GetType().SafeName() ?? "(null)"))) : ""
        //                    )
        //                );

        //        var returnValue =
        //            accessor()
        //                ?? throw new ArgumentException(
        //                    string.Format(
        //                        CultureInfo.CurrentCulture,
        //                        "Member '{0}' on '{1}' returned null when queried for test data",
        //                        MemberName,
        //                        MemberType.SafeName()
        //                    )
        //                );

        //        if (returnValue is IEnumerable dataItems)
        //        {
        //            var result = new List<ITheoryDataRow>();

        //            foreach (var dataItem in dataItems)
        //                // Modified to use SetTestDisplayName
        //                AddToResult(dataItem, testMethod, ref result);

        //            return new(result.CastOrToReadOnlyCollection());
        //        }

        //        return GetDataAsync(returnValue, MemberType, testMethod);
        //    }

        //    // Modified to use SetTestDisplayName
        //    async ValueTask<IReadOnlyCollection<ITheoryDataRow>> GetDataAsync(
        //        object? returnValue,
        //        Type type,
        //        MethodInfo testMethod)
        //    {
        //        var taskAwaitable = returnValue.AsValueTask();
        //        if (taskAwaitable.HasValue)
        //            returnValue = await taskAwaitable.Value;

        //        if (returnValue is IAsyncEnumerable<object?> asyncDataItems)
        //        {
        //            var result = new List<ITheoryDataRow>();

        //            await foreach (var dataItem in asyncDataItems)
        //                // Modified to use SetTestDisplayName
        //                AddToResult(dataItem, testMethod, ref result);

        //            return result.CastOrToReadOnlyCollection();
        //        }

        //        if (returnValue is IEnumerable dataItems)
        //        {
        //            var result = new List<ITheoryDataRow>();

        //            foreach (var dataItem in dataItems)
        //                // Modified to use SetTestDisplayName
        //                AddToResult(dataItem, testMethod, ref result);

        //            return result.CastOrToReadOnlyCollection();
        //        }

        //        throw new ArgumentException(
        //            string.Format(
        //                CultureInfo.CurrentCulture,
        //                "Member '{0}' on '{1}' must return data in one of the following formats:{2}{3}",
        //                MemberName,
        //                type.SafeName(),
        //                Environment.NewLine,
        //                // Modified to access the private static field 'supportedDataSignatures' of the base class directly.
        //                GetSupportedDataSignatures().Value
        //            )
        //        );
        //    }
        //    #endregion Reused with changes

        //    #region Copied without changes
        //    // Modified GetData and GetDataAsync methods use these methods to access the member data
        //    Func<object?>? GetFieldAccessor(Type? type)
        //    {
        //        FieldInfo? fieldInfo = null;
        //        foreach (var reflectionType in GetTypesForMemberResolution(type, includeInterfaces: false))
        //        {
        //            fieldInfo = reflectionType.GetRuntimeField(MemberName);
        //            if (fieldInfo is not null)
        //                break;
        //        }

        //        return
        //            fieldInfo is not null && fieldInfo.IsStatic
        //                ? (() => fieldInfo.GetValue(null))
        //                : null;
        //    }

        //    Func<object?>? GetMethodAccessor(Type? type)
        //    {
        //        MethodInfo? methodInfo = null;
        //        var argumentTypes = Arguments is null ? [] : Arguments.Select(p => p?.GetType()).ToArray();
        //        foreach (var reflectionType in GetTypesForMemberResolution(type, includeInterfaces: true))
        //        {
        //            var methodInfoArray =
        //                reflectionType
        //                    .GetRuntimeMethods()
        //                    .Where(m => m.Name == MemberName && ParameterTypesCompatible(m.GetParameters(), argumentTypes))
        //                    .ToArray();
        //            if (methodInfoArray.Length == 0)
        //                continue;
        //            if (methodInfoArray.Length == 1)
        //            {
        //                methodInfo = methodInfoArray[0];
        //                break;
        //            }
        //            methodInfo = methodInfoArray.Where(m => m.GetParameters().Length == argumentTypes.Length).FirstOrDefault();
        //            if (methodInfo is not null)
        //                break;

        //            throw new ArgumentException(
        //                string.Format(
        //                    CultureInfo.CurrentCulture,
        //                    "The call to method '{0}.{1}' is ambigous between {2} different options for the given arguments.",
        //                    type!.SafeName(),
        //                    MemberName,
        //                    methodInfoArray.Length
        //                ),
        //                nameof(type)
        //            );
        //        }

        //        if (methodInfo is null || !methodInfo.IsStatic)
        //            return null;

        //        var completedArguments = Arguments ?? [];
        //        var finalMethodParameters = methodInfo.GetParameters();

        //        completedArguments =
        //            completedArguments.Length == finalMethodParameters.Length
        //                ? completedArguments
        //                : completedArguments.Concat(finalMethodParameters.Skip(completedArguments.Length).Select(pi => pi.DefaultValue)).ToArray();

        //        return () => methodInfo.Invoke(null, completedArguments);
        //    }

        //    Func<object?>? GetPropertyAccessor(Type? type)
        //    {
        //        PropertyInfo? propInfo = null;
        //        foreach (var reflectionType in GetTypesForMemberResolution(type, includeInterfaces: true))
        //        {
        //            propInfo = reflectionType.GetRuntimeProperty(MemberName);
        //            if (propInfo is not null)
        //                break;
        //        }

        //        return
        //            propInfo is not null && propInfo.GetMethod is not null && propInfo.GetMethod.IsStatic
        //                ? (() => propInfo.GetValue(null, null))
        //                : null;
        //    }

        //    static IEnumerable<Type> GetTypesForMemberResolution(
        //        Type? typeToInspect,
        //        bool includeInterfaces)
        //    {
        //        HashSet<Type> interfaces = [];

        //        for (var reflectionType = typeToInspect; reflectionType is not null; reflectionType = reflectionType.BaseType)
        //        {
        //            yield return reflectionType;

        //            if (includeInterfaces)
        //                foreach (var @interface in reflectionType.GetInterfaces())
        //                    interfaces.Add(@interface);
        //        }

        //        foreach (var @interface in interfaces)
        //            yield return @interface;
        //    }

        //    static bool ParameterTypesCompatible(
        //        ParameterInfo[] parameters,
        //        Type?[] argumentTypes)
        //    {
        //        if (parameters.Length < argumentTypes.Length)
        //            return false;

        //        var idx = 0;
        //        for (; idx < argumentTypes.Length; ++idx)
        //            if (argumentTypes[idx] is not null && !parameters[idx].ParameterType.IsAssignableFrom(argumentTypes[idx]!))
        //                return false;

        //        for (; idx < parameters.Length; ++idx)
        //            if (!parameters[idx].IsOptional)
        //                return false;

        //        return true;
        //    }
        //    #endregion Copied without changes
        //    #endregion Reused MemberDataAttributeBase codes

        //    #region New methods
        //    // New method to set the test display name in certain cases
        //    private void AddToResult(object? dataItem, MethodInfo testMethod, ref List<ITheoryDataRow> result)
        //    {
        //        if (dataItem is ITheoryTestDataRow testDataRow
        //            && testDataRow.ArgsCode == ArgsCode.Properties)
        //        {
        //            dataItem = testDataRow.SetTestDisplayName(testMethod.Name);
        //        }

        //        if (dataItem is not null)
        //        {
        //            result.Add(ConvertDataRow(dataItem));
        //        }
        //    }

        //    // New method to access the private static field 'supportedDataSignatures' of the base class directly.
        //    // Needed just in edge case when ArgumentException is thrown.
        //    // This will be contained by the exception message.
        //    private static Lazy<string> GetSupportedDataSignatures()
        //    {
        //        const string supportedDataSignatures = "supportedDataSignatures";
        //        Type baseType = typeof(MemberDataAttributeBase);
        //        FieldInfo fieldInfo = baseType.GetField(
        //            supportedDataSignatures,
        //            BindingFlags.Static | BindingFlags.NonPublic)
        //            ?? throw new InvalidOperationException(
        //                $"'{supportedDataSignatures}' member of 'MemberDataAttributeBase' does not exist.");

        //        return fieldInfo.GetValue(null) as Lazy<string>
        //            ?? throw new InvalidOperationException(
        //                $"'{supportedDataSignatures}' is not of 'Lazy<string>' type.");
        //    }
        //    #endregion New methods
        //}
    }
}
