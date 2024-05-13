using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CashManager.Serialazer
{
    internal class FileSerializer
    {

        public string SerialazerFile<T>(T obj)
        {
            Type type = typeof(T);
            PropertyInfo[] properties = type.GetProperties();


            string serializedString = string.Join("_", properties
                .Where(p => p.Name != "Image") 
                .Select(p => $"{p.Name}&{p.GetValue(obj)?.ToString()}"));

            return serializedString;
        }

        public T DesialazeFile<T>(string serializedData)
        {
            string[] propertyNames = serializedData.Split('_');
            T obj = Activator.CreateInstance<T>();
            var properties = typeof(T).GetProperties();

            foreach(var propertyName in propertyNames)
            {
                string[] data = propertyName.Split("&");

                PropertyInfo curentProperty = properties.Where(p => p.Name == data[0]).First();

                TypeCode propertyTypeCode = Type.GetTypeCode(curentProperty.PropertyType);

                switch (propertyTypeCode)
                {
                    case TypeCode.Decimal:
                        curentProperty.SetValue(obj, decimal.Parse(data[1]));
                        break;
                    case TypeCode.Boolean:
                        curentProperty.SetValue(obj, bool.Parse(data[1]));
                        break;
                    case TypeCode.String:
                        curentProperty.SetValue(obj, data[1]);
                        break;
                    case TypeCode.DateTime:
                        curentProperty.SetValue(obj, DateTime.Parse(data[1]));
                        break ;
                    case TypeCode:Int32:
                        curentProperty.SetValue(obj, int.Parse(data[1]));
                        break;
                    default:
                        
                        break;
                }


            }
           
            return obj;
        }
    }
}
