using TaskManagment.IRepoServices;
using TaskManagment.Models;

namespace TaskManagment.RepoServices
{
    public class TaskServices : ITaskServices
    {
        private readonly TaskDBContext Context;

        public TaskServices(TaskDBContext taskContext)
        {
            Context = taskContext;
        }
        public void DeleteTask(int id)
        {
            Tasks deletedTask = Context.Tasks.FirstOrDefault(t => t.ID == id);
            if (deletedTask != null)
            {
                Context.Tasks.Remove(deletedTask);
                Context.SaveChanges();
            }
        }

        public void InsertTask(Tasks dbt)
        {

            if (dbt != null)
            {
                Context.Tasks.Add(dbt);
                Context.SaveChanges();
            }
        }

        public List<Tasks> SelectAllTasks()
        {
            return Context.Tasks.ToList();
        }

        public Tasks SelectTaskByID(int id)
        {
            return Context.Tasks.FirstOrDefault(t => t.ID == id);
        }

        public void UpdateTask(Tasks dbt)
        {
            Tasks upd = Context.Tasks.FirstOrDefault(t => t.ID == dbt.ID);
            if (upd != null)
            {
                upd.TaskName = dbt.TaskName;
                upd.TaskDate = dbt.TaskDate;
                upd.Description = dbt.Description;
                upd.Completed = dbt.Completed;
                  
                Context.SaveChanges();
            }
        }
    }
}
