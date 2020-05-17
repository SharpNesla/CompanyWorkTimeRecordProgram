using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using employees.Annotations;
using Employees.Model;
using LiveCharts.Dtos;

namespace employees.Model
{
    public class EmployeeService
    {
        private readonly ApplicationContext _applicationContext;
        
        public EmployeeService(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }

        public async Task<List<Employee>> Get(string searchString, string sortBy, bool sortDirection,
            EmployeeFilterDefinition filter, int limit, int offset)
        {
            var request = this._applicationContext.Employees.AsQueryable();

            if (filter.IsByGender)
                request = request.Where(x => x.Gender == filter.Gender);

            if (filter.IsByDateBirth)
            {
                request = request.Where(x => x.DateBirth >= filter.DateBirthLowBound &&
                                             x.DateBirth <= filter.DateBirthHighBound);
            }

            //if (filter.IsBySumWorkTime)
            //{
            //    request = request.Where(x => x.SumWorkTime >= filter.SumWorkTimeLowBound &&
            //                                 x.SumWorkTime <= filter.SumWorkTimeHighBound);
            //}

            if (filter.IsByRole)
                request = request.Where(x => x.Role == filter.Role);

            return await request.OrderBy(x => x.Id).Skip(offset).Take(limit).ToListAsync();
        }

        public async Task<Employee> GetById(int id)
        {
            return await this._applicationContext.Employees.FindAsync(id);
        }

        public async void Add(Employee employee)
        {
            _applicationContext.Employees.Add(employee);
            await _applicationContext.SaveChangesAsync();
        }
        public async void Update(Employee employee)
        {
            _applicationContext.Entry(employee).State = EntityState.Modified;
            await _applicationContext.SaveChangesAsync();
        }

        public async void Remove(Employee employee)
        {
            employee.DeletedAt = DateTime.Now;
            _applicationContext.Entry(employee).State = EntityState.Modified;
            await _applicationContext.SaveChangesAsync();
        }
    }
}
