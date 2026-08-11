using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using System.Collections.Generic;
using System.Linq;

namespace GHM.Infrastructure.SearchRemote
{
    public static class HandlerSearchResult
    {
        public static LoadResult SearchResult<T>(List<T> data, DataSourceLoadOptions loadOptions)
        {
            loadOptions.RequireTotalCount = true;
            var result = DataSourceLoader.Load(data.AsQueryable(), loadOptions);
            return result;

        }

    }
}
