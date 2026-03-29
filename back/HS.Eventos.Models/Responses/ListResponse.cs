using System;
using System.Collections.Generic;
using System.Text;

namespace HS.Eventos.Models.Responses
{
    public class ListResponse<T>
    {
        public IEnumerable<T> Items { get; set; } = [];
        public string PaginationToken { get; set; } = string.Empty;

        public ListResponse<S> MapperTo<S>(Func<T, S> mapper)
        {
            return new ListResponse<S>
            {
                Items = Items.Select(mapper),
                PaginationToken = PaginationToken
            };
        }
    }
}
