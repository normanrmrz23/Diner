using System;
using Yelp.Api.Models;

namespace Diner.Services
{
    public interface IListLoader
    {
        Task<OkFailResult> LoadAsync(string filename, BusinessResponse business);
    }

    public class ListLoader : IListLoader
    {
        public ListLoader()
        {
        }

        public async Task<OkFailResult> LoadAsync(string filename, BusinessResponse business)
        {
            var retCode = OkFailResult.Ok();
            return retCode;
        }
    }
}
