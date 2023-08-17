using SQLite;
using Diner.Models;
namespace Diner.Models;

public class Data
{
    SQLiteAsyncConnection Database;
    public Data()
    {
    }
    async Task Init()
    {
        if (Database is not null)
            return;

        Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        var result = await Database.CreateTableAsync<BusinessList>();
    }

    public async Task<List<BusinessList>> GetItemsAsync()
    {
        await Init();
        try
        {
            return await Database.Table<BusinessList>().ToListAsync();
        }
        catch(Exception e)
        {
            return null;
        }
    }

  /*  public async Task<List<BusinessList>> GetItemsNotDoneAsync()
    {
        await Init();
        return await Database.Table<BusinessList>().Where(t => t.Done).ToListAsync();

        // SQL queries are also possible
        //return await Database.QueryAsync<TodoItem>("SELECT * FROM [TodoItem] WHERE [Done] = 0");
    }*/

    public async Task<BusinessList> GetItemAsync(int id)
    {
        await Init();
        return await Database.Table<BusinessList>().Where(i => i.ID == id).FirstOrDefaultAsync();
    }

    public async Task<int> SaveItemAsync(BusinessList item)
    {
        await Init();
        if (item.ID != 0)
        {
            return await Database.UpdateAsync(item);
        }
        else
        {
            return await Database.InsertAsync(item);
        }
    }

    public async Task<int> DeleteItemAsync(BusinessList item)
    {
        await Init();
        return await Database.DeleteAsync(item);
    }
}

