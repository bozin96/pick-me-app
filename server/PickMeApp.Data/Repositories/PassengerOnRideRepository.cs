using iComplyICO.Data;
using Microsoft.EntityFrameworkCore;
using PickMeApp.Application.Extensions;
using PickMeApp.Application.Helpers;
using PickMeApp.Application.Interfaces;
using PickMeApp.Application.Models.PassengerOnRideDtos;
using PickMeApp.Core.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PickMeApp.Application.Repositories
{
    public class PassengerOnRideRepository : IPassengerOnRideRepository
    {
        protected readonly ApplicationDbContext _dbContext;

        protected readonly DbSet<PassengerOnRide> _entities;

        private readonly IPropertyMappingService _propertyMappingService;


        public PassengerOnRideRepository(ApplicationDbContext context, IPropertyMappingService propertyMappingService)
        {
            _dbContext = context;
            _entities = context.Set<PassengerOnRide>();
            _propertyMappingService = propertyMappingService;
        }

        public async Task<PassengerOnRide> GetByIdAsync(Guid id)
        {
            return await _entities.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<PassengerOnRide> AddAsync(PassengerOnRide entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            await _entities.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<PagedList<PassengerOnRide>> ListAsync(PassengerOnRideResourceParameters resourceParameters)
        {
            if (resourceParameters == null)
            {
                throw new ArgumentNullException(nameof(resourceParameters));
            }

            var collection = _entities as IQueryable<PassengerOnRide>;

            if (!string.IsNullOrEmpty(resourceParameters.PassengerId))
            {
                collection = collection.Where(e => e.PassengerId == resourceParameters.PassengerId);
            }

            if (!string.IsNullOrWhiteSpace(resourceParameters.OrderBy))
            {
                // get property mapping dictionary
                var authorPropertyMappingDictionary =
                    _propertyMappingService.GetPropertyMapping<PassengerOnRideDto, PassengerOnRide>();

                collection = collection.ApplySort(resourceParameters.OrderBy, authorPropertyMappingDictionary);
            }

            return await PagedList<PassengerOnRide>.CreateAsync(collection,
                resourceParameters.PageNumber,
                resourceParameters.PageSize);
        }

        public async Task<bool> AddReviewAsync(Guid id, int review)
        {
            var entity = await _entities.FirstOrDefaultAsync(e => e.Id == id);
            if (entity == null)
                return false;
            if (review > 5 || review < 1)
                return false;

            entity.Review = review;
            _entities.Update(entity);
            await _dbContext.SaveChangesAsync();

            return true;
        }
    }
}
