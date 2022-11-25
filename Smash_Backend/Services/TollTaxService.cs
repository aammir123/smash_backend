using Microsoft.EntityFrameworkCore;
using Smash_Backend.Entities;
using Smash_Backend.RequestModels;
using Smash_Backend.Responses;
using System.Linq;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Smash_Backend.Services
{
    public class TollTaxService : ITollTaxService
    {
        private readonly SmashDBContext _db;
        private const decimal _baseRate = 20;
        private const decimal _ratePerKm = 0.2M;
        private const decimal _weekendRate = 1.5M;
        private const decimal _discount = 10;
        private const decimal _specialDiscount = 50;
        public TollTaxService(SmashDBContext smashDBContext)
        {
            _db = smashDBContext;
        }

        public async Task<EntryExitTransactions> TollTaxEntry(EntryExitRequest entryExitRequest)
        {
            EntryExitTransactions entry = new EntryExitTransactions();
            entry.NumberPlate = entryExitRequest.numberPlate;
            entry.EntryInterchangeId = entryExitRequest.interchangeId;
            entry.EntryDateTime = entryExitRequest.transactionDateTime;

            await _db.EntryExitTransactions.AddAsync(entry);
            await _db.SaveChangesAsync();

            return entry;
        }

        public async Task<TollTax> TollTaxExit(EntryExitRequest entryExitRequest)
        {
            TollTax tollTax = new TollTax();
            var entry = await _db.EntryExitTransactions.Where(e => e.NumberPlate == entryExitRequest.numberPlate && e.ExitDateTime == null).FirstOrDefaultAsync();
            if (entry != null)
            {
                entry.NumberPlate = entryExitRequest.numberPlate;
                entry.ExitInterchangeId = entryExitRequest.interchangeId;
                entry.ExitDateTime = entryExitRequest.transactionDateTime;

                _db.EntryExitTransactions.Update(entry);
                await _db.SaveChangesAsync();

                var Interchanges = await _db.Interchanges.ToListAsync();
                var entryPoint = Interchanges.Where(e => e.Id == entry.EntryInterchangeId).FirstOrDefault().Distance;
                var exitPoint = Interchanges.Where(e => e.Id == entry.ExitInterchangeId).FirstOrDefault().Distance;

                var totalDistance = Math.Abs(exitPoint - entryPoint);
                decimal totalRs = 0;
                decimal distanceCost = 0;
                decimal other = 0;

                if(IsPublicHoliday(entry.ExitDateTime.Value))
                {
                    distanceCost = (_ratePerKm * totalDistance);
                    other = ((_specialDiscount / 100) * (_baseRate + distanceCost));
                    totalRs = (_baseRate + distanceCost) - other;
                }    
                else
                {
                    var numPlate = Convert.ToInt32(entry.NumberPlate.Split('-').ToArray()[1]);
                    bool isEven = ((numPlate % 2) == 0 ? true : false);
                    var dayOfWeek = entry.EntryDateTime.Value.DayOfWeek;

                    switch (dayOfWeek)
                    {
                        case System.DayOfWeek.Sunday:
                            distanceCost = (_ratePerKm * totalDistance);
                            other = (_weekendRate * (_baseRate + distanceCost));
                            totalRs = _baseRate + distanceCost + other;
                            break;
                        case System.DayOfWeek.Monday:
                            if (isEven)
                            {
                                distanceCost = (_ratePerKm * totalDistance);
                                other = ((_discount / 100) * (_baseRate + distanceCost));
                                totalRs = (_baseRate + distanceCost) - other;
                            }
                            else
                            {
                                distanceCost = (_ratePerKm * totalDistance);
                                totalRs = _baseRate + distanceCost;
                            }
                            break;
                        case System.DayOfWeek.Tuesday:
                            if (!isEven)
                            {
                                distanceCost = (_ratePerKm * totalDistance);
                                other = ((_discount / 100) * (_baseRate + distanceCost));
                                totalRs = (_baseRate + distanceCost) - other;
                            }
                            else
                            {
                                distanceCost = (_ratePerKm * totalDistance);
                                totalRs = _baseRate + distanceCost;
                            }
                            break;
                        case System.DayOfWeek.Wednesday:
                            if (isEven)
                            {
                                distanceCost = (_ratePerKm * totalDistance);
                                other = ((_discount / 100) * (_baseRate + distanceCost));
                                totalRs = (_baseRate + distanceCost) - other;
                            }
                            else
                            {
                                distanceCost = (_ratePerKm * totalDistance);
                                totalRs = _baseRate + distanceCost;
                            }
                            break;
                        case System.DayOfWeek.Thursday:
                            if (!isEven)
                            {
                                distanceCost = (_ratePerKm * totalDistance);
                                other = ((_discount / 100) * (_baseRate + distanceCost));
                                totalRs = (_baseRate + distanceCost) - other;
                            }
                            else
                            {
                                distanceCost = (_ratePerKm * totalDistance);
                                totalRs = _baseRate + distanceCost;
                            }
                            break;
                        case System.DayOfWeek.Friday:
                            distanceCost = (_ratePerKm * totalDistance);
                            totalRs = _baseRate + distanceCost;
                            break;
                        case System.DayOfWeek.Saturday:
                            distanceCost = (_ratePerKm * totalDistance);
                            other = (_weekendRate * (_baseRate + distanceCost));
                            totalRs = _baseRate + distanceCost + other;
                            break;
                        default:
                            distanceCost = (_ratePerKm * totalDistance);
                            totalRs = _baseRate + distanceCost;
                            break;
                    }
                }
               
                tollTax = new TollTax();
                tollTax.baseRate = _baseRate;
                tollTax.totalDistance = totalDistance;
                tollTax.totalDistanceCost = distanceCost;
                tollTax.subTotal = (distanceCost + _baseRate);
                tollTax.discounts = other;
                tollTax.netTax = totalRs;
                return tollTax;
            }
            return new TollTax();
        }

        private bool IsPublicHoliday(DateTime date)
        {
            List<DateTime> holidays = new List<DateTime>() { new DateTime(date.Year, 3,23), new DateTime(date.Year, 8, 14), new DateTime(date.Year, 12, 25) };
            return (holidays.Exists(e => e.Date.Date == date.Date));
        }
        public async Task<List<Interchanges>> GetInterchanges()
        {
            var Interchanges = await _db.Interchanges.ToListAsync();
            return Interchanges;
        }
    }
}
