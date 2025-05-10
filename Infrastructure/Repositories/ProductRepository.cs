using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Exceptions.Product;
using Application.Interfaces;
using Application.Models;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _appContext;
        private readonly IMapper _mapper;

        public ProductRepository(AppDbContext appContext, IMapper mapper)
        {
            _appContext = appContext;
            _mapper = mapper;
        }
        ///<inheritdoc/>
        public async Task<ProductDTO?> AddAsync(ProductDTO item, CancellationToken cancellation)
        {
            var entity = _mapper.Map<Product>(item);

            var result = await _appContext.Products.AddAsync(entity);
           
            await _appContext.SaveChangesAsync(cancellation);
                
            return _mapper.Map<ProductDTO?>(entity);
        }
        
        ///<inheritdoc/>
        public async Task<ProductDTO?> DeleteAsync(int id, CancellationToken cancellation)
        {
            var entity =  await _appContext.Products.FindAsync(id, cancellation);

            var result = _appContext.Products.Remove(entity!);
            await _appContext.SaveChangesAsync(cancellation);

            return _mapper.Map<ProductDTO>(entity);
        }

        ///<inheritdoc/>
        public async Task<List<ProductDTO?>> GetAllAsync(CancellationToken cancellation)
        {
            var products = await _appContext.Products.ToListAsync(cancellation);

            return products.Select(x => _mapper.Map<ProductDTO?>(x)).ToList();
        }
        
        ///<inheritdoc/>
        public async Task<ProductDTO?> GetByIdAsync(int id, CancellationToken cancellation)
        {
            var entity = await  _appContext.Products.FirstOrDefaultAsync(x => x.Id == id);

            return _mapper.Map<ProductDTO?>(entity);
        }

        ///<inheritdoc/>
        public async Task<ProductDTO?> UpdateAsync(ProductDTO item, CancellationToken cancellation)
        {
            var entity = _mapper.Map<Product>(item);

            _appContext.Products.Update(entity);
            await _appContext.SaveChangesAsync(cancellation);

            return _mapper.Map<ProductDTO?>(entity);
        }
    }
}
