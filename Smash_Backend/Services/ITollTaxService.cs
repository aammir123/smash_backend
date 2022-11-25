using Smash_Backend.Entities;
using Smash_Backend.RequestModels;
using Smash_Backend.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Smash_Backend.Services
{
    public interface ITollTaxService
    {
        public Task<EntryExitTransactions> TollTaxEntry(EntryExitRequest entryExitRequest);
        public Task<TollTax> TollTaxExit(EntryExitRequest entryExitRequest);
        public Task<List<Interchanges>> GetInterchanges();

    }
}
