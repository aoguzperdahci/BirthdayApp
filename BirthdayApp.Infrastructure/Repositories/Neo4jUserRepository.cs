using BirthdayApp.Core.Entities;
using BirthdayApp.Core.Repositories;
using Neo4jClient;

namespace BirthdayApp.Infrastructure.Repositories
{
    public class Neo4jUserRepository : IUserRepository
    {
        private readonly IGraphClient _client;

        public Neo4jUserRepository(IGraphClient client)
        {
            _client = client;
        }

        public async Task AddAsync(User entity)
        {
            var create = _client.Cypher
                        .Create("(user:User $param)")
                        .WithParam("param", entity);

            await create.ExecuteWithoutResultsAsync();
        }

        public async Task FollowUserAsync(string username, string followUsername)
        {
            var followUser = _client.Cypher
                        .Match("(follow:User), (user:User)")
                        .Where("follow.Username = $fUsername")
                        .AndWhere("user.Username = $uUsername")
                        .WithParam("fUsername", followUsername)
                        .WithParam("uUsername", username)
                        .Create("(user)-[:FOLLOWS]->(follow)");

            await followUser.ExecuteWithoutResultsAsync();
        }

        public async ValueTask<IEnumerable<User>> GetAllAsync()
        {
            var getAll = _client.Cypher
                  .Match("(user:User)")
                  .Return(user => user.As<User>());

            var result = await getAll.ResultsAsync;

            return result;
        }

        public async ValueTask<User> GetByIdAsync(long id)
        {
            var getById = _client.Cypher
                          .Match("(user:User)")
                          .Where("ID(user) = $userID")
                          .WithParam("userID", id)
            .Return(user => user.As<User>());

            var result = await getById.ResultsAsync;

            return result.FirstOrDefault();
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            var getByUsername = _client.Cypher
                              .Match("(user:User)")
                              .Where("user.Username = $username")
                              .WithParam("username", username)
                              .Return(user => user.As<User>());

            var result = await getByUsername.ResultsAsync;

            return result.FirstOrDefault();
        }

        public async Task<IEnumerable<User>> GetFollowedByUserAsync(string username)
        {
            var follows = _client.Cypher
                          .Match("(user:User)-[:FOLLOWS]->(friend:User)")
                          .Where("user.Username = $username")
                          .WithParam("username", username)
                          .Return((user, friend) => friend.As<User>());

            var result = await follows.ResultsAsync;

            return result;
        }

        public async Task RemoveAsync(User entity)
        {
            var remove = _client.Cypher
                         .Match("(user:User)")
                         .Where("user.Username = $username")
                         .WithParam("username", entity.Username)
                         .DetachDelete("user");

            await remove.ExecuteWithoutResultsAsync();
        }

        public async Task UpdateAsync(User entity)
        {
            var update = _client.Cypher
              .Match("(user:User)")
              .Where("user.Username = $username")
              .Set("user = $user")
              .WithParam("username", entity.Username)
              .WithParam("user", entity);

            await update.ExecuteWithoutResultsAsync();
        }

        public async Task SetUserCity(User user, City city)
        {
            var remove = _client.Cypher
                         .Match("(user:User)-[r:RESIDE_IN]->(city:City)")
                         .Where("user.Username = $username")
                         .WithParam("username", user.Username)
                         .Delete("r");

            await remove.ExecuteWithoutResultsAsync();

            var create = _client.Cypher
                         .Match("(user:User), (city:City)")
                         .Where("user.Username = $username")
                         .AndWhere("city.Name = $name")
                         .AndWhere("city.Country = $country")
                         .Create("(user)-[:RESIDE_IN]->(city)")
                         .WithParam("username", user.Username)
                         .WithParam("name", city.Name)
                         .WithParam("country", city.Country);

            await create.ExecuteWithoutResultsAsync();
        }

        public async ValueTask<IEnumerable<string>> GetUserRoles(User user)
        {
            var getByUsername = _client.Cypher
                  .Match("(user:User)")
                  .Where("user.Username = $username")
                  .WithParam("username", user.Username)
                  .Return(user => user.Labels());

            var result = await getByUsername.ResultsAsync;

            return result.FirstOrDefault();
        }

        public async ValueTask<IEnumerable<UserCity>> GetAllUsersWithCityAsync()
        {
            var getAll = _client.Cypher
                  .Match("(user:User)-[:RESIDE_IN]->(city:City)")
                  .Return((user, city) => new UserCity { User = user.As<User>(), City = city.As<City>() });

            var result = await getAll.ResultsAsync;

            return result;
        }

        public async ValueTask<bool> CheckIfFollowsAsync(string username, string followUsername)
        {
            var follows = _client.Cypher
              .Match("(user:User)-[:FOLLOWS]->(friend:User)")
              .Where("user.Username = $username")
              .AndWhere("friend.Username = $fusername")
              .WithParam("username", username)
              .WithParam("fusername", followUsername)
              .Return((user, friend) => friend.As<User>());

            var result = await follows.ResultsAsync;

            return result.ToList().Count != 0;
        }

        public async Task UnfollowUserAsync(string username, string followUsername)
        {
            var unfollow = _client.Cypher
                          .Match("(user:User)-[r:FOLLOWS]->(friend:User)")
                          .Where("user.Username = $username")
                          .AndWhere("friend.Username = $fusername")
                          .WithParam("username", username)
                          .WithParam("fusername", followUsername)
                          .Delete("r");

            await unfollow.ExecuteWithoutResultsAsync();
        }
    }
}