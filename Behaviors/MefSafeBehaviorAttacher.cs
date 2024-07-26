using CodingDad.Common.Factories;
using Microsoft.Xaml.Behaviors;
using System;
using System.Linq;
using System.Windows;

namespace CodingDad.Common.Behaviors
{
	/// <summary>
	/// Provides attached properties to dynamically attach behaviors to WPF elements using MEF for dependency injection.
	/// </summary>
	public static class MefSafeBehaviorAttacher
	{
		public static readonly DependencyProperty AttachBehaviorsProperty = DependencyProperty.RegisterAttached(
			"AttachBehaviors",
			typeof(bool),
			typeof(MefSafeBehaviorAttacher),
			new PropertyMetadata(false, OnAttachBehaviorsChanged));

		public static readonly DependencyProperty BehaviorTypesProperty = DependencyProperty.RegisterAttached(
			"BehaviorTypes",
			typeof(string),
			typeof(MefSafeBehaviorAttacher),
			new PropertyMetadata(null, OnAttachBehaviorsChanged));

		public static string GetBehaviorTypes (DependencyObject element)
		{
			return (string)element.GetValue(BehaviorTypesProperty);
		}

		public static void SetAttachBehaviors (DependencyObject element, bool value)
		{
			element.SetValue(AttachBehaviorsProperty, value);
		}

		public static void SetBehaviorTypes (DependencyObject element, string value)
		{
			element.SetValue(BehaviorTypesProperty, value);
		}

		private static Behavior? FindExistingBehavior (BehaviorCollection behaviors, Type behaviorType)
		{
			return behaviors.FirstOrDefault(behavior => behavior.GetType() == behaviorType);
		}

		private static void OnAttachBehaviorsChanged (DependencyObject element, DependencyPropertyChangedEventArgs e)
		{
			if (!(element is UIElement uiElement))
			{
				// Log a warning or throw an exception
				return;
			}

			var behaviorTypesString = GetBehaviorTypes(element);
			if (string.IsNullOrEmpty(behaviorTypesString))
			{
				// Log a warning or throw an exception
				return;
			}

			var behaviorTypes = behaviorTypesString.Split(',')
												   .Select(type => Type.GetType(type.Trim()))
												   .Where(type => type != null && typeof(Behavior).IsAssignableFrom(type))
												   .ToList();

			if (!behaviorTypes.Any())
			{
				// Log a warning or throw an exception
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
						var newBehavior = BehaviorFactory.CreateBehavior(behaviorType);
						if (newBehavior != null)
						{
							behaviors.Add(newBehavior);
						}
						else
						{
							// Log a warning or throw an exception
						}
					}
					catch (Exception ex)
					{
						// Log the exception
					}
				}
			}
		}
	}
}