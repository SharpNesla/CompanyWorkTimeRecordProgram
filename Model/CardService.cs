using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace employees.Model
{
    class CardService { 
    private readonly ApplicationContext _applicationContext;

    public CardService(ApplicationContext applicationContext)
    {
        _applicationContext = applicationContext;
    }

    public async Task<List<Card>> Get(string searchString, string sortBy, bool sortDirection,
        CardFilterDefinition filter, int limit, int offset)
    {
        var request = this._applicationContext.Cards.AsQueryable();

        if (filter.IsByEmployee)
            request = request.Where(x => x.EmployeeId == filter.EmployeeId);

        if (filter.IsBySumWorkTime)
        {
            request = request.Where(x => x.SumWorkLoadTime >= filter.SumWorkTimeLowBound &&
                                         x.SumWorkLoadTime <= filter.SumWorkTimeHighBound);
        }

        if (filter.IsByDatePass)
        {
            request = request.Where(x => x.DatePass >= filter.DatePassLowBound &&
                                         x.DatePass <= filter.DatePassHighBound);
        }
        
        return await request.Skip(offset).Take(limit).ToListAsync();
    }

    public async Task<Card> GetById(int id)
    {
        return await this._applicationContext.Cards.FindAsync(id);
    }

    public async void Add(Card card)
    {
        _applicationContext.Cards.Add(card);
        await _applicationContext.SaveChangesAsync();
    }
    public async void Update(Card card)
    {
        _applicationContext.Entry(card).State = EntityState.Modified;
        await _applicationContext.SaveChangesAsync();
    }

    public async void Remove(Card employee)
    {
        employee.DeletedAt = DateTime.Now;
        _applicationContext.Entry(employee).State = EntityState.Modified;
        await _applicationContext.SaveChangesAsync();
    }
}
}
