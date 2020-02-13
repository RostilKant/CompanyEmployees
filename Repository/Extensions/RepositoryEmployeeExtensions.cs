using System.Linq;
using Entities.Models;

namespace Repository.Extensions
{
    public static class RepositoryEmployeeExtensions
    {
        public static IQueryable<Employee> FilterEmployees(this IQueryable<Employee> employees, uint minAge,
            uint maxAge) =>
            employees.Where(e => (e.Age <= maxAge && e.Age >= minAge));

        public static IQueryable<Employee> Search(this IQueryable<Employee> employees, string term) =>
            string.IsNullOrEmpty(term)
                ? employees
                : employees.Where(e => e.Name.ToLower().Contains(term.Trim().ToLower()));
    }
}