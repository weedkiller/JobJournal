﻿using JobJournal.Client;
using JobJournal.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobJournal.Server.Data
{
    public class JobApplicationRepository : IJobApplicationRepository
    {
        private readonly JobJournalContext _db;

        public JobApplicationRepository(JobJournalContext context)
        {
            _db = context;
        }

        public async Task<JobApplication> AddJobApplication(JobApplication application)
        {
            application.ApplicationMethod = await _db.ApplicationMethods.FindAsync(application.ApplicationMethodId);
            application.ApplicationStatus = await _db.ApplicationStatuses.FindAsync(application.ApplicationStatusId);
            application.Company = await _db.Companies.FindAsync(application.CompanyId);

            var newApplication = _db.JobApplications.Add(application);
            await _db.SaveChangesAsync();

            return newApplication.Entity;
        }

        public async Task DeleteJobApplication(Guid applicationId)
        {
            var application = await _db.JobApplications.FindAsync(applicationId);
            if (application == null) return;

            _db.JobApplications.Remove(application);
            await _db.SaveChangesAsync();
        }

        public async Task<JobApplication> GetJobApplication(Guid applicationId)
        {
            return await _db.JobApplications.FindAsync(applicationId);
        }

        public IQueryable<JobApplication> GetJobApplicationsForUser(Guid userId)
        {
            return _db.JobApplications.Where(j => j.UserId == userId);
        }

        public IQueryable<JobApplication> GetJobApplicationsForCompany(Guid companyId)
        {
            return _db.JobApplications.Where(j => j.CompanyId == companyId);
        }

        public async Task<JobApplication> UpdateJobApplication(JobApplication application)
        {
            if (application == null) return null;
            var applicationToUpdate = await _db.JobApplications.FindAsync(application.Id);

            applicationToUpdate.CompanyId = application.CompanyId;
            applicationToUpdate.JobTitle = application.JobTitle;
            applicationToUpdate.ApplicationDate = application.ApplicationDate;
            applicationToUpdate.ApplicationMethod = application.ApplicationMethod;
            applicationToUpdate.ApplicationStatus = application.ApplicationStatus;

            var updatedApplication = _db.JobApplications.Update(applicationToUpdate);
            await _db.SaveChangesAsync();

            return updatedApplication.Entity;
        }
    }
}
