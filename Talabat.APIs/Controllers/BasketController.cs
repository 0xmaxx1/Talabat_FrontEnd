using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Talabat.APIs.Errors;
using Talabat.Core.DTOs;
using Talabat.Core.Models.Basket;
using Talabat.Core.Repositories;

namespace Talabat.APIs.Controllers
{
    public class BasketController : ApiBaseController
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;

        public BasketController(IBasketRepository basketRepository, IMapper mapper)
        {
            _basketRepository = basketRepository;
            _mapper = mapper;
        }


        #region Get My Cart
        [HttpGet] // GET: api/baskets?id=
        public async Task<ActionResult<CustomerBasket>> GetBasket([FromQuery] string id)
        {
            var basket = await _basketRepository.GetBasketAsync(id);

            if (basket is null)
            {
                basket = new CustomerBasket(id); // Create a new basket if the last one was deleted
                return basket;
            }
            else
                return basket;

            //return basket ?? new CustomerBasket(id);
        }
        #endregion


        #region Update Basket
        [HttpPost] // POST: api/basket
        // Update or Create for the first time
        public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasketDto basket) // We made customerBasketDto for Validation only
        {
            CustomerBasket mappedBasket = _mapper.Map<CustomerBasketDto, CustomerBasket>(basket);

            CustomerBasket? createdOrUpdatedBasket = await _basketRepository.UpdateBasketAsync(mappedBasket);

            if (createdOrUpdatedBasket is null)
                return BadRequest(new ApiResponse(400, "An Error With Your Basket"));
            else
                return createdOrUpdatedBasket;
        }
        #endregion


        #region Delete Basket
        [HttpDelete] // DELETE: api/baskets
        public async Task<ActionResult<bool>> DeleteBasket([FromQuery] string id)
        {
            return await _basketRepository.DeleteBasketAsync(id);
        }
        #endregion

    }
}
