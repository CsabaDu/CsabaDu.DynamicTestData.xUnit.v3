/*
 * MIT License
 * 
 * Copyright (c) 2025. Csaba Dudas (CsabaDu)
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */
namespace CsabaDu.DynamicTestData.xUnit.v3.Attributes;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public sealed class MemberTestDataAttribute(string memberName, params object[] arguments)
: DataAttribute
{
    static readonly Lazy<string> supportedDataSignatures;

    static MemberTestDataAttribute() =>
        supportedDataSignatures = new(() =>
        {
            var dataSignatures = new List<string>(18);

            foreach (var enumerable in new[] { "IEnumerable<{0}>", "IAsyncEnumerable<{0}>" })
                foreach (var dataType in new[] { "ITheoryDataRow", "object[]", "Tuple<...>" })
                    foreach (var wrapper in new[] { "- {0}", "- Task<{0}>", "- ValueTask<{0}>" })
                        dataSignatures.Add(string.Format(CultureInfo.CurrentCulture, wrapper, string.Format(CultureInfo.CurrentCulture, enumerable, dataType)));

            return string.Join(Environment.NewLine, dataSignatures);
        });

    /// <summary>
    /// Gets or sets the arguments passed to the member. Only supported for static methods.
    /// </summary>
    public object?[] Arguments { get; } = Guard.ArgumentNotNull(arguments);

    /// <summary>
    /// Returns <c>true</c> if the data attribute wants to skip enumerating data during discovery.
    /// This will cause the theory to yield a single test case for all data, and the data discovery
    /// will be during test execution instead of discovery.
    /// <remark>Default value is <c>true</c> because <see cref="TheoryTestDataRow"/> is a record class so it
    /// cannot implement <see cref="IXunitSerializable"/>, neither an <see cref="IXunitSerializer"/>
    /// can be implemented to support its serialization, because it does not have a parameterless constructor.
    /// Set <see cref="DisableDiscoveryEnumeration"/> to <c>false</c> if use serializable type
    /// and you want to enumerate the data during discovery.
    /// </remark>
    /// </summary>
    public bool DisableDiscoveryEnumeration { get; set; } = true;

    /// <summary>
    /// Gets the member name.
    /// </summary>
    public string MemberName { get; } = Guard.ArgumentNotNull(memberName);

    /// <summary>
    /// Gets or sets the type to retrieve the member from. If not set, then the member will be
    /// retrieved from the unit test class.
    /// </summary>
    public Type? MemberType { get; set; }

    /// <inheritdoc/>
    protected override ITheoryDataRow ConvertDataRow(object dataRow)
    {
        if (dataRow is TheoryTestDataRow theoryTestDataRow)
        {
            return theoryTestDataRow;
        }

        Guard.ArgumentNotNull(dataRow);
        
        try
        {
            return base.ConvertDataRow(dataRow);
        }
        catch (ArgumentException)
        {
            throw new ArgumentException(
                string.Format(
                    CultureInfo.CurrentCulture,
                    "Member '{0}' on '{1}' yielded an item of type '{2}' which is not an 'object?[]', 'Xunit.ITheoryDataRow' or 'System.Runtime.CompilerServices.ITuple'",
                    MemberName,
                    MemberType?.SafeName(),
                    dataRow.GetType().SafeName()
                ),
                nameof(dataRow)
            );
        }
    }

    /// <inheritdoc/>
    public override ValueTask<IReadOnlyCollection<ITheoryDataRow>> GetData(
        MethodInfo testMethod,
        DisposalTracker disposalTracker)
    {
        MemberType ??= testMethod.DeclaringType
            ?? throw new InvalidOperationException("Test method declaring type is null.");

        var accessor =
            GetPropertyAccessor(MemberType)
                ?? GetFieldAccessor(MemberType)
                ?? GetMethodAccessor(MemberType)
                ?? throw new ArgumentException(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        "Could not find public static member (property, field, or method) named '{0}' on '{1}'{2}",
                        MemberName,
                        MemberType.SafeName(),
                        Arguments.Length > 0 ? string.Format(CultureInfo.CurrentCulture, " with parameter types: {0}", string.Join(", ", Arguments.Select(p => p?.GetType().SafeName() ?? "(null)"))) : ""
                    )
                );

        var returnValue =
            accessor()
                ?? throw new ArgumentException(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        "Member '{0}' on '{1}' returned null when queried for test data",
                        MemberName,
                        MemberType.SafeName()
                    )
                );

        if (returnValue is IEnumerable dataItems)
        {
            var result = new List<ITheoryDataRow>();

            foreach (var dataItem in dataItems)
                AddToResult(dataItem, testMethod, ref result);

            return new(result.CastOrToReadOnlyCollection());
        }

        return GetDataAsync(returnValue, MemberType, testMethod);
    }

    async ValueTask<IReadOnlyCollection<ITheoryDataRow>> GetDataAsync(
        object? returnValue,
        Type type,
        MethodInfo testMethod)
    {
        var taskAwaitable = returnValue.AsValueTask();
        if (taskAwaitable.HasValue)
            returnValue = await taskAwaitable.Value;

        if (returnValue is IAsyncEnumerable<object?> asyncDataItems)
        {
            var result = new List<ITheoryDataRow>();

            await foreach (var dataItem in asyncDataItems)
                AddToResult(dataItem, testMethod, ref result);

            return result.CastOrToReadOnlyCollection();
        }

        // Duplicate from GetData(), but it's hard to avoid since we need to support Task/ValueTask
        // of IEnumerable (and not just IAsyncEnumerable).
        if (returnValue is IEnumerable dataItems)
        {
            var result = new List<ITheoryDataRow>();

            foreach (var dataItem in dataItems)
                AddToResult(dataItem, testMethod, ref result);

            return result.CastOrToReadOnlyCollection();
        }

        throw new ArgumentException(
            string.Format(
                CultureInfo.CurrentCulture,
                "Member '{0}' on '{1}' must return data in one of the following formats:{2}{3}",
                MemberName,
                type.SafeName(),
                Environment.NewLine,
                supportedDataSignatures.Value
            )
        );
    }

    private void AddToResult(object? dataItem, MethodInfo testMethod, ref List<ITheoryDataRow> result)
    {
        if (dataItem is ITheoryTestDataRow testDataRow)
        {
            if (testDataRow.ArgsCode == ArgsCode.Properties)
            {
                testDataRow = testDataRow.SetTestDisplayName(testMethod.Name);
            }

            AddConverted(testDataRow, ref result);
        }
        else if (dataItem is not null)
        {
            AddConverted(dataItem, ref result);
        }

    }

    void AddConverted(object dataRow, ref List<ITheoryDataRow> result)
    => result.Add(ConvertDataRow(dataRow));

    Func<object?>? GetFieldAccessor(Type? type)
    {
        FieldInfo? fieldInfo = null;
        foreach (var reflectionType in GetTypesForMemberResolution(type, includeInterfaces: false))
        {
            fieldInfo = reflectionType.GetRuntimeField(MemberName);
            if (fieldInfo is not null)
                break;
        }

        return
            fieldInfo is not null && fieldInfo.IsStatic
                ? (() => fieldInfo.GetValue(null))
                : null;
    }

    Func<object?>? GetMethodAccessor(Type? type)
    {
        MethodInfo? methodInfo = null;
        var argumentTypes = Arguments is null ? [] : Arguments.Select(p => p?.GetType()).ToArray();
        foreach (var reflectionType in GetTypesForMemberResolution(type, includeInterfaces: true))
        {
            var methodInfoArray =
                reflectionType
                    .GetRuntimeMethods()
                    .Where(m => m.Name == MemberName && ParameterTypesCompatible(m.GetParameters(), argumentTypes))
                    .ToArray();
            if (methodInfoArray.Length == 0)
                continue;
            if (methodInfoArray.Length == 1)
            {
                methodInfo = methodInfoArray[0];
                break;
            }
            methodInfo = methodInfoArray.Where(m => m.GetParameters().Length == argumentTypes.Length).FirstOrDefault();
            if (methodInfo is not null)
                break;

            throw new ArgumentException(
                string.Format(
                    CultureInfo.CurrentCulture,
                    "The call to method '{0}.{1}' is ambigous between {2} different options for the given arguments.",
                    type!.SafeName(),
                    MemberName,
                    methodInfoArray.Length
                ),
                nameof(type)
            );
        }

        if (methodInfo is null || !methodInfo.IsStatic)
            return null;

        var completedArguments = Arguments ?? [];
        var finalMethodParameters = methodInfo.GetParameters();

        completedArguments =
            completedArguments.Length == finalMethodParameters.Length
                ? completedArguments
                : completedArguments.Concat(finalMethodParameters.Skip(completedArguments.Length).Select(pi => pi.DefaultValue)).ToArray();

        return () => methodInfo.Invoke(null, completedArguments);
    }

    Func<object?>? GetPropertyAccessor(Type? type)
    {
        PropertyInfo? propInfo = null;
        foreach (var reflectionType in GetTypesForMemberResolution(type, includeInterfaces: true))
        {
            propInfo = reflectionType.GetRuntimeProperty(MemberName);
            if (propInfo is not null)
                break;
        }

        return
            propInfo is not null && propInfo.GetMethod is not null && propInfo.GetMethod.IsStatic
                ? (() => propInfo.GetValue(null, null))
                : null;
    }

    static IEnumerable<Type> GetTypesForMemberResolution(
        Type? typeToInspect,
        bool includeInterfaces)
    {
        HashSet<Type> interfaces = [];

        for (var reflectionType = typeToInspect; reflectionType is not null; reflectionType = reflectionType.BaseType)
        {
            yield return reflectionType;

            if (includeInterfaces)
                foreach (var @interface in reflectionType.GetInterfaces())
                    interfaces.Add(@interface);
        }

        foreach (var @interface in interfaces)
            yield return @interface;
    }

    static bool ParameterTypesCompatible(
        ParameterInfo[] parameters,
        Type?[] argumentTypes)
    {
        if (parameters.Length < argumentTypes.Length)
            return false;

        var idx = 0;
        for (; idx < argumentTypes.Length; ++idx)
            if (argumentTypes[idx] is not null && !parameters[idx].ParameterType.IsAssignableFrom(argumentTypes[idx]!))
                return false;

        for (; idx < parameters.Length; ++idx)
            if (!parameters[idx].IsOptional)
                return false;

        return true;
    }

    /// <inheritdoc/>
    public override bool SupportsDiscoveryEnumeration()
    =>  !DisableDiscoveryEnumeration;
}

