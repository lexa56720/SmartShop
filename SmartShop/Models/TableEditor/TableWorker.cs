using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Newtonsoft.Json.Linq;
using SmartShop.DataBase.Tables;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Reflection;

namespace SmartShop.Models.TableEditor
{
    public class TableWorker<T> : ITableWorker where T : class, new()
    {
        public string TableName { get; }

        public DbContext Context { get; }

        public ReadOnlyDictionary<string, string> Columns { get; }

        private List<IProperty> Properties = new();

        private Dictionary<Type, Func<string, object>> Converter = new()
        {
            { typeof(int),s=>int.Parse(s) },
            { typeof(float),s=>Convert.ToSingle(s, CultureInfo.InvariantCulture.NumberFormat)},
            { typeof(string),s=>s },
            { typeof(DateTime),s=>DateTime.Parse(s) },
                { typeof(OrderStatus),s=>Enum.Parse<OrderStatus>(s) },
        };


        public TableWorker(IEntityType tableEntity, DbContext context)
        {
            TableName = tableEntity.GetTableName();
            Columns = new ReadOnlyDictionary<string, string>(GetColumns(tableEntity));
            Context = context;
        }


        private Dictionary<string, string> GetColumns(IEntityType table)
        {
            var columns = new Dictionary<string, string>();
            Properties = new List<IProperty>(table.GetProperties());
            foreach (var property in Properties)
                columns.Add(property.GetColumnName(), property.GetColumnType());

            return columns;
        }
        public async Task<bool> UpdateRow(params string[] values)
        {
            var table = GetTable(Context);
            if (table == null)
                return false;
            var value = await table.FindAsync(int.Parse(values[0]));

            try
            {
                var result = Assign(value, values[1..]);
                if (result == null)
                    return false;
                return await Context.SaveChangesAsync() != 0;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> AddRow(params string[] values)
        {
            var value = Assign(null, values);
            var table = GetTable(Context);

            if (value == null || table == null ||
                !int.TryParse(values[0], out var id) || await table.FindAsync(id) != null)
                return false;

            try
            {
                var result = await table.AddAsync(value);
                return await Context.SaveChangesAsync() != 0;
            }
            catch
            {
                return false;
            }

        }

        public async Task<bool> DeleteRow(params string[] values)
        {
            var table = GetTable(Context);

            if (table == null || !int.TryParse(values[0], out var id))
                return false;

            var value = await table.FindAsync(id);
            if (value == null)
                return false;
            try
            {
                var result = table.Remove(value);
                return await Context.SaveChangesAsync() != 0;
            }
            catch
            {
                return false;
            }

        }

        public virtual async Task<string[]> GetAll()
        {
            var table = GetTable(Context);

            if (table == null)
                return Array.Empty<string>();

            return (await table.ToArrayAsync())
                               .SelectMany(ConvertToStrings)
                               .ToArray();
        }

        protected string[] ConvertToStrings(T value)
        {
            return Properties.Select(p =>
            {
                return p.PropertyInfo.GetValue(value).ToString();
            }).ToArray();
        }
        protected virtual DbSet<T>? GetTable(DbContext context)
        {
            return (DbSet<T>?)context.GetType()
                                    .GetProperties()
                                    .Where(p => p.PropertyType == typeof(DbSet<T>))
                                    .First()
                                    .GetValue(context);
        }


        protected T? Assign(T? obj, params string[] values)
        {
            obj ??= new T();

            var props = Properties.Select(p => p.PropertyInfo).ToArray();

            for (int i = 0; i < props.Length; i++)
            {
                var prop = props[i];
                var converter = GetType().GetMethod(nameof(StringToValue), BindingFlags.NonPublic | BindingFlags.Instance)
                                         .MakeGenericMethod(prop.PropertyType);
                try
                {
                    var convertedValue = converter.Invoke(this, new object[] { values[i] });
                    prop.SetValue(obj, convertedValue);
                }
                catch
                {
                    return null;
                }
            }

            return obj;
        }

        private PropertyInfo[] GetProperties()
        {
            return typeof(T).GetProperties()
                  .Where(p => !p.GetMethod.IsVirtual)
                  .ToArray();
        }
        private U? StringToValue<U>(string s)
        {
            try
            {
                return (U)Converter[typeof(U)](s);
            }
            catch
            {
                throw;
            }
        }
    }
}
