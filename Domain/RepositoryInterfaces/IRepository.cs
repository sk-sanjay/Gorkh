using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace Domain.RepositoryInterfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<List<TEntity>> Get();
        Task<List<TEntity>> GetActive();
        Task<TEntity> Get(int id);
        Task<TEntity> Get(string id);
        Task<List<TEntity>> GetMultiple(string ids);
        void Create(TEntity entity);
        //void CreateSkillCourse(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        void CreateRange(IEnumerable<TEntity> entities);
        void UpdateRange(IEnumerable<TEntity> entities);
        void DeleteRange(IEnumerable<TEntity> entities);
        EntityEntry<TEntity> GetEntityEntry(TEntity entity);
        EntityState GetEntityState(TEntity entity);
        void SetEntityState(TEntity entity, EntityState state);
        Task<List<TEntity>> GetListFromSql(string procName, List<KeyValuePair<string, string>> values);
        //Task<List<TEntity>> GetListFMFromSql(string procName);
        int GetFlagFromSql(string procName, List<KeyValuePair<string, string>> values);
    }
}
