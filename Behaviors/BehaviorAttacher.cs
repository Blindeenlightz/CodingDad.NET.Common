using CodingDad.Common.Loggers;
using Microsoft.Xaml.Behaviors;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Windows;

namespace CodingDad.Common.Behaviors
{
    /// <summary>
    /// Provides attached properties to dynamically attach behaviors to WPF elements.
    /// </summary>
    public static class BehaviorAttacher
    {
        /// <summary>
        /// Identifies the AttachBehaviors attached property.
        /// </summary>
        public static readonly DependencyProperty AttachBehaviorsProperty = DependencyProperty.RegisterAttached(
            "AttachBehaviors",
            typeof(bool),
            typeof(BehaviorAttacher),
            new PropertyMetadata(false, OnAttachBehaviorsChanged));

        /// <summary>
        /// Identifies the BehaviorTypes attached property.
        /// </summary>
        public static readonly DependencyProperty BehaviorTypesProperty = DependencyProperty.RegisterAttached(
            "BehaviorTypes",
            typeof(string),
            typeof(BehaviorAttacher),
            new PropertyMetadata(null, OnAttachBehaviorsChanged));

        /// <summary>
        /// Gets the BehaviorTypes property value from the specified element.
        /// </summary>
        /// <param name="element">The element from which to read the property value.</param>
        /// <returns>The behavior types as a comma-separated string.</returns>
        public static string GetBehaviorTypes (DependencyObject element)
        {
            return (string)element.GetValue(BehaviorTypesProperty);
        }

        /// <summary>
        /// Sets the AttachBehaviors property value on the specified element.
        /// </summary>
        /// <param name="element">The element on which to set the property value.</param>
        /// <param name="value">The value to set.</param>
        public static void SetAttachBehaviors (DependencyObject element, bool value)
        {
            element.SetValue(AttachBehaviorsProperty, value);
        }

        /// <summary>
        /// Sets the BehaviorTypes property value on the specified element.
        /// </summary>
        /// <param name="element">The element on which to set the property value.</param>
        /// <param name="value">The behavior types as a comma-separated string.</param>
        public static void SetBehaviorTypes (DependencyObject element, string value)
        {
            element.SetValue(BehaviorTypesProperty, value);
        }

        /// <summary>
        /// Finds an existing behavior of the specified type in the provided behavior collection.
        /// </summary>
        /// <param name="behaviors">The behavior collection to search.</param>
        /// <param name="behaviorType">The type of the behavior to find.</param>
        /// <returns>The found behavior, or null if no such behavior exists.</returns>
        private static Behavior? FindExistingBehavior (BehaviorCollection behaviors, Type behaviorType)
        {
            return behaviors.FirstOrDefault(behavior => behavior.GetType() == behaviorType);
        }

        /// <summary>
        /// Callback method that gets invoked when the attached properties change.
        /// </summary>
        /// <param name="element">The element on which the property changed.</param>
        /// <param name="e">The event data.</param>
        private static void OnAttachBehaviorsChanged (DependencyObject element, DependencyPropertyChangedEventArgs e)
        {
            if (!(element is UIElement uiElement))
            {
                LoggerProvider.Log("Element is not a UIElement.", LogLevel.Warning);
                return;
            }

            var behaviorTypesString = GetBehaviorTypes(element);
            if (string.IsNullOrEmpty(behaviorTypesString))
            {
                LoggerProvider.Log("BehaviorTypes is null or empty.", LogLevel.Warning);
                return;
            }

            var behaviorTypes = behaviorTypesString.Split(',')
                                                   .Select(type => Type.GetType(type.Trim()))
                                                   .Where(type => type != null && typeof(Behavior).IsAssignableFrom(type))
                                                   .ToList();

            if (!behaviorTypes.Any())
            {
                LoggerProvider.Log("No valid behavior types found.", LogLevel.Warning);
                return;
            }

            var behaviors = Interaction.GetBehaviors(uiElement);

            foreach (var behaviorType in behaviorTypes)
            {
                var existingBehavior = FindExistingBehavior(behaviors, behaviorType);

                if (existingBehavior == null)
                {
                    try
                    {
                        var newBehavior = Activator.CreateInstance(behaviorType) as Behavior;
                        if (newBehavior != null)
                        {
                            behaviors.Add(newBehavior);
                            LoggerProvider.Log($"Attached new behavior: {behaviorType.FullName}", LogLevel.Information);
                        }
                        else
                        {
                            LoggerProvider.Log($"Failed to create an instance of behavior: {behaviorType.FullName}", LogLevel.Warning);
                        }
                    }
                    catch (Exception ex)
                    {
                        LoggerProvider.Log($"Exception occurred while attaching behavior: {ex.Message}", LogLevel.Error);
                    }
                }
                else
                {
                    LoggerProvider.Log($"Behavior already attached: {behaviorType.FullName}", LogLevel.Information);
                }
            }
        }
    }
}
