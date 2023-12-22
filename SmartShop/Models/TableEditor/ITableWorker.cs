using System.Collections.ObjectModel;

namespace SmartShop.Models.TableEditor
{
    public interface ITableWorker
    {
        public string TableName { get; }
        public ReadOnlyDictionary<string, string> Columns { get; }
        public Task<bool> UpdateRow(params string[] values);

        public Task<bool> DeleteRow(params string[] values);

        public Task<bool> AddRow(params string[] values);

        public Task<string[]> GetAll();
    }
}
