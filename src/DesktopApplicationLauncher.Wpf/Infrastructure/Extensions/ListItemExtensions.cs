namespace DesktopApplicationLauncher.Wpf.Infrastructure.Extensions
{
    using System;
    using System.Collections.Generic;

    using DesktopApplicationLauncher.Wpf.Infrastructure.Models;

    public static class ListItemExtensions
    {
        public static IList<ListItemModel<TEnumValue>> GetListItemsOfEnum<TEnumValue>()
        {
            var enumType = typeof(TEnumValue);
            if (!enumType.IsEnum)
            {
                throw new NotSupportedException("Type must be enum.");
            }

            var result = new List<ListItemModel<TEnumValue>>();

            var names = Enum.GetNames(enumType);
            for (var i = 0; i < names.Length; i++)
            {
                var name = names[i];
                result.Add(new ListItemModel<TEnumValue> { Text = name, Value = (TEnumValue)Enum.Parse(enumType, name) });
            }

            return result;
        }
    }
}
