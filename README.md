# CodingDad.NET.Common

Welcome to the `CodingDad.NET.Common` class library! This repository contains a collection of commonly used classes and utilities designed to streamline and enhance your .NET projects. As a developer, you often encounter repetitive tasks and patterns across different projects. The `CodingDad.NET.Common` library aims to provide a reusable, well-structured set of tools to help you write cleaner, more efficient code, and reduce redundancy.

## Overview

The `CodingDad.NET.Common` library is a versatile and comprehensive set of components built to support a wide range of .NET applications. Whether you're developing web applications, desktop applications, or services, this library offers essential utilities to simplify your development process.

## Table of Contents

- [Overview](#overview)
- [Behaviors](#behaviors)
  - [BehaviorAttacher](#behaviorattacher)
    - [Properties](#properties)
    - [Methods](#methods)
    - [Private Methods](#private-methods)
    - [Usage](#usage)
  - [MefSafeBehaviorAttacher](#mefsafebehaviorattacher)
    - [Properties](#properties-1)
    - [Methods](#methods-1)
    - [Private Methods](#private-methods-1)
    - [Usage](#usage-1)
  - [WindowCloseBehavior](#windowclosebehavior)
    - [Properties](#properties-2)
    - [Methods](#methods-2)
    - [Private Methods](#private-methods-2)
    - [Usage](#usage-2)
- [Commands](#commands)
  - [RelayCommand](#relaycommand)
    - [Properties](#properties-3)
    - [Methods](#methods-3)
    - [Usage](#usage-3)
- [CustomControls](#customcontrols)
  - [ResizableGridControl](#resizablegridcontrol)
    - [Methods](#methods-4)
    - [Event Handlers](#event-handlers)
    - [Usage](#usage-4)
- [DbHelpers](#dbhelpers)
  - [MongoDbHelper](#mongodbhelper)
    - [Methods](#methods-5)
    - [Usage](#usage-5)
    - [Example](#example)
  - [SqlServerHelper](#sqlserverhelper)
    - [Methods](#methods-6)
    - [Usage](#usage-6)
    - [Example](#example-1)
  - [SQLiteHelper](#sqlitehelper)
    - [Methods](#methods-7)
    - [Usage](#usage-7)
    - [Example](#example-2)

## Behaviors

### BehaviorAttacher

The `BehaviorAttacher` class is a static helper class designed to dynamically attach behaviors to WPF `UIElement` controls using attached properties. This is particularly useful for scenarios where you want to declaratively add behaviors to your UI elements in XAML without having to explicitly define them in the code-behind.

#### Properties

- `AttachBehaviorsProperty`: An attached property of type `bool` used to trigger the attachment of behaviors.
- `BehaviorTypesProperty`: An attached property of type `string` that specifies a comma-separated list of behavior types to attach to the target element.

#### Methods

- `GetBehaviorTypes(DependencyObject element)`: Retrieves the value of the `BehaviorTypesProperty` attached property.
- `SetAttachBehaviors(DependencyObject element, bool value)`: Sets the value of the `AttachBehaviorsProperty` attached property.
- `SetBehaviorTypes(DependencyObject element, string value)`: Sets the value of the `BehaviorTypesProperty` attached property.

#### Private Methods

- `FindExistingBehavior(BehaviorCollection behaviors, Type behaviorType)`: Searches for an existing behavior of the specified type within a given `BehaviorCollection`.
- `OnAttachBehaviorsChanged(DependencyObject element, DependencyPropertyChangedEventArgs e)`: Callback method that gets invoked when the attached properties change. It parses the behavior types, creates instances of the specified behaviors, and attaches them to the target element.

### Usage

To use the `BehaviorAttacher` class in your XAML, follow these steps:

1. Include the necessary namespaces:
    ```xml
    xmlns:i="http://schemas.microsoft.com/expression/2010/interactivity"
    xmlns:local="clr-namespace:YourNamespace"
    ```

2. Attach the behaviors to your `UIElement` using the attached properties:
    ```xml
    <Button Content="Click Me"
            local:BehaviorAttacher.AttachBehaviors="True"
            local:BehaviorAttacher.BehaviorTypes="NamespaceOfBehavior1.Behavior1, NamespaceOfBehavior2.Behavior2" />
    ```

In this example, replace `NamespaceOfBehavior1.Behavior1` and `NamespaceOfBehavior2.Behavior2` with the fully qualified names of the behaviors you want to attach to the `Button`.

Here's a complete example in context:

```xml
<Window x:Class="YourNamespace.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:i="http://schemas.microsoft.com/expression/2010/interactivity"
        xmlns:local="clr-namespace:YourNamespace"
        Title="MainWindow" Height="350" Width="525">
    <Grid>
        <Button Content="Click Me"
                local:BehaviorAttacher.AttachBehaviors="True"
                local:BehaviorAttacher.BehaviorTypes="NamespaceOfBehavior1.Behavior1, NamespaceOfBehavior2.Behavior2" />
    </Grid>
</Window>
```

This setup allows you to dynamically attach multiple behaviors to UI elements by specifying the behavior types in a comma-separated string. The BehaviorAttacher class will handle the creation and attachment of these behaviors at runtime.

### MefSafeBehaviorAttacher

The `MefSafeBehaviorAttacher` class is a static helper class designed to dynamically attach behaviors to XAML elements using MEF (Managed Extensibility Framework) for dependency injection. This class is useful for scenarios where you want to declaratively add behaviors to your UI elements in XAML without having to explicitly define them in the code-behind, and to leverage MEF for creating behavior instances.

#### Properties

- `AttachBehaviorsProperty`: An attached property of type `bool` used to trigger the attachment of behaviors.
- `BehaviorTypesProperty`: An attached property of type `string` that specifies a comma-separated list of behavior types to attach to the target element.

#### Methods

- `GetBehaviorTypes(DependencyObject element)`: Retrieves the value of the `BehaviorTypesProperty` attached property.
- `SetAttachBehaviors(DependencyObject element, bool value)`: Sets the value of the `AttachBehaviorsProperty` attached property.
- `SetBehaviorTypes(DependencyObject element, string value)`: Sets the value of the `BehaviorTypesProperty` attached property.

#### Private Methods

- `FindExistingBehavior(BehaviorCollection behaviors, Type behaviorType)`: Searches for an existing behavior of the specified type within a given `BehaviorCollection`.
- `OnAttachBehaviorsChanged(DependencyObject element, DependencyPropertyChangedEventArgs e)`: Callback method that gets invoked when the attached properties change. It parses the behavior types, creates instances of the specified behaviors using MEF, and attaches them to the target element.

### Usage

To use the `MefSafeBehaviorAttacher` class in your XAML, follow these steps:

1. Include the necessary namespaces:
    ```xml
    xmlns:i="http://schemas.microsoft.com/expression/2010/interactivity"
    xmlns:local="clr-namespace:YourNamespace"
    ```

2. Attach the behaviors to your `UIElement` using the attached properties:
    ```xml
    <Button Content="Click Me"
            local:MefSafeBehaviorAttacher.AttachBehaviors="True"
            local:MefSafeBehaviorAttacher.BehaviorTypes="NamespaceOfBehavior1.Behavior1, NamespaceOfBehavior2.Behavior2" />
    ```

In this example, replace `NamespaceOfBehavior1.Behavior1` and `NamespaceOfBehavior2.Behavior2` with the fully qualified names of the behaviors you want to attach to the `Button`.

Here's a complete example in context:

```xml
<Window x:Class="YourNamespace.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:i="http://schemas.microsoft.com/expression/2010/interactivity"
        xmlns:local="clr-namespace:YourNamespace"
        Title="MainWindow" Height="350" Width="525">
    <Grid>
        <Button Content="Click Me"
                local:MefSafeBehaviorAttacher.AttachBehaviors="True"
                local:MefSafeBehaviorAttacher.BehaviorTypes="NamespaceOfBehavior1.Behavior1, NamespaceOfBehavior2.Behavior2" />
    </Grid>
</Window>
```

This setup allows you to dynamically attach multiple behaviors to UI elements by specifying the behavior types in a comma-separated string. The MefSafeBehaviorAttacher class will handle the creation and attachment of these behaviors at runtime, leveraging MEF for dependency injection.

### WindowCloseBehavior

The `WindowCloseBehavior` class is a static helper class designed to bind a command to the closing of a WPF window. This allows you to perform custom actions when a window is closed, such as saving state or cleaning up resources.

#### Properties

- `CloseCommandProperty`: An attached property of type `ICommand` used to bind a command to the window's close event.

#### Methods

- `GetCloseCommand(DependencyObject obj)`: Retrieves the value of the `CloseCommandProperty` attached property.
- `SetCloseCommand(DependencyObject obj, ICommand value)`: Sets the value of the `CloseCommandProperty` attached property.

#### Private Methods

- `OnCloseCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)`: Callback method that gets invoked when the `CloseCommandProperty` changes. It attaches or detaches the window's Closed event handler based on the property value.
- `OnWindowClosed(object sender, EventArgs e)`: Handles the Closed event of the window and executes the bound command.

### Usage

To use the `WindowCloseBehavior` class in your XAML, follow these steps:

1. Include the necessary namespace:
    ```xml
    xmlns:local="clr-namespace:YourNamespace"
    ```

2. Bind the `CloseCommand` property to a command in your view model:
    ```xml
    <Window x:Class="YourNamespace.MainWindow"
            xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
            xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
            xmlns:local="clr-namespace:YourNamespace"
            Title="MainWindow" Height="350" Width="525"
            local:WindowCloseBehavior.CloseCommand="{Binding CloseCommand}">
        <!-- Window content -->
    </Window>
    ```

In this example, replace `YourNamespace` with the appropriate namespace for your project. The `CloseCommand` property is bound to a command in your view model that will be executed when the window is closed.

Here's a complete example in context:

```xml
<Window x:Class="YourNamespace.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:local="clr-namespace:YourNamespace"
        Title="MainWindow" Height="350" Width="525"
        local:WindowCloseBehavior.CloseCommand="{Binding CloseCommand}">
    <Grid>
        <!-- Window content -->
    </Grid>
</Window>
```

This setup allows you to bind a command to the window's close event, enabling you to perform custom actions when the window is closed.

## Commands
### RelayCommand

The `RelayCommand` class is a simple yet powerful implementation of the `ICommand` interface that allows you to delegate command logic using delegates. This class is especially useful for implementing commands in MVVM (Model-View-ViewModel) patterns in WPF, UWP, and other XAML-based applications.

#### Properties

- `CanExecuteChanged`: An event that occurs when changes occur that affect whether or not the command should execute.

#### Methods

- `RelayCommand(Action<object> execute)`: Initializes a new instance of the `RelayCommand` class with an execute delegate.
- `RelayCommand(Predicate<object?>? canExecute, Action<object> execute)`: Initializes a new instance of the `RelayCommand` class with canExecute and execute delegates.
- `CanExecute(object? parameter)`: Defines the method that determines whether the command can execute in its current state.
- `Execute(object parameter)`: Defines the method to be called when the command is invoked.

### Usage

To use the `RelayCommand` class in your application, follow these steps:

1. Create an instance of `RelayCommand` in your ViewModel:
    ```csharp
    public class MainViewModel
    {
        public ICommand MyCommand { get; }

        public MainViewModel()
        {
            MyCommand = new RelayCommand(ExecuteMyCommand, CanExecuteMyCommand);
        }

        private bool CanExecuteMyCommand(object? parameter)
        {
            // Your logic to determine if the command can execute
            return true;
        }

        private void ExecuteMyCommand(object parameter)
        {
            // Your logic to execute the command
        }
    }
    ```

2. Bind the command to a UI element in your XAML:
    ```xml
    <Window x:Class="YourNamespace.MainWindow"
            xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
            xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
            Title="MainWindow" Height="350" Width="525">
        <Window.DataContext>
            <local:MainViewModel />
        </Window.DataContext>
        <Grid>
            <Button Content="Click Me" Command="{Binding MyCommand}" />
        </Grid>
    </Window>
    ```

In this example, replace `YourNamespace` with the appropriate namespace for your project. The `MyCommand` property is bound to the `RelayCommand` in your ViewModel, and the `Button` in the XAML will execute the command when clicked.

Here's a complete example in context:

```xml
<Window x:Class="YourNamespace.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        Title="MainWindow" Height="350" Width="525">
    <Window.DataContext>
        <local:MainViewModel />
    </Window.DataContext>
    <Grid>
        <Button Content="Click Me" Command="{Binding MyCommand}" />
    </Grid>
</Window>
```

This setup allows you to easily bind commands to UI elements and define the command logic in your ViewModel.

## CustomControls

### ResizableGridControl

The `ResizableGridControl` class is a custom WPF `Grid` control that supports dynamic row and column resizing. This control allows users to resize rows and columns by dragging the edges with the mouse, providing a more flexible and interactive layout experience.

#### Methods

- `AddColumn()`: Adds a new column to the grid.
- `AddRow()`: Adds a new row to the grid.
- `RemoveColumn(int index)`: Removes a column at the specified index from the grid.
- `RemoveRow(int index)`: Removes a row at the specified index from the grid.

#### Event Handlers

- `GridControl_MouseDown(object sender, MouseButtonEventArgs e)`: Handles the MouseDown event, initiating row or column resizing.
- `GridControl_MouseLeave(object sender, MouseEventArgs e)`: Handles the MouseLeave event, cancelling row or column resizing.
- `GridControl_MouseMove(object sender, MouseEventArgs e)`: Handles the MouseMove event, performing row or column resizing.
- `GridControl_MouseUp(object sender, MouseButtonEventArgs e)`: Handles the MouseUp event, finalizing row or column resizing.

### Usage

To use the `ResizableGridControl` in your application, follow these steps:

1. Include the necessary namespace:
    ```xml
    xmlns:local="clr-namespace:YourNamespace"
    ```

2. Use the `ResizableGridControl` in your XAML:
    ```xml
    <local:ResizableGridControl>
        <!-- Add your content here -->
    </local:ResizableGridControl>
    ```

Here's a complete example in context:

```xml
<Window x:Class="YourNamespace.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:local="clr-namespace:YourNamespace"
        Title="MainWindow" Height="350" Width="525">
    <local:ResizableGridControl>
        <local:ResizableGridControl.ColumnDefinitions>
            <ColumnDefinition Width="*" />
            <ColumnDefinition Width="2*" />
        </local:ResizableGridControl.ColumnDefinitions>
        <local:ResizableGridControl.RowDefinitions>
            <RowDefinition Height="*" />
            <RowDefinition Height="2*" />
        </local:ResizableGridControl.RowDefinitions>
        <TextBlock Grid.Row="0" Grid.Column="0" Text="Cell 0,0" />
        <TextBlock Grid.Row="0" Grid.Column="1" Text="Cell 0,1" />
        <TextBlock Grid.Row="1" Grid.Column="0" Text="Cell 1,0" />
        <TextBlock Grid.Row="1" Grid.Column="1" Text="Cell 1,1" />
    </local:ResizableGridControl>
</Window>
```

This setup allows you to use the ResizableGridControl to create grids with dynamically resizable rows and columns. Users can resize the rows and columns by dragging the edges with the mouse.

## DbHelpers
### MongoDbHelper

The `MongoDbHelper<T>` class is a general-purpose interface for interacting with a MongoDB database. This class provides methods to create, read, update, and delete documents in a MongoDB collection, and it uses the `LoggerProvider` for logging operations.

#### Methods

- `MongoDbHelper(string connectionString, string databaseName, string collectionName)`: Initializes a new instance of the `MongoDbHelper<T>` class.
- `CreateUserAsync(string email, string username, string password)`: Asynchronously creates a new user in the MongoDB database.
- `DeleteAsync(ObjectId id)`: Deletes a document from the collection by its ObjectId.
- `GetAllAsync()`: Retrieves all documents from the collection.
- `GetByIdAsync(ObjectId id)`: Retrieves a document by its ObjectId.
- `InsertAsync(T document)`: Inserts a document into the collection.
- `UpdateAsync(ObjectId id, T document)`: Updates a document in the collection by its ObjectId.
- `VerifyUserAsync(string email, string password)`: Asynchronously verifies if a user with the given email and password exists in the MongoDB database.

### Usage

To use the `MongoDbHelper<T>` class in your application, follow these steps:

1. Create an instance of `MongoDbHelper<T>` in your code:
    ```csharp
    var mongoDbHelper = new MongoDbHelper<MyDocumentClass>("your_connection_string", "your_database_name", "your_collection_name");
    ```

2. Use the available methods to interact with the MongoDB database. For example, to insert a document:
    ```csharp
    await mongoDbHelper.InsertAsync(new MyDocumentClass { Name = "John", Age = 30 });
    ```

3. To retrieve all documents from the collection:
    ```csharp
    var allDocuments = await mongoDbHelper.GetAllAsync();
    ```

4. To verify a user:
    ```csharp
    bool isVerified = await mongoDbHelper.VerifyUserAsync("user@example.com", "password");
    ```

### Example

Here's a complete example in context:

```csharp
public class MyDocumentClass
{
    public ObjectId Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
}

var mongoDbHelper = new MongoDbHelper<MyDocumentClass>("your_connection_string", "your_database_name", "your_collection_name");

await mongoDbHelper.InsertAsync(new MyDocumentClass { Name = "John", Age = 30 });

var allDocuments = await mongoDbHelper.GetAllAsync();

bool isVerified = await mongoDbHelper.VerifyUserAsync("user@example.com", "password");
```

This setup allows you to easily interact with a MongoDB database, performing CRUD operations and logging activities using the LoggerProvider.

### SqlServerHelper

The `SqlServerHelper` class is a general-purpose interface for interacting with a SQL Server database. This class provides methods to create, read, update, and delete records in a SQL Server database, and it uses the `LoggerProvider` for logging operations.

#### Methods

- `SqlServerHelper(string connectionString)`: Initializes a new instance of the `SqlServerHelper` class.
- `CreateUserAsync(string email, string username, string password)`: Asynchronously creates a new user in the SQL Server database.
- `ExecuteNonQueryAsync(string commandText, SqlParameter[] parameters = null)`: Executes a non-query SQL command.
- `ExecuteQueryAsync(string queryText, SqlParameter[] parameters = null)`: Executes a SQL query and returns the result as a DataTable.
- `VerifyUser(string email, string password)`: Verifies if a user with the given email and password exists in the database.
- `VerifyUserAsync(string email, string password)`: Asynchronously verifies if a user with the given email and password exists in the database.

### Usage

To use the `SqlServerHelper` class in your application, follow these steps:

1. Create an instance of `SqlServerHelper` in your code:
    ```csharp
    var sqlServerHelper = new SqlServerHelper("your_connection_string");
    ```

2. Use the available methods to interact with the SQL Server database. For example, to insert a user:
    ```csharp
    await sqlServerHelper.CreateUserAsync("user@example.com", "username", "password");
    ```

3. To execute a non-query command:
    ```csharp
    string commandText = "INSERT INTO Users (Name, Age) VALUES (@Name, @Age)";
    SqlParameter[] parameters = {
        new SqlParameter("@Name", "John"),
        new SqlParameter("@Age", 30)
    };
    await sqlServerHelper.ExecuteNonQueryAsync(commandText, parameters);
    ```

4. To execute a query and get results as a DataTable:
    ```csharp
    string queryText = "SELECT * FROM Users WHERE Age > @Age";
    SqlParameter[] parameters = {
        new SqlParameter("@Age", 25)
    };
    DataTable result = await sqlServerHelper.ExecuteQueryAsync(queryText, parameters);
    ```

5. To verify a user:
    ```csharp
    bool isVerified = sqlServerHelper.VerifyUser("user@example.com", "password");
    ```

### Example

Here's a complete example in context:

```csharp
public class User
{
    public string Email { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}

var sqlServerHelper = new SqlServerHelper("your_connection_string");

await sqlServerHelper.CreateUserAsync("user@example.com", "username", "password");

string commandText = "INSERT INTO Users (Name, Age) VALUES (@Name, @Age)";
SqlParameter[] parameters = {
    new SqlParameter("@Name", "John"),
    new SqlParameter("@Age", 30)
};
await sqlServerHelper.ExecuteNonQueryAsync(commandText, parameters);

string queryText = "SELECT * FROM Users WHERE Age > @Age";
parameters = new SqlParameter[] {
    new SqlParameter("@Age", 25)
};
DataTable result = await sqlServerHelper.ExecuteQueryAsync(queryText, parameters);

bool isVerified = sqlServerHelper.VerifyUser("user@example.com", "password");
```

This setup allows you to easily interact with a SQL Server database, performing CRUD operations and logging activities using the LoggerProvider.

### SQLiteHelper

The `SQLiteHelper` class is a general-purpose interface for interacting with a SQLite database. This class provides methods to create, read, update, and delete records in a SQLite database, and it uses the `LoggerProvider` for logging operations.

#### Methods

- `SQLiteHelper(string connectionString)`: Initializes a new instance of the `SQLiteHelper` class.
- `CreateTableAsync(string createTableQuery)`: Asynchronously creates a table if it doesn't already exist.
- `CreateUserAsync(string email, string username, string password)`: Asynchronously creates a new user in the SQLite database.
- `DeleteRecordAsync(string tableName, string conditionColumn, object conditionValue)`: Deletes a record from a specified table based on a condition.
- `ExecuteNonQueryAsync(string commandText, SQLiteParameter[] parameters = null)`: Executes a non-query SQL command asynchronously.
- `ExecuteQueryAsync(string queryText, SQLiteParameter[] parameters = null)`: Executes a SQL query and returns the result as a DataTable.
- `InsertIntoTableAsync(string tableName, Dictionary<string, object?> columnData)`: Inserts a new record into a specified table.
- `RetrieveAllViewModelStatesAsync(string viewModelName)`: Asynchronously retrieves the serialized states of all instances of a specific ViewModel from the database.
- `SaveSingletonViewModelStateAsync(string viewModelName, string serializedState)`: Asynchronously saves the serialized state of a ViewModel into the database.
- `SaveViewModelStateAsync(string viewModelName, string instanceId, string serializedState)`: Asynchronously saves the serialized state of a ViewModel into the database.
- `StoreUniqueIdentifierAsync(string userId, string uniqueIdentifier)`: Stores a unique identifier for a user.
- `VerifyUserAsync(string email, string password)`: Asynchronously verifies if a user with the given email and password exists in the SQLite database.

### Usage

To use the `SQLiteHelper` class in your application, follow these steps:

1. Create an instance of `SQLiteHelper` in your code:
    ```csharp
    var sqliteHelper = new SQLiteHelper("your_connection_string");
    ```

2. Use the available methods to interact with the SQLite database. For example, to create a table:
    ```csharp
    await sqliteHelper.CreateTableAsync("CREATE TABLE IF NOT EXISTS Users (Id INTEGER PRIMARY KEY, Email TEXT, Username TEXT, Password TEXT)");
    ```

3. To insert a user:
    ```csharp
    await sqliteHelper.CreateUserAsync("user@example.com", "username", "password");
    ```

4. To execute a non-query command:
    ```csharp
    string commandText = "INSERT INTO Users (Name, Age) VALUES (@Name, @Age)";
    SQLiteParameter[] parameters = {
        new SQLiteParameter("@Name", "John"),
        new SQLiteParameter("@Age", 30)
    };
    await sqliteHelper.ExecuteNonQueryAsync(commandText, parameters);
    ```

5. To execute a query and get results as a DataTable:
    ```csharp
    string queryText = "SELECT * FROM Users WHERE Age > @Age";
    SQLiteParameter[] parameters = {
        new SQLiteParameter("@Age", 25)
    };
    DataTable result = await sqliteHelper.ExecuteQueryAsync(queryText, parameters);
    ```

6. To verify a user:
    ```csharp
    bool isVerified = await sqliteHelper.VerifyUserAsync("user@example.com", "password");
    ```

### Example

Here's a complete example in context:

```csharp
public class User
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}

var sqliteHelper = new SQLiteHelper("your_connection_string");

await sqliteHelper.CreateTableAsync("CREATE TABLE IF NOT EXISTS Users (Id INTEGER PRIMARY KEY, Email TEXT, Username TEXT, Password TEXT)");

await sqliteHelper.CreateUserAsync("user@example.com", "username", "password");

string commandText = "INSERT INTO Users (Name, Age) VALUES (@Name, @Age)";
SQLiteParameter[] parameters = {
    new SQLiteParameter("@Name", "John"),
    new SQLiteParameter("@Age", 30)
};
await sqliteHelper.ExecuteNonQueryAsync(commandText, parameters);

string queryText = "SELECT * FROM Users WHERE Age > @Age";
parameters = new SQLiteParameter[] {
    new SQLiteParameter("@Age", 25)
};
DataTable result = await sqliteHelper.ExecuteQueryAsync(queryText, parameters);

bool isVerified = await sqliteHelper.VerifyUserAsync("user@example.com", "password");
```

This setup allows you to easily interact with a SQLite database, performing CRUD operations and logging activities using the LoggerProvider.