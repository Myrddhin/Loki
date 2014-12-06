using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Loki.Common
{
    /// <summary>
    /// A small helper class that has a method to help create
    /// PropertyChangedEventArgs when using the INotifyPropertyChanged
    /// interface.
    /// </summary>
    public static class ObservableHelper
    {
        #region Public Methods

        /// <summary>
        /// Creates PropertyChangedEventArgs.
        /// </summary>
        /// <typeparam name="T">Type used to get the property name.</typeparam>
        /// <param name="propertyExpression">Expression to make PropertyChangedEventArgs out of.</param>
        /// <returns>PropertyChangedEventArgs for the specified property.</returns>
        public static PropertyChangedEventArgs CreateChangedArgs<T>(Expression<Func<T, object>> propertyExpression)
        {
            return new PropertyChangedEventArgs(ExpressionHelper.GetProperty(propertyExpression).Name);
        }

        /// <summary>
        /// Creates the PropertyChangingEventArgs.
        /// </summary>
        /// <typeparam name="T">Type used to get the property name.</typeparam>
        /// <param name="propertyExpression">Expression to make PropertyChangingEventArgs out of.</param>
        /// <returns>PropertyChangingEventArgs for the specified property.</returns>
        public static PropertyChangingEventArgs CreateChangingArgs<T>(Expression<Func<T, object>> propertyExpression)
        {
            return new PropertyChangingEventArgs(ExpressionHelper.GetProperty(propertyExpression).Name);
        }

        #endregion Public Methods
    }
}