# CodingDad.NET.Common

Welcome to the `CodingDad.NET.Common` class library! This repository contains a collection of commonly used classes and utilities designed to streamline and enhance your .NET projects. As a developer, you often encounter repetitive tasks and patterns across different projects. The `CodingDad.NET.Common` library aims to provide a reusable, well-structured set of tools to help you write cleaner, more efficient code, and reduce redundancy.

## Overview

The `CodingDad.NET.Common` library is a versatile and comprehensive set of components built to support a wide range of .NET applications. Whether you're developing web applications, desktop applications, or services, this library offers essential utilities to simplify your development process.


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