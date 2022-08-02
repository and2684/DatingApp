using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace API.Helpers
{
    public class PagedList<T> : List<T>
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }

        public PagedList(IEnumerable<T> items, int count, int pageNumber, int pageSize)
        {
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize); // Сколько всего страниц? Делим общее кол-во записей на размер одной страницы
            PageSize = pageSize;
            TotalCount = count;
            AddRange(items);
        }

        public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = await source.CountAsync(); // Считаем сколько всего записей по нашему запросу в бд
            var items = await source
                              .Skip(pageSize * (pageNumber - 1))
                              .Take(pageSize)
                              .ToListAsync(); // Выводим нужную страницу в List<T>

            return new PagedList<T>(items, count, pageNumber, pageSize); // Возвращаем PagedList созданный на основе полученного List<T>
        }
    }
}