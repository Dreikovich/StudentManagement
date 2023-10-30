using System.Data;
using System.Reflection;
namespace StudentManagementWebApi.DataAccess;
public class DataHelper
{
    public List<T> DataTableToList<T>(DataTable dataTable) where T : class, new()
    {
        List<T> list = new List<T>();
        foreach (DataRow row in dataTable.Rows)
        {
            T item = new T();
            object subModelObject = null; 
            bool isDataExist = false;
            var propertiesInModel = typeof(T).GetProperties();
            foreach (var modelProp in propertiesInModel)
            {
                if (modelProp.PropertyType.IsClass && modelProp.PropertyType != typeof(string))
                {
                    subModelObject = Activator.CreateInstance(modelProp.PropertyType);
                }
            }
            foreach (DataColumn column in dataTable.Columns)
            {
                var property = typeof(T).GetProperty(column.ColumnName);
                if (property != null && row[column] != DBNull.Value)
                {
                    property.SetValue(item, row[column], null);
                }
                else if (property == null && row[column] != DBNull.Value)
                {
                    isDataExist= true;
                    subModelObject.GetType().GetProperty(column.ColumnName)?.SetValue(subModelObject, row[column], null);
                }
            }

            if (isDataExist)
            {
                item.GetType().GetProperty(subModelObject?.GetType().Name)?.SetValue(item, subModelObject, null);  
            }
             
            list.Add(item);
        }
        return list;
    }
    
    public T DataTableToObject<T>(DataTable dataTable) where T : class, new()
    {
        if (dataTable == null || dataTable.Rows.Count == 0)
        {
            return null;
        }
        DataRow row = dataTable.Rows[0];
        T item = new T();
        foreach (DataColumn column in dataTable.Columns)
        {
            PropertyInfo property = typeof(T).GetProperty(column.ColumnName);
            if (property != null && row[column] != DBNull.Value)
            {
                
                property.SetValue(item, Convert.ChangeType(row[column], property.PropertyType));
            }
        }
        return item;
    }
    
    
}