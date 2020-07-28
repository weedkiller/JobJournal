﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using JobJournal.Server.Data;
using JobJournal.Shared;
using JobJournal.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobJournal.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api")]
    public class CompanyContactController : ControllerBase
    {
        private readonly ICompanyContactRepository _repository;
        private readonly IMapper _mapper;

        public CompanyContactController(ICompanyContactRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // GET: api/company/AD94A572-5104-4303-82F7-FAC0A7D06897/contacts
        [HttpGet("company/{id:Guid}/contacts")]
        public async Task<ActionResult<IEnumerable<CompanyContactDTO>>> GetCompanyContacts(Guid id)
        {
            try
            {
                return Ok(await _mapper.ProjectTo<CompanyContactDTO>(_repository.GetContactsForCompany(id)).ToListAsync());
            }
            catch
            {
                // TODO: Log exception
                return BadRequest();
            }
        }

        // GET api/companycontact/1E98B65B-C56D-470D-B509-148AC693A013
        [HttpGet("companycontact/{id:Guid}")]
        public ActionResult<CompanyContactDTO> GetCompanyContact(Guid id)
        {
            try
            {
                var contact = _repository.GetCompanyContact(id);
                return Ok(_mapper.Map<CompanyContactDTO>(contact));
            }
            catch
            {
                // TODO: Log exception
                return NotFound();
            }
        }

        // POST api/companycontact
        [HttpPost("companycontact")]
        public async Task<ActionResult> AddCompanyContact([FromBody] CompanyContactDTO contactDTO)
        {
            try
            {
                var contact = _mapper.Map<CompanyContact>(contactDTO);
                var newContact = await _repository.AddCompanyContact(contact);

                return CreatedAtAction(nameof(GetCompanyContact), new { id = newContact.Id }, _mapper.Map<CompanyContactDTO>(newContact));
            }
            catch
            {
                // TODO: Log exception
                return BadRequest();
            }
        }

        // PUT api/companycontact/1E98B65B-C56D-470D-B509-148AC693A013
        [HttpPut("companycontact/{id:Guid}")]
        public async Task<ActionResult> UpdateCompanyContact(Guid id, [FromBody] CompanyContactDTO contactDTO)
        {
            if (id != contactDTO.Id)
            {
                return BadRequest();
            }

            try
            {
                var contact = _mapper.Map<CompanyContact>(contactDTO);
                await _repository.UpdateCompanyContact(contact);

                return NoContent();
            }
            catch
            {
                // TODO: Log exception
                return NotFound();
            }
        }

        // DELETE api/companycontact/1E98B65B-C56D-470D-B509-148AC693A013
        [HttpDelete("companycontact/{id:Guid}")]
        public async Task<ActionResult> DeleteCompanyContact(Guid id)
        {
            try
            {
                await _repository.DeleteCompanyContact(id);
                return NoContent();
            }
            catch
            {
                // TODO: Log exception
                return BadRequest();
            }
        }
    }
}
