using TaskManagment.Models;

namespace TaskManagment.IRepoServices
{
    public interface ITaskServices
    {
        public List<Tasks> SelectAllTasks();
        public Tasks SelectTaskByID(int id);
        public void InsertTask(Tasks dbt);
        public void UpdateTask(Tasks dbt);
        public void DeleteTask(int id);
    }
}
