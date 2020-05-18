using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace employees.Model
{
    public class CardService { 
    private readonly ApplicationContext _applicationContext;

    public CardService(ApplicationContext applicationContext)
    {
        _applicationContext = applicationContext;
    }

    public List<Card> Get(string searchString, string sortBy, bool sortDirection,
        CardFilterDefinition filter, int limit, int offset)
    {
        var request = this._applicationContext.Cards.AsQueryable().Where(x=>x.DeletedAt == null);

        //if (filter.IsByEmployee)
        //    request = request.Where(x => x.EmployeeId == filter.EmployeeId);

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
        
        return request.OrderBy(x=>x.Id).Skip(offset).Take(limit).ToList();
    }

    public long GetCount(string searchString, CardFilterDefinition filter)
    {
        return this._applicationContext.Cards.Count();
    }

    public Card GetById(int id)
    {
        return this._applicationContext.Cards.Find(id);
    }

    public void Add(Card card)
    {
        _applicationContext.Cards.Add(card);
        _applicationContext.SaveChangesAsync();
    }
    public void Update(Card card)
    {
        _applicationContext.Entry(card).State = EntityState.Modified;
        _applicationContext.SaveChangesAsync();
    }

    public void Remove(int id)
    {
        var card = GetById(id);
        card.DeletedAt = DateTime.Now;
        _applicationContext.Entry(card).State = EntityState.Modified;
        _applicationContext.SaveChangesAsync();
    }
}
}
