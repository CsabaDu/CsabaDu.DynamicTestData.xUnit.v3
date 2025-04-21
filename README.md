# CsabaDu.DynamicTestData.xUnit.v3

A powerful xUnit.net v3 extension for creating dynamic, strongly-typed test data with fluent configuration options.

## Features

- **Fluent API** for building test cases with clear, chainable syntax
- **Strongly-typed test data** with compile-time type checking
- **Dynamic test display names** automatically generated from test data
- **Comprehensive configuration** including:
  - Custom display names
  - Explicit test marking
  - Skip reasons
  - Timeout values
  - Test traits
- **Support for multiple test scenarios**:
  - Normal test cases
  - Value-returning test cases
  - Exception-throwing test cases
- **Type-safe argument handling** up to 9 parameters
- **Clean separation** between test data and display logic

## Installation

Add the NuGet package to your test project:

```bash
dotnet add package CsabaDu.DynamicTestData.xUnit.v3
```

## Usage

### Basic Example

```csharp
public class CalculatorTests
{
    [Theory]
    [DynamicTestDisplayName(nameof(GetAdditionTestData))]
    public void AdditionTest(TestData<int, int, int> testData)
    {
        // Arrange
        var calculator = new Calculator();
        int actual = calculator.Add(testData.Arg1, testData.Arg2);
        
        // Assert
        Assert.Equal(testData.Expected, actual);
    }

    public static IEnumerable<TheoryTestDataRow> GetAdditionTestData()
    {
        var testData = new DynamicTheoryTestDataSource<int, int, int>(ArgsCode.Properties);

        testData.AddTestDataToTheoryTestData("Simple addition", "3", 1, 2, 3);
        testData.AddTestDataToTheoryTestData("Add zero", "5", 5, 0, 5);
        testData.AddTestDataToTheoryTestData("Negative numbers", "-3", -1, -2, -3);

        return testData.TheoryTestData;
    }
}
```

### Fluent Configuration

```csharp
public static IEnumerable<TheoryTestDataRow> GetConfiguredTestData()
{
    var testData = new DynamicTheoryTestDataSource<string, bool>(ArgsCode.Instance);

    testData
        .AddTestDataToTheoryTestData("Empty string", "True", "", true)
        .SetTestDisplayName("Test_EmptyString")
        .SetExplicit(true)
        .SetTimeout(1000)
        .SetTraits("Category", "Validation");

    testData
        .AddTestDataToTheoryTestData("Null string", "False", null, false)
        .SetSkip("Temporarily disabled for investigation");

    return testData.TheoryTestData;
}
```

### Exception Testing

```csharp
public static IEnumerable<TheoryTestDataRow> GetExceptionTestData()
{
    var testData = new DynamicTheoryTestDataSource<string>(ArgsCode.Properties);

    testData.AddTestDataThrowsToTheoryTestData(
        "Null argument",
        new ArgumentNullException("input"),
        null);

    return testData.TheoryTestData;
}
```

## API Reference

### Core Interfaces

#### `ISetTheoryDataRow<TTheoryDataRow>`
Provides a fluent interface for configuring theory test data rows with methods for:
- Setting display names (`SetTestDisplayName`)
- Marking tests as explicit (`SetExplicit`)
- Setting skip reasons (`SetSkip`)
- Configuring timeouts (`SetTimeout`)
- Adding traits (`SetTraits`)

#### `ITheoryTestDataRow`
Represents a row of test data with strongly-typed access to:
- The test data instance (`TestData`)
- Argument conversion strategy (`ArgsCode`)

#### `ITheoryTestData`
Provides initialization capabilities for theory test data collections with:
- Argument conversion strategy (`ArgsCode`)
- Test method name initialization (`InitTestMethodName`)

### Main Classes

#### `TheoryTestDataRow`
A record type that implements both `ITheoryTestDataRow` and `ISetTheoryDataRow<TTheoryDataRow>`, providing:
- Immutable test data configuration
- Fluent API for configuration
- Conversion of test data to method arguments

#### `TheoryTestData`
A collection class for theory test data that:
- Maintains type consistency
- Supports initialization with test method names
- Provides conversion of test data to rows

#### `DynamicTheoryTestDataSource`
Abstract base class for creating strongly-typed test data sources that:
- Enforces type safety
- Provides methods for adding different test case types
- Supports temporary argument conversion strategy changes

### Attributes

#### `DynamicTestDisplayNameAttribute`
Enables dynamic test display names by:
- Specifying a data source member
- Generating display names from test data
- Maintaining clean separation between data and display logic

## Advanced Usage

### Custom Argument Conversion

```csharp
// Use ArgsCode.Instance to pass the entire TestData object
var instanceTestData = new DynamicTheoryTestDataSource<int, int>(ArgsCode.Instance);

// Use ArgsCode.Properties to pass individual properties as arguments
var propertiesTestData = new DynamicTheoryTestDataSource<int, int>(ArgsCode.Properties);
```

### Temporary Strategy Changes

```csharp
testData.AddOptionalToTheoryTestData(() => {
    // Add test data with different ArgsCode temporarily
    testData.AddTestDataToTheoryTestData("Special case", "Result", 42, 42);
}, ArgsCode.Instance);
```

## Best Practices

1. **Initialize test method name** early in your test data setup:
   ```csharp
   var testData = new DynamicTheoryTestDataSource<int>(ArgsCode.Properties);
   testData.InitTestMethodName(nameof(MyTestMethod));
   ```

2. **Use descriptive names** for test cases in the `definition` parameter to generate meaningful display names.

3. **Group related tests** using traits for better test organization:
   ```csharp
   .SetTraits("Category", "Performance")
   ```

4. **Limit use of explicit tests** and always provide clear reasons when using `SetSkip`.

5. **Consider timeout values** carefully for tests that might hang or run too long.

## License

MIT License - see the [LICENSE](LICENSE) file for details.

