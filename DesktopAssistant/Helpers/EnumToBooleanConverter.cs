using System.Globalization;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;

namespace DesktopAssistant.Helpers;

/// <summary>
/// 列挙型をbool値に変換するコンバーター
/// </summary>
public class EnumToBooleanConverter : IValueConverter
{
    public EnumToBooleanConverter()
    {
    }
    // デフォルトのこの実装はダメ。ElementThemeにしか対応していない。

    //public object Convert(object value, Type targetType, object parameter, string language)
    //{
    //    if (parameter is string enumString)
    //    {
    //        if (!Enum.IsDefined(typeof(ElementTheme), value))
    //        {
    //            throw new ArgumentException("ExceptionEnumToBooleanConverterValueMustBeAnEnum");
    //        }

    //        var enumValue = Enum.Parse(typeof(ElementTheme), enumString);

    //        return enumValue.Equals(value);
    //    }

    //    throw new ArgumentException("ExceptionEnumToBooleanConverterParameterMustBeAnEnumName");
    //}

    //public object ConvertBack(object value, Type targetType, object parameter, string language)
    //{
    //    if (parameter is string enumString)
    //    {
    //        return Enum.Parse(typeof(ElementTheme), enumString);
    //    }

    //    throw new ArgumentException("ExceptionEnumToBooleanConverterParameterMustBeAnEnumName");
    //}
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (parameter is not string parameterString)
            return DependencyProperty.UnsetValue;

        if (!Enum.IsDefined(value.GetType(), value))
            return DependencyProperty.UnsetValue;

        var parameterValue = Enum.Parse(value.GetType(), parameterString);
        return parameterValue.Equals(value);
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        if (parameter is not string parameterString)
            return DependencyProperty.UnsetValue;

        if (true.Equals(value))
            return Enum.Parse(targetType, parameterString);
        else
            return DependencyProperty.UnsetValue;
    }

}


/// <summary>
/// 任意の型の2つの値が等しければtrueを返すコンバーター
/// </summary>
public class EqualityToBooleanConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        return value?.ToString()!.Equals(parameter.ToString()) ?? false;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        return value is true ? parameter : DependencyProperty.UnsetValue;   // ※ConvertBackは使っていないので、UnsetValueが正しいかは検証していない
    }
}