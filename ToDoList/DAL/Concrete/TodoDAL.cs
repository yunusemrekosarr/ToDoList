using ToDoList.DAL.Abstract;
using ToDoList.Models;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;
using ToDoList.Models.TodoVİewModels;


namespace ToDoList.DAL.Concrete
{
    public class TodoDAL : ITodoDAL
    {
        readonly IOptions<ConnectionString> _connectionString;

        public TodoDAL(IOptions<ConnectionString> connectionString)
        {
            _connectionString = connectionString;
        }

        public string AddTodo(Todo todo)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString.Value.SQLConnectionString))
            {
                try
                {
                    connection.Open();

                    var command = new SqlCommand(
                            "INSERT INTO Todos (user_id, title, expired_on, description) OUTPUT INSERTED.id VALUES (@userId, @title, @expired_on, @description)",
                            connection);

                    command.Parameters.AddWithValue("@userId", new Guid("91587BEF-0EE6-4D93-B0F7-0DEB0DB6D72A"));
                    command.Parameters.AddWithValue("@title", todo.Title);
                    command.Parameters.AddWithValue("@expired_on", todo.ExpiredOn == null ? DBNull.Value : todo.ExpiredOn);
                    command.Parameters.AddWithValue("@description", todo.Description == null ? DBNull.Value : todo.Description);

                    string? row = command.ExecuteScalar().ToString();

                    if (row == null)
                    {
                        return "false";

                    }
                    return row;

                }
                catch (Exception e)
                {
                    return "false";
                }

            }
        }

        public bool DeleteTodo(Guid Id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString.Value.SQLConnectionString))
            {
                try
                {
                    connection.Open();

                    var command = new SqlCommand("Update todos SET is_active = 0 where id = @id", connection);

                    command.Parameters.AddWithValue("@id", Id);

                    int row = command.ExecuteNonQuery();

                    if (row == 0)
                    {
                        return false;
                    }

                    return true;

                }
                catch (Exception e)
                {
                    return false;
                }

            }

        }

        public bool ToggleTodo(Guid Id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString.Value.SQLConnectionString))
            {
                try
                {
                    connection.Open();

                    var command = new SqlCommand("UPDATE todos SET is_completed = is_completed ^ 1 where id = @id", connection);

                    command.Parameters.AddWithValue("@id", Id);

                    int row = command.ExecuteNonQuery();

                    if (row == 0)
                    {
                        return false;
                    }

                    return true;

                }
                catch (Exception e)
                {
                    return false;
                }

            }
        }

        public Todo GetTodoById(Guid TodoId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString.Value.SQLConnectionString))
            {
                try
                {
                    connection.Open();

                    var command = new SqlCommand("Select [id], [title], [is_completed], [created_on], [updated_on], [expired_on], [description] from todos where id = @TodoId and is_active = 1", connection);

                    command.Parameters.AddWithValue("@TodoId", TodoId);

                    var reader = command.ExecuteReader();

                    if (!reader.HasRows)
                        return null;

                    reader.Read();
                    Todo todo = new Todo();
                    todo.Id = reader.GetGuid(0);
                    todo.Title = reader.GetString(1);
                    todo.IsCompleted = reader.GetBoolean(2);
                    todo.CreatedOn = reader.GetDateTime(3);
                    todo.UpdatedOn = reader.GetDateTime(4);
                    todo.ExpiredOn = reader.IsDBNull(5) ? null : reader.GetDateTime(5);
                    todo.Description = reader.IsDBNull(5) ? null : reader.GetString(6);

                    return todo;
                }
                catch (Exception e)
                {
                    return null;
                }
            }
        }

        public List<Todo>? GetUsersTodos(Guid UserId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString.Value.SQLConnectionString))
            {
                try
                {
                    connection.Open();

                    var command = new SqlCommand("Select [id], [title], [is_completed], [expired_on], [created_on],[updated_on],[description] from todos where is_active = 1 and user_id = @UserId order by created_on desc", connection);

                    command.Parameters.AddWithValue("@UserId", UserId);

                    var reader = command.ExecuteReader();

                    List<Todo> Todos = new List<Todo>();


                    while (reader.Read())
                    {
                        Todo todo = new Todo();
                        todo.Id = reader.GetGuid(0);
                        todo.Title = reader.GetString(1);
                        todo.IsCompleted = reader.GetBoolean(2);
                        todo.ExpiredOn = reader.IsDBNull(3) ? null : reader.GetDateTime(3);
                        todo.CreatedOn = reader.GetDateTime(4);
                        todo.UpdatedOn = reader.GetDateTime(5);
                        todo.Description = reader.IsDBNull(6) ? null : reader.GetString(6);

                        Todos.Add(todo);
                    }

                    return Todos;

                }
                catch (Exception e)
                {
                    return null;
                }

            }
        }

        public bool UpdateTodo(Todo todo)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString.Value.SQLConnectionString))
            {
                try
                {
                    connection.Open();

                    var command = new SqlCommand("Update todos SET  title = @title, description = @description, updated_on = @UpdatedOn, expired_on = @ExpiredOn  where id = @id", connection);

                    command.Parameters.AddWithValue("@title", todo.Title);
                    command.Parameters.AddWithValue("@description", todo.Description == null ? DBNull.Value : todo.Description);
                    command.Parameters.AddWithValue("@UpdatedOn", DateTime.Now);
                    command.Parameters.AddWithValue("@ExpiredOn", todo.ExpiredOn == null ? DBNull.Value : todo.ExpiredOn);
                    command.Parameters.AddWithValue("@id", todo.Id);

                    int row = command.ExecuteNonQuery();

                    if (row == 0)
                    {
                        return false;
                    }

                    return true;

                }
                catch (Exception e)
                {
                    return false;
                }

            }
        }

        public Todo GetLastTodo(Guid UserId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString.Value.SQLConnectionString))
            {
                try
                {
                    connection.Open();

                    var command = new SqlCommand("Select TOP(1) [id], [title], [is_completed], [created_on], [updated_on], [expired_on], [description] from todos where user_id = @user_id and is_active = 1 ORDER BY created_on desc", connection);
                    command.Parameters.AddWithValue("@user_id", UserId);

                    var reader = command.ExecuteReader();

                    if (!reader.HasRows)
                        return null;

                    Todo todo = new Todo();
                    reader.Read();
                    todo.Id = reader.GetGuid(0);
                    todo.Title = reader.GetString(1);
                    todo.IsCompleted = reader.GetBoolean(2);
                    todo.CreatedOn = reader.GetDateTime(3);
                    todo.UpdatedOn = reader.GetDateTime(4);
                    todo.ExpiredOn = reader.IsDBNull(5) ? null : reader.GetDateTime(5);
                    todo.Description = reader.IsDBNull(5) ? null : reader.GetString(6);

                    return todo;

                }
                catch (Exception e)
                {
                    return null;
                }

            }
        }
    }
}
