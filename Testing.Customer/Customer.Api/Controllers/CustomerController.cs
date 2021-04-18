using System;
using System.Collections.Generic;
using Customer.Contract;
using Customer.Service;
using Microsoft.AspNetCore.Mvc;

namespace Customer.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        public ActionResult<IList<Domain.Customer>> Get()
        {
            return Ok(_customerService.GetAll());
        }

        [HttpGet("{id}")]
        public ActionResult<Domain.Customer> Get(Guid customerId)
        {
            return Ok(_customerService.GetById(customerId));
        }

        [HttpPost]
        public ActionResult Post([FromBody] CustomerDto customerDto)
        {
            _customerService.CreateNew(customerDto);
            return Ok();
        }

        [HttpPut]
        public ActionResult<CustomerDto> Put([FromBody] CustomerDto customerDto)
        {
            return Ok(_customerService.Update(customerDto));
        }

        [HttpGet("getByCityCode/{cityCode}")]
        public ActionResult<IList<CustomerDto>> GetByCityCode(string cityCode)
        {
            return Ok(_customerService.GetByCityCode(cityCode));
        }
    }
}