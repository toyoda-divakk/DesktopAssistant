using System.Globalization;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;

namespace DesktopAssistant.Helpers;

/// <summary>
/// 列挙型をbool値に変換するコンバーター
/// Convertに関して、EnumToBooleanConverterとは逆の結果を出力する
/// </summary>
public class EnumToBooleanNegationConverter : IValueConverter
{
    public EnumToBooleanNegationConverter()
    {
    }
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (parameter is not string parameterString)
            return DependencyProperty.UnsetValue;

        if (!Enum.IsDefined(value.GetType(), value))
            return DependencyProperty.UnsetValue;

        var parameterValue = Enum.Parse(value.GetType(), parameterString);
        return !parameterValue.Equals(value);
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
