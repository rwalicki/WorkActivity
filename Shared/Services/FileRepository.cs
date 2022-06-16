using Newtonsoft.Json;
using Shared.Interfaces;
using Shared.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Shared.Services
{
    public class FileRepository<T> : IFileService<T> where T : BaseEntity
    {
        private readonly string _filePath;
        private readonly string _path;

        public FileRepository(string path)
        {
            _path = path;
            _filePath = _path + Path.DirectorySeparatorChar + $"{typeof(T).Name}.json".ToLower();

            if (!File.Exists(_filePath))
            {
                File.Create(_filePath);
            }
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await Read();
        }

        public async Task<T> Get(int id)
        {
            var elements = await GetAll();
            return elements.Where(x => x.Id == id).FirstOrDefault();
        }

        public async Task<IEnumerable<T>> Create(T entity)
        {
            var entities = (await Read()).ToList();

            var id = 0;
            if (entities.Any())
            {
                id = entities.Max(x => x.Id);
                entity.Id = ++id;
            }
            else
            {
                entity.Id = id;
            }

            entities.Add(entity);

            await Save(entities);

            return entities;
        }

        public async Task<T> Update(T entity)
        {
            var entities = (await Read()).ToList();

            var foundEntity = entities.Where(x => x.Id == entity.Id).First();
            entities = entities.Except(new List<T>() { foundEntity }).ToList();
            entities.Add(entity);

            await Save(entities);

            return entity;
        }

        public async Task<T> Delete(int id)
        {
            var entities = (await Read()).ToList();
            var entity = entities.FirstOrDefault(x => x.Id == id);
            if (entity != null)
            {
                entities = entities.Except(new List<T>() { entity }).ToList();
                await Save(entities);
                return entity;
            }
            return null;
        }

        private async Task Save(IEnumerable<T> entities)
        {
            await Task.Run(() =>
            {
                using (var stream = new StreamWriter(_filePath))
                {
                    entities = entities.OrderBy(x => x.Id);
                    var content = JsonConvert.SerializeObject(entities);
                    stream.Write(content);
                }
            });
        }

        private async Task<IEnumerable<T>> Read()
        {
            return await Task.Run(() =>
            {
                using (var stream = new StreamReader(_filePath))
                {
                    var content = stream.ReadToEnd();
                    var entities = JsonConvert.DeserializeObject<IEnumerable<T>>(content);
                    entities = entities?.OrderBy(x => x.Id);
                    return entities ?? new List<T>();
                }
            });
        }
    }
}