using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Work.Core.Interfaces;

namespace WorkActivity.WPF.Stores
{
    public class WorkStore
    {
        private readonly IWorkRepository _workRepository;
        private readonly Lazy<Task> _initialize;
        private readonly List<Work.Core.Models.Work> _works;

        public IEnumerable<Work.Core.Models.Work> Works => _works;

        public WorkStore(IWorkRepository workRepository)
        {
            _workRepository = workRepository;
            _initialize = new Lazy<Task>(Initialize);
            _works = new List<Work.Core.Models.Work>();
        }

        private async Task Initialize()
        {
            var result = await _workRepository.GetAll();
            if (result.Success)
            {
                _works.Clear();
                _works.AddRange(result.Data);
            }
        }

        public async Task Load()
        {
            await _initialize.Value;
        }

        public async Task<ServiceResult<IEnumerable<Work.Core.Models.Work>>> Create(Work.Core.Models.Work task)
        {
            var result = await _workRepository.Create(task);
            if (result.Success)
            {
                _works.Clear();
                _works.AddRange(result.Data);
            }
            return result;
        }

        public async Task<ServiceResult<Work.Core.Models.Work>> Update(Work.Core.Models.Work task)
        {
            var result = await _workRepository.Update(task);
            if (result.Success)
            {
                _works.Remove(task);
                _works.Add(result.Data);
            }
            return result;
        }

        public async Task<ServiceResult<Work.Core.Models.Work>> Delete(int id)
        {
            var result = await _workRepository.Delete(id);
            if (result.Success)
            {
                var task = _works.FirstOrDefault(x => x.Id == id);
                if (task != null)
                {
                    _works.Remove(task);
                }
            }
            return result;
        }
    }
}