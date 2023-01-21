using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Talabat.APIs.Dtos;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;

namespace Talabat.APIs.Controllers
{
    
    public class BasketController : BaseApiController
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;

        public BasketController(IBasketRepository basketRepository , IMapper mapper)
        {
            _basketRepository = basketRepository;
            _mapper = mapper;
        }

        [HttpGet] // Get : /api/Basket/1
        public async Task<ActionResult<CustomerBasket>> GetBasketById(string id)
        {
            var basket = await _basketRepository.GetBasketAsync(id);
            return Ok(basket ?? new CustomerBasket(id));
        }

        [HttpPost] // Post : /api/Basket
        public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasketDto basket)
        {
            var mapperBasket = _mapper.Map<CustomerBasketDto, CustomerBasket>(basket);

            var updatedOrCreatedBasket = await _basketRepository.UpdateBasketAsync(mapperBasket);
            return Ok(updatedOrCreatedBasket);
        }

        [HttpDelete] // Delete : /api/Basket
        public async Task  DeleteBasket(string id)
        {
           await _basketRepository.DeleteBasketAsync(id);
        }

    }
}
