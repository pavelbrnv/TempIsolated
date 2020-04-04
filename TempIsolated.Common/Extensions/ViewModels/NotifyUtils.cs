using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;

namespace TempIsolated.Common.Extensions.ViewModels
{
	/// <summary>
	/// Notify helpers
	/// </summary>
	public static class NotifyUtils
	{
		#region Property helpers

		/// <summary>
		/// Helper for dynamic getting propertyName
		/// </summary>
		/// <typeparam name="TProperty">Property type</typeparam>
		/// <param name="propertyExpression">Property expression</param>
		/// <returns></returns>
		public static string GetPropertyName<TProperty>(Expression<Func<TProperty>> propertyExpression)
		{
			var property = propertyExpression.Body as MemberExpression;
			if (property == null || !(property.Member is PropertyInfo))
			{
				throw new ArgumentException(string.Format(
					CultureInfo.CurrentCulture,
					"Expression must be of the form 'someObject.PropertyName'. Invalid expression '{0}'.", propertyExpression),
					"propertyExpression");
			}
			return property.Member.Name;
		}

		/// <summary>
		/// Check is property helper
		/// </summary>
		/// <param name="o">Object</param>
		/// <param name="property">Expression</param>
		/// <returns>If expression corresponds to property</returns>
		public static bool IsPropertyOfThis(this object o, MemberExpression property)
		{
			var constant = RemoveCast(property.Expression) as ConstantExpression;
			return constant != null && constant.Value == o;
		}

		/// <summary>
		/// Removes cast from expression
		/// </summary>
		/// <param name="expression">Expression</param>
		/// <returns>Expression with removed casts</returns>
		private static Expression RemoveCast(Expression expression)
		{
			if (expression.NodeType == ExpressionType.Convert ||
				expression.NodeType == ExpressionType.ConvertChecked)
				return ((UnaryExpression)expression).Operand;

			return expression;
		}

		#endregion
	}

	/// <summary>
	/// Simple INotifyPropertyChanged implementation
	/// </summary>
	[Serializable]
	public class NotifyPropertyChanged : INotifyPropertyChanged
	{
		#region INotifyPropertyChanged implementation

		/// <summary>
		/// Property changed event
		/// </summary>
		[field: NonSerialized]
		public event PropertyChangedEventHandler PropertyChanged = (sender, args) => { };

		#endregion

		#region Set property helpers

		/// <summary>
		/// Sets property to new value
		/// </summary>
		/// <typeparam name="T">Property type</typeparam>
		/// <param name="x">Reference to property</param>
		/// <param name="value">New value</param>
		/// <param name="name">Property name</param>
		public void SetProperty<T>(ref T x, T value, string name)
		{
			if (x != null && x.Equals(value))
				return;
			if (x == null && value == null)
				return;
			x = value;
			RaisePropertyChanged(name);
		}

		/// <summary>
		/// Sets property to new value
		/// </summary>
		/// <typeparam name="T">Property type</typeparam>
		/// <param name="x">Reference to property</param>
		/// <param name="value">New value</param>
		/// <param name="propertyExpression">Property expression to get property name</param>
		public void SetProperty<T>(ref T x, T value, Expression<Func<T>> propertyExpression)
		{
			if (x != null && x.Equals(value))
				return;
			if (x == null && value == null)
				return;
			x = value;
			RaiseChanged(propertyExpression);
		}

		#endregion

		#region Get property name helpers


		#endregion

		#region Raise changed implementation

		/// <summary>
		/// Helper for more convenient raising of ProperyChanged event
		/// </summary>
		/// <typeparam name="TProperty">Property type</typeparam>
		/// <param name="propertyExpression">Property expression</param>
		protected void RaiseChanged<TProperty>(Expression<Func<TProperty>> propertyExpression)
		{
			var property = propertyExpression.Body as MemberExpression;
			if (property == null || !(property.Member is PropertyInfo) ||
				!this.IsPropertyOfThis(property))
			{
				throw new ArgumentException(string.Format(
					CultureInfo.CurrentCulture,
					"Expression must be of the form 'this.PropertyName'. Invalid expression '{0}'.",
					propertyExpression), "propertyExpression");
			}

			RaisePropertyChanged(property.Member.Name);
		}

		/// <summary>
		/// Raises PropertyChanged by property name
		/// </summary>
		/// <param name="propertyName">Property name</param>
		protected virtual void RaisePropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}

		#endregion
	}
}
