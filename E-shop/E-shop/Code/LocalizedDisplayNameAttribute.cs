using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Web;

namespace E_shop.Code
{
    //Локализация значений Data Annotations
    //Localization values ​​Data Annotations
    public class LocalizedDisplayNameAttribute : DisplayNameAttribute
    {
        private PropertyInfo _nameProperty;
        private Type _resourceType;

        public LocalizedDisplayNameAttribute(string displayNameKey): base(displayNameKey) { }

        public Type NameResourceType
        {
            get
            {
                return _resourceType;
            }
            set
            {
                _resourceType = value;
                //инициализация nameProperty, когда тип свойства устанавливается set'ром
                //Initialization nameProperty, when the type of property is set
                _nameProperty = _resourceType.GetProperty(base.DisplayName, BindingFlags.Static | BindingFlags.Public);
            }
        }

        public override string DisplayName
        {
            get
            {
                //проверяет,nameProperty null и возвращает исходный значения отображаемого имени
                //checks, nameProperty null and returns the original value of the display name
                if (_nameProperty == null)
                {
                    return base.DisplayName;
                }

                return (string)_nameProperty.GetValue(_nameProperty.DeclaringType, null);
            }
        }
    }
}