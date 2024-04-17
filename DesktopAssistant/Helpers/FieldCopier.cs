using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DesktopAssistant.Helpers;

/// <summary>
/// フィールドをコピーするヘルパークラス
/// </summary>
public static class FieldCopier
{
    /// <summary>
    /// インタフェースに含まれるプロパティのみをコピーする
    /// </summary>
    /// <typeparam name="T">インタフェース</typeparam>
    /// <param name="source">コピー元オブジェクト</param>
    /// <param name="destination">コピー先オブジェクト</param>
    public static void CopyProperties<T>(object source, object destination)
    where T : class
    {
        var interfaceProperties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var interfaceProperty in interfaceProperties)
        {
            var sourceProperty = source.GetType().GetProperty(interfaceProperty.Name);
            var destinationProperty = destination.GetType().GetProperty(interfaceProperty.Name);

            if (sourceProperty != null && destinationProperty != null
                && sourceProperty.PropertyType == destinationProperty.PropertyType)
            {
                var value = sourceProperty.GetValue(source);
                destinationProperty.SetValue(destination, value);
            }
        }
    }
}
