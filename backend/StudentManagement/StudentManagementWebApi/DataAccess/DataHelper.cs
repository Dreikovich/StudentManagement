using System.Data;
using System.Reflection;

namespace StudentManagementWebApi.DataAccess;

public class DataHelper
{
    public List<T> DataTableToList<T>(DataTable dataTable) where T : class, new()
    {
        var list = new List<T>();


        foreach (DataRow row in dataTable.Rows)
        {
            var addedSubModelTypes = new HashSet<Type>();
            var subModelList = new List<object>();
            var item = new T();
            var isDataExist = false;
            var propertiesInModel = typeof(T).GetProperties();

            foreach (var modelProp in propertiesInModel)
            {
                var subModelObject = CreateSubModelInstance(modelProp);
                if (subModelObject != null && !addedSubModelTypes.Contains(subModelObject.GetType()))
                {
                    addedSubModelTypes.Add(subModelObject.GetType());
                    subModelList.Add(subModelObject);
                }
            }

            foreach (DataColumn column in dataTable.Columns)
            {
                var property = typeof(T).GetProperty(column.ColumnName);
                if (property == null && row[column] != DBNull.Value)
                    isDataExist |= UpdateSubModel(subModelList, item, row[column], column.ColumnName);
                else if (property != null && row[column] != DBNull.Value) property.SetValue(item, row[column], null);
            }

            list.Add(item);
        }

        return list;
    }

    private object CreateSubModelInstance(PropertyInfo modelProp)
    {
        if (modelProp.PropertyType.IsClass && modelProp.PropertyType != typeof(string))
            return Activator.CreateInstance(modelProp.PropertyType);
        return null;
    }

    private bool UpdateSubModel(List<object> subModelList, object item, object columnValue, string columnName)
    {
        var isDataExist = false;


        foreach (var subModel in subModelList)
        {
            var subModelProperty = subModel.GetType().GetProperty(columnName);
            if (subModelProperty != null && subModelProperty.PropertyType == columnValue.GetType())
            {
                isDataExist = true;
                subModelProperty.SetValue(subModel, columnValue, null);
                item.GetType().GetProperty(subModel.GetType().Name)?.SetValue(item, subModel, null);
                break;
            }
        }

        return isDataExist;
    }


    public T DataTableToObject<T>(DataTable dataTable) where T : class, new()
    {
        if (dataTable == null || dataTable.Rows.Count == 0) return null;
        var row = dataTable.Rows[0];
        var item = new T();
        foreach (DataColumn column in dataTable.Columns)
        {
            var property = typeof(T).GetProperty(column.ColumnName);
            if (property != null && row[column] != DBNull.Value)
                property.SetValue(item, Convert.ChangeType(row[column], property.PropertyType));
        }

        return item;
    }
}