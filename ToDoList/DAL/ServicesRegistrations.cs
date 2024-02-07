using ToDoList.DAL.Abstract;
using ToDoList.DAL.Concrete;
using Microsoft.AspNetCore.Identity;

namespace ToDoList.DAL
{
    public static class ServicesRegistrations
    {
        public static void AddDataAccesLayerService(this IServiceCollection services)
        {
            services.AddScoped<ITodoDAL, TodoDAL>();
        }       
    }
}
