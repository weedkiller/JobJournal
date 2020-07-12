﻿using JobJournal.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobJournal.Server.Data
{
    public class CompanyContactRepository : ICompanyContactRepository
    {
        private readonly JobJournalContext _db;

        public CompanyContactRepository(JobJournalContext context)
        {
            _db = context;
        }

        public async Task<CompanyContact> AddCompanyContact(CompanyContact companyContact)
        {
            var newContact = _db.CompanyContacts.Add(companyContact);
            await _db.SaveChangesAsync();

            return newContact.Entity;
        }

        public async Task DeleteCompanyContact(Guid contactId)
        {
            var contact = await _db.CompanyContacts.FindAsync(contactId);
            if (contact == null) return;

            _db.CompanyContacts.Remove(contact);
            await _db.SaveChangesAsync();
        }

        public async Task<CompanyContact> GetCompanyContact(Guid contactId)
        {
            return await _db.CompanyContacts.FirstAsync(c => c.Id == contactId);
        }

        public IQueryable<CompanyContact> GetContactsForCompany(Guid companyId)
        {
            return _db.CompanyContacts.Where(c => c.CompanyId == companyId);
        }

        public async Task<CompanyContact> UpdateCompanyContact(CompanyContact companyContact)
        {
            if (companyContact == null) return null;
            var contactToUpdate = await _db.CompanyContacts.FindAsync(companyContact.Id);

            contactToUpdate.FullName = companyContact.FullName;
            contactToUpdate.EmailAddress = companyContact.EmailAddress;
            contactToUpdate.PhoneNumber = companyContact.PhoneNumber;
            contactToUpdate.FirstContactDate = companyContact.FirstContactDate;
            contactToUpdate.MostRecentContactDate = companyContact.MostRecentContactDate;

            var updatedContact = _db.CompanyContacts.Update(contactToUpdate);
            await _db.SaveChangesAsync();

            return updatedContact.Entity;
        }
    }
}