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

            // Отримайте значення кожної властивості та об'єднайте їх у рядок
            string serializedString = string.Join("_", properties
                .Where(p => p.Name != "Image" && p.Name != "Id") // Додайте інші властивості, які ви хочете виключити
                .Select(p => $"{p.Name}:{p.GetValue(obj)?.ToString()}"));

            return serializedString;
        }

        public T DesialazeFile<T>(string serializedData)
        {
            // Розділіть рядок за допомогою роздільника "_"
            string[] propertyNames = serializedData.Split('_');

            // Створіть об'єкт типу T
            T obj = Activator.CreateInstance<T>();

            // Отримайте всі властивості об'єкта
            var properties = typeof(T).GetProperties();

            // Присвойте значення властивостям з розділеного рядка

            foreach(var propertyName in propertyNames)
            {
                string[] data = propertyName.Split(":");

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
                    // Додайте інші типи даних за потреби
                    default:
                        // Обробка невідомого типу даних (за власним бажанням)
                        break;
                }


            }


            //for (int i = 0; i < Math.Min(properties.Length, propertyNames.Length); i++)
            //{
            //    TypeCode propertyTypeCode = Type.GetTypeCode(properties[i].PropertyType);

               

            //}

            return obj;
        }
    }
}
