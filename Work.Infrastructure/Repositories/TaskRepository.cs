using Microsoft.EntityFrameworkCore;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Work.Core.Interfaces;

namespace Work.Infrastructure.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly IDbContextFactory<Data.WorkActivityContext> _contextFactory;

        public TaskRepository(IDbContextFactory<Data.WorkActivityContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<ServiceResult<IEnumerable<Core.Models.Task>>> GetAll()
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                var result = new ServiceResult<IEnumerable<Core.Models.Task>>();
                try
                {
                    result.Data = await context.Tasks.Include(x=>x.Sprints).ToListAsync();
                }
                catch (Exception ex)
                {
                    result.Message = ex.ToString();
                    result.Success = false;
                }
                return result;
            }
        }

        public async Task<ServiceResult<Core.Models.Task>> Get(int id)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                var result = new ServiceResult<Core.Models.Task>();
                try
                {
                    var task = await context.Tasks.Include(x=>x.Sprints).FirstOrDefaultAsync(x => x.Id == id);
                    if (task != null)
                    {
                        result.Data = task;
                    }
                    else
                    {
                        result.Message = $"Item id:{id} not found";
                        result.Success = false;
                    }
                }
                catch (Exception ex)
                {
                    result.Message = ex.ToString();
                    result.Success = false;
                }
                return result;
            }
        }

        public async Task<ServiceResult<IEnumerable<Core.Models.Task>>> Create(Core.Models.Task entity)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                var result = new ServiceResult<IEnumerable<Core.Models.Task>>();
                try
                {
                    if (await context.Tasks.AnyAsync(x => x.Name == entity.Name))
                    {
                        result.Success = false;
                        result.Message = "Task already exists.";
                    }
                    else
                    {
                        context.Tasks.Add(entity);
                        await context.SaveChangesAsync();
                        result.Data = await context.Tasks.Include(x => x.Sprints).ToListAsync();
                    }
                }
                catch (Exception ex)
                {
                    result.Message = ex.ToString();
                    result.Success = false;
                }
                return result;
            }
        }

        public async Task<ServiceResult<Core.Models.Task>> Update(Core.Models.Task entity)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                var result = new ServiceResult<Core.Models.Task>();
                try
                {
                    context.Tasks.Update(entity);
                    await context.SaveChangesAsync();
                    result.Data = entity;
                }
                catch (Exception ex)
                {
                    result.Message = ex.ToString();
                    result.Success = false;
                }
                return result;
            }
        }

        public async Task<ServiceResult<Core.Models.Task>> Delete(int id)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                var result = new ServiceResult<Core.Models.Task>();
                try
                {
                    var task = await context.Tasks.FirstOrDefaultAsync(x => x.Id == id);
                    if (task != null)
                    {
                        context.Remove(task);
                        await context.SaveChangesAsync();
                    }
                    else
                    {
                        result.Message = $"Item id:{id} not found";
                        result.Success = false;
                    }
                }
                catch (Exception ex)
                {
                    result.Message = ex.ToString();
                    result.Success = false;
                }
                return result;
            }
        }
    }
}