# CodingDad.NET.Common

Welcome to the `CodingDad.NET.Common` class library! This repository contains a collection of commonly used classes and utilities designed to streamline and enhance your .NET projects. As a developer, you often encounter repetitive tasks and patterns across different projects. The `CodingDad.NET.Common` library aims to provide a reusable, well-structured set of tools to help you write cleaner, more efficient code, and reduce redundancy.

## Overview

The `CodingDad.NET.Common` library is a versatile and comprehensive set of components built to support a wide range of .NET applications. Whether you're developing web applications, desktop applications, or services, this library offers essential utilities to simplify your development process.

## Table of Contents

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
- [Factories](#factories)
  - [BehaviorFactory](#behaviorfactory)
    - [Methods](#methods-8)
    - [Usage](#usage-8)
    - [Example](#example-3)
  - [ViewModelFactory](#viewmodelfactory)
    - [Methods](#methods-9)
    - [Usage](#usage-9)
    - [Example](#example-4)
- [InputOutput](#inputoutput)
  - [CursorHelper](#cursorhelper)
    - [Methods](#methods-10)
    - [Usage](#usage-10)
    - [Example](#example-5)
- [Locators](#locators)
  - [ContainerLocator](#containerlocator)
    - [Methods](#methods-11)
    - [Usage](#usage-11)
    - [Example](#example-6)
- [Loggers](#loggers)
  - [LoggerProvider](#loggerprovider)
    - [Methods](#methods-12)
    - [Usage](#usage-12)
    - [Example](#example-7)
  - [ColorConsoleLogger](#colorconsolelogger)
    - [Methods](#methods-13)
    - [Private Methods](#private-methods-3)
    - [Usage](#usage-13)
    - [Example](#example-8)
  - [DatabaseLoggers](#databaseloggers)
    - [DbLoggerBase](#dbloggerbase)
      - [Constructor](#constructor)
      - [Methods](#methods-14)
    - [DbLoggerProvider](#dbloggerprovider)
      - [Constructor](#constructor-1)
      - [Methods](#methods-15)
    - [MongoDbLogger](#mongodblogger)
      - [Constructor](#constructor-2)
      - [Methods](#methods-16)
    - [SqliteLogger](#sqlitelogger)
      - [Constructor](#constructor-3)
      - [Methods](#methods-17)
    - [SqlServerLogger](#sqlserverlogger)
      - [Constructor](#constructor-4)
      - [Methods](#methods-18)
    - [DbLoggerConfiguration](#dbloggerconfiguration)
      - [Properties](#properties-4)
      - [Usage Example](#usage-example)
- [StorageLocationManager](#storagelocationmanager)
  - [Methods](#methods-19)
  - [Usage](#usage-14)
  - [Example](#example-9)
- [User Creation and Login](#user-creation-and-login)
  - [Components](#components)
    - [UserModel](#usermodel)
    - [UserIdentifier](#useridentifier)
    - [UserViewModel](#userviewmodel)
  - [User Controls](#user-controls)
    - [UserCreateView](#usercreateview)
    - [UserLoginView](#userloginview)
  - [Usage](#usage-15)
- [MefJsonUtility](#mefjsonutility)
  - [Methods](#methods-20)
  - [Usage](#usage-16)
  - [Example](#example-10)
- [BaseViewModel](#baseviewmodel)
  - [Properties](#properties-5)
  - [Methods](#methods-21)
  - [Usage](#usage-17)

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

## Factories
### BehaviorFactory

The `BehaviorFactory` class provides methods to create instances of `Behavior` types with dependency injection support using MEF (Managed Extensibility Framework). This class is particularly useful for scenarios where behaviors require constructor parameters or dependency injection.

#### Methods

- `CreateBehavior<T>(params object[] constructorArgs) where T : Behavior`: Creates an instance of a `Behavior` with the specified constructor arguments.
- `CreateBehavior(Type type)`: General-purpose method to create an instance of any `Behavior` type.
- `Initialize(CompositionContainer compositionContainer)`: Initializes the `BehaviorFactory` with a `CompositionContainer` for dependency injection.

#### Private Methods

- `EnsureInitialized()`: Ensures that the factory has been initialized before use.
- `SatisfyImports(Behavior instance)`: Performs MEF composition on the instance to inject dependencies.
- `ValidateInstance(Behavior instance, Type type)`: Validates that an instance was created successfully.

### Usage

To use the `BehaviorFactory` class, follow these steps:

1. Initialize the `BehaviorFactory` with a `CompositionContainer`:
    ```csharp
    var catalog = new AggregateCatalog();
    // Add parts to the catalog here
    var container = new CompositionContainer(catalog);
    BehaviorFactory.Initialize(container);
    ```

2. Create an instance of a `Behavior` with constructor arguments:
    ```csharp
    var behavior = BehaviorFactory.CreateBehavior<MyBehavior>(arg1, arg2);
    ```

3. Create an instance of a `Behavior` without constructor arguments:
    ```csharp
    var behavior = BehaviorFactory.CreateBehavior(typeof(MyBehavior));
    ```

### Example

Here's a complete example in context:

```csharp
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using Microsoft.Xaml.Behaviors;

public class MyBehavior : Behavior<UIElement>
{
    [Import]
    public IService MyService { get; set; }

    protected override void OnAttached()
    {
        base.OnAttached();
        // Use MyService
    }
}

// Setup MEF
var catalog = new AggregateCatalog();
catalog.Catalogs.Add(new AssemblyCatalog(typeof(MyBehavior).Assembly));
var container = new CompositionContainer(catalog);
BehaviorFactory.Initialize(container);

// Create an instance of MyBehavior
var behavior = BehaviorFactory.CreateBehavior<MyBehavior>();
```

This setup allows you to create and use behaviors with dependency injection using MEF.

### ViewModelFactory

The `ViewModelFactory` class provides methods to create instances of ViewModel types with dependency injection support using MEF (Managed Extensibility Framework). This class is particularly useful for scenarios where ViewModels require constructor parameters or dependency injection.

#### Methods

- `CreateViewModel<T>() where T : new()`: Creates an instance of a ViewModel of type `T` with a parameterless constructor and satisfies its dependencies using MEF.
- `CreateViewModel<T>(params object[] constructorArgs) where T : class`: Creates an instance of a ViewModel of type `T` with the specified constructor arguments and satisfies its dependencies using MEF.
- `CreateViewModel(Type type)`: Creates an instance of a specified type and satisfies its dependencies using MEF.
- `Initialize(CompositionContainer compositionContainer)`: Initializes the `ViewModelFactory` with a `CompositionContainer` for dependency injection.

#### Private Methods

- `EnsureInitialized()`: Ensures that the factory has been initialized before use.
- `SatisfyImports(object instance)`: Performs MEF composition on the instance to inject dependencies.
- `ValidateInstance(object instance, Type type)`: Validates that an instance was created successfully.

### Usage

To use the `ViewModelFactory` class, follow these steps:

1. Initialize the `ViewModelFactory` with a `CompositionContainer`:
    ```csharp
    var catalog = new AggregateCatalog();
    // Add parts to the catalog here
    var container = new CompositionContainer(catalog);
    ViewModelFactory.Initialize(container);
    ```

2. Create an instance of a ViewModel with a parameterless constructor:
    ```csharp
    var viewModel = ViewModelFactory.CreateViewModel<MyViewModel>();
    ```

3. Create an instance of a ViewModel with constructor arguments:
    ```csharp
    var viewModel = ViewModelFactory.CreateViewModel<MyViewModel>(arg1, arg2);
    ```

4. Create an instance of a specified type:
    ```csharp
    var viewModel = ViewModelFactory.CreateViewModel(typeof(MyViewModel));
    ```

### Example

Here's a complete example in context:

```csharp
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using Microsoft.Extensions.Logging;

public class MyViewModel
{
    [Import]
    public IService MyService { get; set; }

    public MyViewModel()
    {
        // Default constructor
    }

    public MyViewModel(IService service)
    {
        MyService = service;
    }
}

// Setup MEF
var catalog = new AggregateCatalog();
catalog.Catalogs.Add(new AssemblyCatalog(typeof(MyViewModel).Assembly));
var container = new CompositionContainer(catalog);
ViewModelFactory.Initialize(container);

// Create an instance of MyViewModel with a parameterless constructor
var viewModel1 = ViewModelFactory.CreateViewModel<MyViewModel>();

// Create an instance of MyViewModel with constructor arguments
var viewModel2 = ViewModelFactory.CreateViewModel<MyViewModel>(new ServiceImplementation());

// Create an instance of a specified type
var viewModel3 = ViewModelFactory.CreateViewModel(typeof(MyViewModel));
```

This setup allows you to create and use ViewModels with dependency injection using MEF.

## InputOutput

### CursorHelper

The `CursorHelper` class provides utility methods for creating custom cursors in WPF applications. It includes methods to create cursors from strings and UI elements, allowing for more interactive and visually appealing drag-and-drop operations.

#### Methods

- `CreateStringCursor(string cursorText, double pixelPerDip)`: Creates a cursor containing a given string.
- `CreateUIElementCursor(UIElement element)`: Creates a cursor containing an image that represents the given UI element.

### Usage

To use the `CursorHelper` class, follow these steps:

1. Create a cursor from a string:
    ```csharp
    double dpi = VisualTreeHelper.GetDpi(this).PixelsPerDip;
    Cursor stringCursor = CursorHelper.CreateStringCursor("Drag Me", dpi);
    ```

2. Create a cursor from a UI element:
    ```csharp
    UIElement element = myUIElement;
    Cursor elementCursor = CursorHelper.CreateUIElementCursor(element);
    ```

### Example

Here's a complete example in context:

```csharp
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        double dpi = VisualTreeHelper.GetDpi(this).PixelsPerDip;

        // Create a cursor from a string
        Cursor stringCursor = CursorHelper.CreateStringCursor("Drag Me", dpi);

        // Create a cursor from a UI element
        Button button = new Button { Content = "Drag Me" };
        Cursor elementCursor = CursorHelper.CreateUIElementCursor(button);

        // Set the cursor to the window (for demonstration purposes)
        this.Cursor = stringCursor;

        // Alternatively, set the cursor to an element
        myUIElement.Cursor = elementCursor;
    }
}
```

This setup allows you to create and use custom cursors in your WPF applications, enhancing the user experience for drag-and-drop operations.

##Locators

### ContainerLocator

The `ContainerLocator` class provides a static locator service for resolving dependencies via MEF (Managed Extensibility Framework). This class is designed to compose parts and retrieve exported values, enabling efficient dependency injection and service location.

#### Methods

- `ComposeParts(object obj)`: Composes the parts of a particular object.
- `GetExportedValue<T>(string contractName = "")`: Retrieves the exported value of the specified type `T`.
- `GetExportedValues<T>(string contractName = "")`: Retrieves the exported values of the specified type `T`.
- `Initialize(CompositionContainer container)`: Initializes the composition container.

### Usage

To use the `ContainerLocator` class, follow these steps:

1. Initialize the `ContainerLocator` with a `CompositionContainer`:
    ```csharp
    var catalog = new AggregateCatalog();
    // Add parts to the catalog here
    var container = new CompositionContainer(catalog);
    ContainerLocator.Initialize(container);
    ```

2. Compose the parts of an object:
    ```csharp
    var myObject = new MyObject();
    ContainerLocator.ComposeParts(myObject);
    ```

3. Retrieve an exported value:
    ```csharp
    var myService = ContainerLocator.GetExportedValue<IMyService>();
    ```

4. Retrieve exported values:
    ```csharp
    var services = ContainerLocator.GetExportedValues<IMyService>();
    ```

### Example

Here's a complete example in context:

```csharp
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;

public interface IMyService
{
    void DoWork();
}

[Export(typeof(IMyService))]
public class MyService : IMyService
{
    public void DoWork()
    {
        Console.WriteLine("Work done!");
    }
}

public class MyObject
{
    [Import]
    public IMyService MyService { get; set; }

    public void UseService()
    {
        MyService.DoWork();
    }
}

class Program
{
    static void Main(string[] args)
    {
        // Setup MEF
        var catalog = new AggregateCatalog();
        catalog.Catalogs.Add(new AssemblyCatalog(typeof(MyService).Assembly));
        var container = new CompositionContainer(catalog);
        ContainerLocator.Initialize(container);

        // Compose parts
        var myObject = new MyObject();
        ContainerLocator.ComposeParts(myObject);

        // Use the service
        myObject.UseService();

        // Retrieve an exported value
        var myService = ContainerLocator.GetExportedValue<IMyService>();
        myService.DoWork();

        // Retrieve exported values
        var services = ContainerLocator.GetExportedValues<IMyService>();
        foreach (var service in services)
        {
            service.DoWork();
        }
    }
}
```
This setup allows you to efficiently resolve dependencies and compose parts using MEF, providing a flexible and powerful mechanism for dependency injection in your applications.

##Loggers
### LoggerProvider

The `LoggerProvider` class provides a static logging service using the `ColorConsoleLoggerProvider`. This class is designed to log messages with different log levels, leveraging the `Microsoft.Extensions.Logging` framework.

#### Methods

- `Log(string message, LogLevel logLevel = LogLevel.Information)`: Logs a message with the specified log level.

### Usage

To use the `LoggerProvider` class, follow these steps:

1. Log a message with the default log level (Information):
    ```csharp
    LoggerProvider.Log("This is an informational message.");
    ```

2. Log a message with a specific log level:
    ```csharp
    LoggerProvider.Log("This is an error message.", LogLevel.Error);
    ```

### Example

Here's a complete example in context:

```csharp
using Microsoft.Extensions.Logging;

public class Program
{
    static void Main(string[] args)
    {
        // Log an informational message
        LoggerProvider.Log("Application has started.");

        // Log an error message
        LoggerProvider.Log("An error occurred.", LogLevel.Error);

        // Log a debug message
        LoggerProvider.Log("Debugging application.", LogLevel.Debug);
    }
}
```

This setup allows you to log messages with different log levels using the LoggerProvider class, providing a consistent and centralized logging mechanism for your applications.

### ColorConsoleLogger

The `ColorConsoleLogger` class provides color-coded logging to the console or debug window, leveraging the `Microsoft.Extensions.Logging` framework. This logger is designed to enhance log readability by using different colors for different log levels.

#### Methods

- `ColorConsoleLogger(string name, Func<ColorConsoleLoggerConfiguration> getCurrentConfig, LoggerOutputTarget outputTarget = LoggerOutputTarget.DebugWindow)`: Initializes a new instance of the `ColorConsoleLogger` class.
- `BeginScope<TState>(TState state)`: This method is not supported in `ColorConsoleLogger`.
- `IsEnabled(LogLevel logLevel)`: Checks if the specified log level is enabled.
- `Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)`: Logs a message with the specified log level.

#### Private Methods

- `SetConsoleColor(ConsoleColor color)`: Sets the console color.
- `GetCurrentConfig()`: Retrieves the current logger configuration.
- `WriteLog<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter, ColorConsoleLoggerConfiguration config)`: Writes the log message to the console or debug window.

### Usage

To use the `ColorConsoleLogger` class, follow these steps:

1. Configure the logger in your application:
    ```csharp
    public class Program
    {
        public static void Main(string[] args)
        {
            var logger = new ColorConsoleLogger("MyLogger", () => new ColorConsoleLoggerConfiguration());
            logger.Log(LogLevel.Information, new EventId(1, "AppStart"), "Application has started.", null, (state, exception) => state);
        }
    }
    ```

2. Log messages with different log levels:
    ```csharp
    var logger = new ColorConsoleLogger("MyLogger", () => new ColorConsoleLoggerConfiguration());

    // Log an informational message
    logger.Log(LogLevel.Information, new EventId(1, "InfoEvent"), "This is an informational message.", null, (state, exception) => state);

    // Log an error message
    logger.Log(LogLevel.Error, new EventId(2, "ErrorEvent"), "This is an error message.", null, (state, exception) => state);

    // Log a debug message
    logger.Log(LogLevel.Debug, new EventId(3, "DebugEvent"), "This is a debug message.", null, (state, exception) => state);
    ```

### Example

Here's a complete example in context:

```csharp
using System;
using Microsoft.Extensions.Logging;

public class Program
{
    public static void Main(string[] args)
    {
        // Configure logger
        var logger = new ColorConsoleLogger("MyLogger", () => new ColorConsoleLoggerConfiguration
        {
            LogLevelToColorMap = new Dictionary<LogLevel, ConsoleColor>
            {
                [LogLevel.Trace] = ConsoleColor.Gray,
                [LogLevel.Debug] = ConsoleColor.Blue,
                [LogLevel.Information] = ConsoleColor.Green,
                [LogLevel.Warning] = ConsoleColor.Yellow,
                [LogLevel.Error] = ConsoleColor.Red,
                [LogLevel.Critical] = ConsoleColor.Magenta
            }
        });

        // Log messages
        logger.Log(LogLevel.Information, new EventId(1, "InfoEvent"), "This is an informational message.", null, (state, exception) => state);
        logger.Log(LogLevel.Error, new EventId(2, "ErrorEvent"), "This is an error message.", null, (state, exception) => state);
        logger.Log(LogLevel.Debug, new EventId(3, "DebugEvent"), "This is a debug message.", null, (state, exception) => state);
    }
}

public class ColorConsoleLoggerConfiguration
{
    public Dictionary<LogLevel, ConsoleColor> LogLevelToColorMap { get; set; } = new();
}
```

This setup allows you to log messages with different log levels using the ColorConsoleLogger class, providing a color-coded output to enhance log readability.


### DatabaseLoggers

The `DatabaseLoggers` namespace contains classes for logging to various types of databases using the `Microsoft.Extensions.Logging` framework. This setup allows for centralized logging to SQL Server, SQLite, or MongoDB databases.

#### Classes

- `DbLoggerBase`: Abstract base class for database loggers.
- `DbLoggerProvider`: Provides instances of `DbLoggerBase` based on configuration.
- `MongoDbLogger`: MongoDB-specific logger implementation.
- `SqliteLogger`: SQLite-specific logger implementation.
- `SqlServerLogger`: SQL Server-specific logger implementation.
- `DbLoggerConfiguration`: Base configuration class for database loggers.

#### DbLoggerBase

The `DbLoggerBase` class is an abstract base class for implementing database loggers.

**Constructor:**
- `DbLoggerBase(string connectionString, string logTable, LogLevel minLogLevel)`: Initializes a new instance of the `DbLoggerBase` class.

**Methods:**
- `BeginScope<TState>(TState state)`: Not supported, returns `null`.
- `IsEnabled(LogLevel logLevel)`: Checks if the specified log level is enabled.
- `Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)`: Logs the specified log level, event ID, state, exception, and formatter to the database.
- `LogToDatabase(LogLevel logLevel, string message, Exception exception)`: Abstract method to log the message to the database.

#### DbLoggerProvider

The `DbLoggerProvider` class provides instances of `DbLoggerBase` based on the provided configuration.

**Constructor:**
- `DbLoggerProvider(DbLoggerConfiguration config)`: Initializes a new instance of the `DbLoggerProvider` class.

**Methods:**
- `CreateLogger(string categoryName)`: Creates a new logger instance of the specified category.
- `Dispose()`: Disposes the logger provider and releases resources.
- `CreateLoggerInstance()`: Creates a logger instance based on the current configuration.

#### MongoDbLogger

The `MongoDbLogger` class is a MongoDB-specific logger implementation.

**Constructor:**
- `MongoDbLogger(string connectionString, string logCollection, LogLevel minLogLevel)`: Initializes a new instance of the `MongoDbLogger` class.

**Methods:**
- `LogToDatabase(LogLevel logLevel, string message, Exception exception)`: Logs the message to the MongoDB database.

#### SqliteLogger

The `SqliteLogger` class is a SQLite-specific logger implementation.

**Constructor:**
- `SqliteLogger(string connectionString, string logTable, LogLevel minLogLevel)`: Initializes a new instance of the `SqliteLogger` class.

**Methods:**
- `LogToDatabase(LogLevel logLevel, string message, Exception exception)`: Logs the message to the SQLite database.

#### SqlServerLogger

The `SqlServerLogger` class is a SQL Server-specific logger implementation.

**Constructor:**
- `SqlServerLogger(string connectionString, string logTable, LogLevel minLogLevel)`: Initializes a new instance of the `SqlServerLogger` class.

**Methods:**
- `LogToDatabase(LogLevel logLevel, string message, Exception exception)`: Logs the message to the SQL Server database.

#### DbLoggerConfiguration

The `DbLoggerConfiguration` class provides the configuration for database loggers.

**Properties:**
- `string ConnectionString { get; set; }`: Gets or sets the connection string for the database.
- `string DatabaseType { get; set; }`: Gets or sets the database type (e.g., "SqlServer", "MongoDb", "Sqlite").
- `string LogTable { get; set; }`: Gets or sets the table or collection name for storing logs.
- `LogLevel MinLogLevel { get; set; }`: Gets or sets the minimum log level to log (default is Information).

#### Usage Example

```csharp
var config = new DbLoggerConfiguration
{
    ConnectionString = "your-connection-string",
    DatabaseType = "SqlServer",
    LogTable = "Logs",
    MinLogLevel = LogLevel.Information
};

using var loggerProvider = new DbLoggerProvider(config);
var logger = loggerProvider.CreateLogger("MyLogger");

logger.LogInformation("This is an informational message.");
logger.LogError("This is an error message.");
```

## StorageLocationManager

The `StorageLocationManager` class provides utility methods for managing storage locations on a Windows machine. It includes methods to create directories in various special folders, such as `AppData`, `CommonApplicationData`, and `LocalApplicationData`, as well as custom and temporary directories.

### Methods

- `CreateAppDataDirectory(string appName)`: Creates a directory for the application in the `AppData` folder.
- `CreateCommonAppDataDirectory(string appName)`: Creates a directory for the application in the `CommonApplicationData` folder.
- `CreateCustomDirectory(string customPath, string appName)`: Creates a directory for the application in a custom folder.
- `CreateLocalAppDataDirectory(string appName)`: Creates a directory for the application in the `LocalApplicationData` folder.
- `CreateTempDirectory(string appName)`: Creates a directory for the application in the system's temporary folder.

### Usage

To use the `StorageLocationManager` class, follow these steps:

1. Create a directory in the `AppData` folder:
    ```csharp
    string appDataDirectory = StorageLocationManager.CreateAppDataDirectory("MyApp");
    ```

2. Create a directory in the `CommonApplicationData` folder:
    ```csharp
    string commonAppDataDirectory = StorageLocationManager.CreateCommonAppDataDirectory("MyApp");
    ```

3. Create a directory in a custom folder:
    ```csharp
    string customDirectory = StorageLocationManager.CreateCustomDirectory("C:\\CustomPath", "MyApp");
    ```

4. Create a directory in the `LocalApplicationData` folder:
    ```csharp
    string localAppDataDirectory = StorageLocationManager.CreateLocalAppDataDirectory("MyApp");
    ```

5. Create a directory in the system's temporary folder:
    ```csharp
    string tempDirectory = StorageLocationManager.CreateTempDirectory("MyApp");
    ```

### Example

Here's a complete example in context:

```csharp
using System;
using CodingDad.Common.Storage;

public class Program
{
    public static void Main(string[] args)
    {
        // Create directories
        string appDataDirectory = StorageLocationManager.CreateAppDataDirectory("MyApp");
        Console.WriteLine($"AppData Directory: {appDataDirectory}");

        string commonAppDataDirectory = StorageLocationManager.CreateCommonAppDataDirectory("MyApp");
        Console.WriteLine($"CommonAppData Directory: {commonAppDataDirectory}");

        string customDirectory = StorageLocationManager.CreateCustomDirectory("C:\\CustomPath", "MyApp");
        Console.WriteLine($"Custom Directory: {customDirectory}");

        string localAppDataDirectory = StorageLocationManager.CreateLocalAppDataDirectory("MyApp");
        Console.WriteLine($"LocalAppData Directory: {localAppDataDirectory}");

        string tempDirectory = StorageLocationManager.CreateTempDirectory("MyApp");
        Console.WriteLine($"Temp Directory: {tempDirectory}");
    }
}
```

This setup allows you to manage storage locations for your application efficiently, providing methods to create directories in various special folders on a Windows machine.

## User Creation and Login

This section provides an overview of the components used for handling user creation and login functionalities in the `CodingDad.Common` namespace. These components work together to provide a full implementation for user management in your application.

### Components

#### UserModel

The `UserModel` class represents a user with properties for email, ID, and username.

- **Properties:**
  - `Email`: The user's email address.
  - `Id`: The user's unique identifier.
  - `Username`: The user's username.

#### UserIdentifier

The `UserIdentifier` class provides functionality to uniquely identify a user based on certain system attributes.

- **Constructor:**
  - `UserIdentifier(NetworkInterface[] nics)`: Initializes a new instance of the `UserIdentifier` class with an array of `NetworkInterface` objects.

- **Methods:**
  - `string GetUniqueIdentifier()`: Generates a unique identifier for the user based on system attributes.

#### UserViewModel

The `UserViewModel` class provides a view model for handling user creation and login functionalities. It utilizes the `LoggerProvider` for logging various operations.

- **Constructor:**
  - `UserViewModel(IDatabaseHelper databaseManager, string email, string username)`: Initializes a new instance of the `UserViewModel` class.

- **Properties:**
  - `Email`: The user's email address.
  - `Username`: The user's username.
  - `CreateUserCommand`: Command for creating a new user.
  - `GoToCreateUserCommand`: Command for navigating to the create user view.
  - `LoginCommand`: Command for logging in a user.

- **Methods:**
  - `int GenerateUniqueId()`: Generates a unique ID for the user.
  - `bool IsUserLoggedIn()`: Checks if the user is logged in.
  - `void CreateUser(string password)`: Creates a new user.
  - `void GoToCreateUser()`: Navigates to the create user view.
  - `void ValidateUserLogin(string password)`: Validates the user login.

### User Controls

#### UserCreateView

The `UserCreateView` user control provides the UI for creating a new user.

- **Components:**
  - `TextBox`: For entering the new username.
  - `TextBox`: For entering the new email.
  - `PasswordBox`: For entering the password.
  - `Button`: For submitting the create user command.

#### UserLoginView

The `UserLoginView` user control provides the UI for logging in a user.

- **Components:**
  - `TextBox`: For entering the username.
  - `PasswordBox`: For entering the password.
  - `Button`: For submitting the login command.
  - `Button`: For navigating to the create user view.

### Usage

To integrate the user creation and login functionality into your application, follow these steps:

1. **ViewModel Initialization:**

```csharp
   var databaseManager = new YourDatabaseHelperImplementation();
   var userViewModel = new UserViewModel(databaseManager, "initialEmail@example.com", "initialUsername");
```
2. **Bind ViewModel to Views:**

```xml
<!-- UserCreateView.xaml -->
<UserControl
    x:Class="CodingDad.NET.Common.UserCreationLogin.Views.UserCreateView"
    DataContext="{Binding UserViewModelInstance}"
    ...>
    ...
</UserControl>

<!-- UserLoginView.xaml -->
<UserControl
    x:Class="CodingDad.NET.Common.UserCreationLogin.Views.UserLoginView"
    DataContext="{Binding UserViewModelInstance}"
    ...>
    ...
</UserControl>
```

3. **Configure Commands and Logging:**

Ensure that the LoggerProvider is correctly set up to log user creation and login operations.

By following these steps, you can utilize the user creation and login functionalities provided by the CodingDad.Common namespace in your application, ensuring a consistent and efficient user management experience.

### MefJsonUtility

The `MefJsonUtility` class is a utility for serializing and deserializing objects while also managing MEF (Managed Extensibility Framework) imports. It ensures that deserialized objects have their MEF dependencies satisfied, making it useful for applications that rely on dependency injection and modular components.

#### Methods

- `DeserializeAndSatisfyImports<T>(string json) where T : class`: Deserializes a JSON string to an object of type `T` and satisfies its MEF imports.
- `Initialize(CompositionContainer container)`: Initializes the `MefJsonUtility` with a `CompositionContainer`.
- `Serialize<T>(T obj)`: Serializes an object of type `T` to a JSON string.

### Usage

To use the `MefJsonUtility` class, follow these steps:

1. Initialize the utility with a `CompositionContainer`:
    ```csharp
    var catalog = new AggregateCatalog();
    // Add parts to the catalog here
    var container = new CompositionContainer(catalog);
    MefJsonUtility.Initialize(container);
    ```

2. Serialize an object to a JSON string:
    ```csharp
    var myObject = new MyClass { Property1 = "value1", Property2 = "value2" };
    string jsonString = MefJsonUtility.Serialize(myObject);
    ```

3. Deserialize a JSON string to an object and satisfy MEF imports:
    ```csharp
    string jsonString = "{ \"Property1\": \"value1\", \"Property2\": \"value2\" }";
    var myObject = MefJsonUtility.DeserializeAndSatisfyImports<MyClass>(jsonString);
    ```

### Example

Here's a complete example in context:

```csharp
using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Text.Json;

public class MyClass
{
    public string Property1 { get; set; }
    public string Property2 { get; set; }

    [Import]
    public IMyService MyService { get; set; }
}

public interface IMyService
{
    void DoWork();
}

[Export(typeof(IMyService))]
public class MyService : IMyService
{
    public void DoWork()
    {
        Console.WriteLine("Work done!");
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        // Setup MEF
        var catalog = new AggregateCatalog();
        catalog.Catalogs.Add(new AssemblyCatalog(typeof(MyService).Assembly));
        var container = new CompositionContainer(catalog);
        MefJsonUtility.Initialize(container);

        // Create and serialize an object
        var myObject = new MyClass { Property1 = "value1", Property2 = "value2" };
        string jsonString = MefJsonUtility.Serialize(myObject);

        // Deserialize the object and satisfy MEF imports
        var deserializedObject = MefJsonUtility.DeserializeAndSatisfyImports<MyClass>(jsonString);

        // Use the imported service
        deserializedObject.MyService.DoWork();
    }
}
```

This setup ensures that serialized objects can be deserialized with all their MEF dependencies correctly satisfied, maintaining the integrity of the dependency injection pattern.

## BaseViewModel

The `BaseViewModel` class serves as a base class for view models in a MVVM (Model-View-ViewModel) architecture. It provides essential functionality for property change notification and resource management.

### Properties

- **Id**: A unique identifier for the view model instance.
- **IsDirty**: A flag indicating whether the view model has unsaved changes.

### Methods

- **Dispose()**: Releases all resources used by the view model.
- **Dispose(bool disposing)**: Releases unmanaged resources and optionally releases managed resources.
- **OnPropertyChanged(string? propertyName = null)**: Raises the `PropertyChanged` event to notify the UI of property changes.
- **SetProperty<T>(ref T storage, T value, string? propertyName = null)**: Sets a property and raises the `PropertyChanged` event only if the new value is different from the old value.

### Usage

To create a view model that inherits from `BaseViewModel`, define your properties and use the `SetProperty` method to update their values and notify listeners of changes:

```csharp
public class MyViewModel : BaseViewModel
{
    private string _name;

    public string Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }
}
```

In this example, MyViewModel inherits from BaseViewModel and uses the SetProperty method to manage the Name property, ensuring that property change notifications are sent to the UI.

